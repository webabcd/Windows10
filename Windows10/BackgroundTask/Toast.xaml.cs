/*
 * 演示如何通过 toast 激活后台任务
 * 
 * 注：
 * 1、需要引用后台任务项目，相关代码参见 BackgroundTaskLib/BackgroundTaskToast.cs
 * 2、需要在 Package.appxmanifest 添加“后台任务”声明，支持的任务类型选择“常规”，并指定 EntryPoint（后台任务的类全名），类似如下：
 * <Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskToast">
 *   <BackgroundTasks>
 *     <Task Type="general" />
 *   </BackgroundTasks>
 * </Extension>
 * 
 * 
 * 本例的 toast 的 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，background 代表后台方式激活
 * 其他 toast 的相关知识点请参见：/Notification/Toast/
 */

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.BackgroundTask
{
    public sealed partial class Toast : Page
    {
        private string _taskName = "Toast";
        private string _taskEntryPoint = "BackgroundTaskLib.BackgroundTaskToast";

        public Toast()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);


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

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder
            {
                Name = _taskName,
                TaskEntryPoint = _taskEntryPoint
            };
            // 指定后台任务的触发器类型为 ToastNotificationActionTrigger（即通过 toast 激活）
            builder.SetTrigger(new ToastNotificationActionTrigger());
            // 注册后台任务
            BackgroundTaskRegistration task = builder.Register();
        }

        // 弹出 toast 通知（点击 toast 框或点击 toast 中的按钮则可激活后台任务）
        private void buttonShowToast_Click(object sender, RoutedEventArgs e)
        {
            string toastXml = @"
                <toast activationType='background' launch='launch arguments'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content</text>
                        </binding>
                    </visual>
                    <actions>
                        <input type='text' id='message1' title='title1' />
                        <action content='确认' activationType='background' arguments='action arguments'/>
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
