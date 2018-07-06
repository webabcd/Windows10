/*
 * 演示如何自定义启动屏幕（闪屏）
 * 
 * 说明及应用场景：
 * 1、在 Package.appxmanifest 中可以设置 app 的启动屏幕，例如: <uap:SplashScreen Image="Assets\SplashScreen.png" BackgroundColor="#ff0000" />，其就是一个显示在窗口中间的图片（620 * 300）以及一个全窗口背景色
 * 2、在 app 的启动屏幕过后，可以显示一个自定义启动屏幕
 * 3、在自定义启动屏幕显示时，可以做一些程序的初始化工作，初始化完成后再进入主程序
 */

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10
{
    using Windows10.UI;

    public partial class App
    {
        // partial method，实现了 App.xaml.cs 中的声明
        partial void OnLaunched_SplashScreen(LaunchActivatedEventArgs args)
        {
            // 启动后显示自定义启动屏幕
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                MySplashScreen mySplashScreen = new MySplashScreen(args);
                Window.Current.Content = mySplashScreen;
            }
        }
    }
}

namespace Windows10.UI
{
    public sealed partial class MySplashScreen : Page
    {
        /*
         * SplashScreen - app 的启动屏幕对象，在 Application 中的若干事件处理器中的事件参数中均可获得
         *     ImageLocation - app 的启动屏幕的图片的位置信息，返回 Rect 类型对象
         *     Dismissed - app 的启动屏幕关闭时所触发的事件
         */

        // app 启动屏幕的相关信息
        private SplashScreen _splashScreen;

        public MySplashScreen()
        {
            this.InitializeComponent();

            lblMsg.Text = "自定义 app 的启动屏幕，打开 app 时可看到此页面的演示";
        }

        public MySplashScreen(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();
            
            ImplementCustomSplashScreen(args);
        }

        private async void ImplementCustomSplashScreen(LaunchActivatedEventArgs args)
        {
            // 窗口尺寸发生改变时，重新调整自定义启动屏幕
            Window.Current.SizeChanged += Current_SizeChanged;

            // 获取 app 的启动屏幕的相关信息
            _splashScreen = args.SplashScreen;

            // app 的启动屏幕关闭时所触发的事件
            _splashScreen.Dismissed += _splashScreen_Dismissed;

            // 获取 app 启动屏幕的图片的位置，并按此位置调整自定义启动屏幕的图片的位置
            Rect splashImageRect = _splashScreen.ImageLocation;
            splashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            splashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            splashImage.Height = splashImageRect.Height;
            splashImage.Width = splashImageRect.Width;

            await Task.Delay(1000);

            // 关掉自定义启动屏幕，跳转到程序主页面
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage), args.Arguments);
            if (!string.IsNullOrEmpty(args.Arguments))
            {
                await new MessageDialog("启动参数: " + args.Arguments).ShowAsync();
            }
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // 获取 app 启动屏幕的图片的最新位置，并按此位置调整自定义启动屏幕的图片的位置
            Rect splashImageRect = _splashScreen.ImageLocation;
            splashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            splashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            splashImage.Height = splashImageRect.Height;
            splashImage.Width = splashImageRect.Width;
        }

        private void _splashScreen_Dismissed(SplashScreen sender, object args)
        {
            // app 的启动屏幕关闭了
        }
    }
}