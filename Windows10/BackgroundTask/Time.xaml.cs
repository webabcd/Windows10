/*
 * 演示后台任务的应用（定时激活后台任务）
 * 
 * 注：
 * 1、需要引用后台任务项目，相关代码参见 BackgroundTaskLib/BackgroundTaskTime.cs
 * 2、需要在 Package.appxmanifest 添加“后台任务”声明，支持的任务类型选择“计时器”，并指定 EntryPoint（后台任务的类全名），类似如下：
 * <Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskTime">
 *   <BackgroundTasks>
 *     <Task Type="timer" />
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
    public sealed partial class Time : Page
    {
        // 所注册的后台任务的名称
        private string _taskName = "Time";

        // 所注册的后台任务的 EntryPoint，即后台任务的类全名
        private string _taskEntryPoint = "BackgroundTaskLib.BackgroundTaskTime";

        // 后台任务是否已在系统中注册
        private bool _taskRegistered = false;

        public Time()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 遍历所有已注册的后台任务
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == _taskName)
                {
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
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = _taskName,
                TaskEntryPoint = _taskEntryPoint

            };
            // 指定后台任务的触发器类型为 MaintenanceTrigger（定时激活，最小周期 15 分钟）
            builder.SetTrigger(new MaintenanceTrigger(15, false));
            // 注册后台任务
            BackgroundTaskRegistration task = builder.Register();
            
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


        private async void UpdateUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                btnRegister.IsEnabled = !_taskRegistered;
                btnUnregister.IsEnabled = _taskRegistered;
            });
        }
    }
}
