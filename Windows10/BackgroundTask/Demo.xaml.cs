/*
 * 演示后台任务的应用（后台任务与 app 不同进程）
 * 
 * 注：
 * 1、需要引用后台任务项目，相关代码参见 BackgroundTaskLib/BackgroundTaskDemo.cs
 * 2、需要在 Package.appxmanifest 添加“后台任务”声明，支持的任务类型选择“系统事件”，并指定 EntryPoint（后台任务的类全名），类似如下：
 * <Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskDemo">
 *   <BackgroundTasks>
 *     <Task Type="systemEvent" />
 *   </BackgroundTasks>
 * </Extension>
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.BackgroundTask
{
    public sealed partial class Demo : Page
    {
        // 所注册的后台任务的名称
        private string _taskName = "Demo";

        // 所注册的后台任务的 EntryPoint，即后台任务的类全名
        // 对应 Package.appxmanifest 中的“后台任务”声明，类似如下：<Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskDemo" />
        private string _taskEntryPoint = "BackgroundTaskLib.BackgroundTaskDemo";

        // 后台任务是否已在系统中注册
        private bool _taskRegistered = false;

        // 后台任务执行状况的进度说明
        private string _taskProgress = "";

        public Demo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 遍历所有已注册的后台任务（避免重复注册）
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == _taskName)
                {
                    // 如果找到了指定的后台任务，则为其增加 Progress 和 Completed 事件监听，以便前台 app 接收后台任务的进度汇报和完成汇报
                    AttachProgressAndCompletedHandlers(task.Value);
                    _taskRegistered = true;
                    break;
                }
            }

            UpdateUI();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // 在注册后台任务之前，需要调用 BackgroundExecutionManager.RequestAccessAsync()，如果是更新过的 app 则在之前还需要调用 BackgroundExecutionManager.RemoveAccess()
            string appVersion = $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}.{Package.Current.Id.Version.Revision}";
            if ((string)ApplicationData.Current.LocalSettings.Values["AppVersion"] != appVersion)
            {
                // 对于更新的 app 来说先要调用这个方法
                BackgroundExecutionManager.RemoveAccess();
                // 注册后台任务之前先要调用这个方法，并获取 BackgroundAccessStatus 状态
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedBySystemPolicy || status == BackgroundAccessStatus.DeniedByUser)
                {
                    // 无权限注册后台任务

                    await new MessageDialog("没有权限注册后台任务").ShowAsync();
                }
                else
                {
                    // 有权限注册后台任务

                    ApplicationData.Current.LocalSettings.Values["AppVersion"] = appVersion;
                }
            }


            // 用于构造一个后台任务
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();

            builder.Name = _taskName; // 后台任务的名称
            builder.TaskEntryPoint = _taskEntryPoint; // 后台任务入口点，即后台任务的类全名

            /*
             * 后台任务触发器 IBackgroundTrigger
             *     TimeTrigger - 循环触发器，需要 app 在锁屏上，最小周期 15 分钟
             *     MaintenanceTrigger - 循环触发器，与 TimeTrigger 类似，但是不要求 app 在锁屏上，最小周期 15 分钟
             *     SystemTrigger - 一般不要求 app 在锁屏上（实例化时的第二个参数是 oneShot，其代表是否只触发一次）
             *         SmsReceived - 接收到新的 sms 消息时
             *         LockScreenApplicationAdded - app 添加到锁屏时
             *         LockScreenApplicationRemoved - app 从锁屏移除时
             *         OnlineIdConnectedStateChange - 当前连接的 Microsoft 帐户更改时
             *         TimeZoneChange - 时区发生更改时
             *         ServicingComplete - 系统完成了 app 的更新时
             *         ControlChannelReset - 重置 ControlChannel 时，需要 app 在锁屏上
             *         NetworkStateChange - 网络状态发生改变时
             *         InternetAvailable - Internet 变为可用时
             *         SessionConnected - 会话状态连接时，需要 app 在锁屏上
             *             这里的 Session 指的是，用户与本机之间的 Session，也就是说当切换用户时 Session 会发生改变
             *         UserPresent - 用户变为活动状态时，需要 app 在锁屏上
             *         UserAway - 用户变为非活动状态时，需要 app 在锁屏上
             *         PowerStateChange - 当 Windows.System.Power.PowerManager.BatteryStatus 发生变化时
             *     ToastNotificationHistoryChangedTrigger - 当 toast 通知从 ToastNotificationHistory 中添加或删除时
             *     PushNotificationTrigger - 需要 app 在锁屏上。关于“推送通知”请参见：BackgroundTask/PushNotification.xaml
             *     ControlChannelTrigger - 需要 app 在锁屏上。关于“推送通道”请参见：BackgroundTask/ControlChannel.xaml
             */
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));

            /*
             * 后台任务执行条件 SystemConditionType，当后台任务触发器触发后，只有满足了指定的条件才能执行（可以添加多个条件，也可以一个条件都不添加）
             *     UserPresent - 用户为活动状态
             *     UserNotPresent - 用户为非活动状态
             *     InternetAvailable - Internet 状态为可用
             *     InternetNotAvailable - Internet 状态为不可用
             *     SessionConnected - 会话状态是连接的。这里的 Session 指的是，用户与本机之间的 Session，也就是说如果系统中有用户登录则 SessionConnected
             *     SessionDisconnected - 会话状态是断开的。这里的 Session 指的是，用户与本机之间的 Session，也就是说如果系统中没有用户登录（所有用户都注销了）则 SessionDisconnected
             *     FreeNetworkAvailable - 免费网络（比如 wifi）
             *     BackgroundWorkCostNotHigh - 后台任务开销较低
             */
            // builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            // 在后台任务运行过程中，如果发现后台任务执行条件至少有一条不符合要求时，是否取消此后台任务的执行（默认值为 false）
            // builder.CancelOnConditionLoss = false;

            // 设置后台任务的所属组，以便组操作（关于 BackgroundTaskRegistrationGroup 和 BackgroundTaskRegistration 的组相关的操作请参见文档）
            // builder.TaskGroup = new BackgroundTaskRegistrationGroup("myGroup");


            /*
             * 再说明一下
             * SetTrigger() 用于指定什么时候触发后台任务
             * AddCondition() 用于指定在后台任务被触发时，其必须满足什么条件才能被执行
             * CancelOnConditionLoss 用于指定在后台任务的执行过程中，如果发现 AddCondition() 中的条件不满足了，是否要取消此后台任务的执行
             */


            // 向系统注册此后台任务
            BackgroundTaskRegistration task = builder.Register();
            // task.TaskId; 获取此后台任务的标识，一个 GUID（其与后台任务类中的 taskInstance.InstanceId 一致）

            // 为此后台任务增加 Progress 和 Completed 事件监听，以便前台 app 接收后台任务的进度汇报和完成汇报
            AttachProgressAndCompletedHandlers(task);

            _taskRegistered = true;

            UpdateUI();
        }

        private void btnUnregister_Click(object sender, RoutedEventArgs e)
        {
            // 遍历所有已注册的后台任务
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == _taskName)
                {
                    // 从系统中注销指定的后台任务。唯一一个参数代表如果当前后台任务正在运行中，是否需要将其取消
                    task.Value.Unregister(true);
                    break;
                }
            }

            _taskRegistered = false;

            UpdateUI();
        }

        private void AttachProgressAndCompletedHandlers(IBackgroundTaskRegistration task)
        {
            // 为任务增加 Progress 和 Completed 事件监听，以便前台 app 接收后台任务的进度汇报和完成汇报
            task.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
            task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            // 获取后台任务的执行进度
            _taskProgress = args.Progress.ToString();

            UpdateUI();
        }

        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            // 后台任务已经执行完成
            _taskProgress = "完成";

            // 如果此次后台任务的执行出现了错误，则调用 CheckResult() 后会抛出异常
            try
            {
                args.CheckResult();
            }
            catch (Exception ex)
            {
                _taskProgress = ex.ToString();
            }

            UpdateUI();
        }

        private async void UpdateUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                btnRegister.IsEnabled = !_taskRegistered;
                btnUnregister.IsEnabled = _taskRegistered;

                if (_taskProgress != "")
                    lblMsg.Text = "进度：" + _taskProgress;
            });
        }
    }
}
