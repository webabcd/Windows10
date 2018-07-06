/*
 * 演示后台任务的应用（后台任务与 app 相同进程）
 * 
 * 注：
 * 1、后台任务与 app 不同进程，详见 /BackgroundTask/Demo.xaml.cs
 * 2、此示例不需要在 Package.appxmanifest 添加“后台任务”声明
 * 3、此示例不需要引用后台任务项目，而是在 App.xaml.cs 中通过 override void OnBackgroundActivated(BackgroundActivatedEventArgs args) 来编写后台任务的逻辑
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.BackgroundTask
{
    public sealed partial class DemoInProcess : Page
    {
        // 所注册的后台任务的名称
        private string _taskName = "DemoInProcess";

        public DemoInProcess()
        {
            this.InitializeComponent();
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


            // 如果任务已注册，则注销
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> t in BackgroundTaskRegistration.AllTasks)
            {
                if (t.Value.Name == _taskName)
                {
                    t.Value.Unregister(true);
                }
            }

            // 注册后台任务，后台任务的代码参见 App.xaml.cs 中的 OnBackgroundActivated(BackgroundActivatedEventArgs args) 方法
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder
            {
                Name = _taskName,
            };
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            BackgroundTaskRegistration task = builder.Register();
        }
    }
}
