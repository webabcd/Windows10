/*
 * 演示后台任务的应用（前台程序激活后台任务）
 *
 * 注：
 * 1、需要引用后台任务项目，相关代码参见 BackgroundTaskLib/BackgroundTaskFore.cs
 * 2、需要在 Package.appxmanifest 添加“后台任务”声明，支持的任务类型选择“常规”，并指定 EntryPoint（后台任务的类全名），类似如下：
 * <Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskFore">
 *   <BackgroundTasks>
 *     <Task Type="general" />
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
    public sealed partial class Fore : Page
    {
        // 所注册的后台任务的名称
        private string _taskName = "Fore";

        // 所注册的后台任务的 EntryPoint，即后台任务的类全名
        private string _taskEntryPoint = "BackgroundTaskLib.BackgroundTaskFore";

        // 后台任务是否已在系统中注册
        private bool _taskRegistered = false;

        // 后台任务执行状况的进度说明
        private string _taskProgress = "";

        // 实例化一个 ApplicationTrigger 类型的后台任务触发器（由前台程序通过 api 激活后台任务）
        private ApplicationTrigger applicationTrigger = new ApplicationTrigger();

        public Fore()
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
            builder.SetTrigger(applicationTrigger);  // 指定后台任务的触发器类型为 ApplicationTrigger（由前台程序通过 api 激活后台任务）

            // 向系统注册此后台任务
            BackgroundTaskRegistration task = builder.Register();

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

        private async void btnRequest_Click(object sender, RoutedEventArgs e)
        {
            // 激活后台任务
            await applicationTrigger.RequestAsync();
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
