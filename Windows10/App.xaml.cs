using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Windows10
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            /* 此部分为默认模板生成，本例用不到，相关逻辑转到 OnLaunched_SplashScreen(e) 中写了
            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            */

            OnLaunched_MultipleViews(e);
            OnLaunched_SplashScreen(e);

            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }



        // partial method（复习一下 partial method: 必须在 partial class 中定义，一个地方声明，一个地方实现，也可以只有声明没有实现）
        // 具体实现在 /UI/MultipleViews/AppPartial.cs
        partial void OnLaunched_MultipleViews(LaunchActivatedEventArgs args);

        // 具体实现在 /UI/MySplashScreen.xaml.cs
        partial void OnLaunched_SplashScreen(LaunchActivatedEventArgs args);



        // 通过文件打开选取器激活应用程序时所调用的方法
        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(Windows10.Picker.MyOpenPicker), args);
            Window.Current.Content = rootFrame;

            Window.Current.Activate();
        }

        // 通过文件保存选取器激活应用程序时所调用的方法
        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(Windows10.Picker.MySavePicker), args);
            Window.Current.Content = rootFrame;

            Window.Current.Activate();
        }

        // 通过打开文件激活应用程序时所调用的方法
        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            Frame rootFrame = new Frame();
            rootFrame.Navigate(typeof(Windows10.AssociationLaunching.FileTypeAssociation), args);
            Window.Current.Content = rootFrame;

            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // 通过协议激活应用程序时（参见 AssociationLaunching/ProtocolAssociation.xaml.cs 中的示例）
            if (args.Kind == ActivationKind.Protocol)
            {
                ProtocolActivatedEventArgs protocolArgs = args as ProtocolActivatedEventArgs;

                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(Windows10.AssociationLaunching.ProtocolAssociation), protocolArgs);
                Window.Current.Content = rootFrame;
            }

            // 通过可返回结果的协议激活应用程序时（参见 App2AppCommunication/LaunchUriForResults.xaml.cs 中的示例）
            if (args.Kind == ActivationKind.ProtocolForResults)
            {
                ProtocolForResultsActivatedEventArgs protocolForResultsArgs = args as ProtocolForResultsActivatedEventArgs;

                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(Windows10.App2AppCommunication.LaunchUriForResults), protocolForResultsArgs);
                Window.Current.Content = rootFrame;

                Window.Current.Activate();
            }

            // 通过 toast 激活应用程序时（前台方式激活）
            if (args.Kind == ActivationKind.ToastNotification)
            {
                ToastNotificationActivatedEventArgs toastArgs = args as ToastNotificationActivatedEventArgs;

                Frame rootFrame = new Frame();
                rootFrame.Navigate(typeof(Windows10.Notification.Toast.Demo), toastArgs);
                Window.Current.Content = rootFrame;

                Window.Current.Activate();
            }
        }

        // 后台任务与 app 相同进程时，后台任务的实现逻辑
        // 这部分与“后台任务与 app 不同进程”中的后台任务的编写基本一致，详细可参见 /BackgroundTaskLib/BackgroundTaskDemo.cs
        // 后台任务与前台的通信也可以参见“后台任务与 app 不同进程”示例，不过由于本后台任务示例是和 app 同进程的，所以通过内存通信会更简单
        protected async override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            // 获取 IBackgroundTaskInstance 对象
            IBackgroundTaskInstance taskInstance = args.TaskInstance;

            // 异步操作
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // 写入相关数据到文件
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdBackgroundTask\demoInProcess.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.AppendTextAsync(file, "background task in process: " + DateTime.Now.ToString() + Environment.NewLine);

            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }
    }
}
