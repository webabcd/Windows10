/*
 * 演示 TitleBar 相关知识点
 *
 * 通过 ApplicationView.GetForCurrentView().TitleBar 获取当前的 ApplicationViewTitleBar
 * 通过 CoreApplication.GetCurrentView().TitleBar 获取当前的 CoreApplicationViewTitleBar
 *
 * 注：手工测量 TitleBar 高度为 32 像素
 */

using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.UI
{
    public sealed partial class TitleBarDemo : Page
    {
        public TitleBarDemo()
        {
            this.InitializeComponent();

            this.Loaded += TitleBarDemo_Loaded;
        }

        private void TitleBarDemo_Loaded(object sender, RoutedEventArgs e)
        {

        }


        // 改变 Title
        private void btnTitle_Click(object sender, RoutedEventArgs e)
        {
            // 改变左上角 Title 的显示内容
            ApplicationView.GetForCurrentView().Title = $"My Title Bar({new Random().Next(1000, 10000).ToString()})";
        }


        // 改变 TitleBar 上的相关颜色
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            // 获取当前的 ApplicationViewTitleBar
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            if (titleBar.BackgroundColor != Colors.Orange)
            {
                // 背景色
                titleBar.BackgroundColor = Colors.Orange;
                // 前景色
                titleBar.ForegroundColor = Colors.White;
                // 窗口为非活动状态时的背景色（活动状态就是焦点在窗口上，非活动状态就是焦点不在窗口上）
                titleBar.InactiveBackgroundColor = Colors.Yellow;
                // 窗口为非活动状态时的前景色
                titleBar.InactiveForegroundColor = Colors.Gray;

                // TitleBar 上的按钮的背景色（按钮包括：最小化按钮，最大化按钮，关闭按钮，返回按钮）
                titleBar.ButtonBackgroundColor = Colors.Orange;
                // TitleBar 上的按钮的鼠标经过的背景色
                titleBar.ButtonHoverBackgroundColor = Colors.Blue;
                // TitleBar 上的按钮的鼠标按下的背景色
                titleBar.ButtonPressedBackgroundColor = Colors.Green;
                // TitleBar 上的按钮的非活动状态的背景色
                titleBar.ButtonInactiveBackgroundColor = Colors.Yellow;

                // TitleBar 上的按钮的前景色
                titleBar.ButtonForegroundColor = Colors.White;
                // TitleBar 上的按钮的鼠标经过的前景色
                titleBar.ButtonHoverForegroundColor = Colors.Red;
                // TitleBar 上的按钮的鼠标按下的前景色
                titleBar.ButtonPressedForegroundColor = Colors.Purple;
                // TitleBar 上的按钮的非活动状态的前景色
                titleBar.ButtonInactiveForegroundColor = Colors.Gray;
            }
            else
            {
                titleBar.BackgroundColor = null;
                titleBar.ForegroundColor = null;
                titleBar.InactiveBackgroundColor = null;
                titleBar.InactiveForegroundColor = null;

                titleBar.ButtonBackgroundColor = null;
                titleBar.ButtonHoverBackgroundColor = null;
                titleBar.ButtonPressedBackgroundColor = null;
                titleBar.ButtonInactiveBackgroundColor = null;

                titleBar.ButtonForegroundColor = null;
                titleBar.ButtonHoverForegroundColor = null;
                titleBar.ButtonPressedForegroundColor = null;
                titleBar.ButtonInactiveForegroundColor = null;
            }
        }


        // 显示或隐藏 TitleBar 左上角的返回按钮
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // 当前 TitleBar 左上角的返回按钮是隐藏状态
            if (SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Collapsed)
            {
                // 显示 TitleBar 左上角的返回按钮
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                // 监听 TitleBar 左上角的返回按钮的点击事件
                SystemNavigationManager.GetForCurrentView().BackRequested += TitleBarDemo_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= TitleBarDemo_BackRequested;
            }
        }
        // 处理 TitleBar 左上角的返回按钮的点击事件
        private void TitleBarDemo_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MainPage.Current.Container.CanGoBack)
                MainPage.Current.Container.GoBack();
        }


        // 显示或隐藏 TitleBar
        private void btnHidden_Click(object sender, RoutedEventArgs e)
        {
            // 切换 TitleBar 的显示/隐藏状态
            // 注：TitleBar 隐藏时，仍会显示最小化按钮、最大化按钮、关闭按钮
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar ^= true;

            /*
             * 如果需要自定义 TitleBar 的话，就隐藏它，然后自定义一个自己的即可
             * 要注意 TitleBar 的 Height, SystemOverlayLeftInset, SystemOverlayRightInset
             * 要注意如果收到 CoreApplicationViewTitleBar.LayoutMetricsChanged 事件，则 Height, SystemOverlayLeftInset, SystemOverlayRightInset 的值可能发生了变化
             * 要注意窗口大小发生变化时的处理
             */
            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            // Height - TitleBar 的高度
            // SystemOverlayLeftInset - TitleBar 浮层左侧的间隔，在这个间隔部分不要放置自定义内容
            // SystemOverlayRightInset - TitleBar 浮层右侧的间隔，在这个间隔部分不要放置自定义内容（右侧间隔部分是用于放置最小化按钮，最大化按钮，关闭按钮的。经过测试这个间隔明显多出来一些，也许是预留给其他按钮的）
            lblMsg.Text = $"titleBarHeight: {titleBar.Height}, titleBarLeftInset: {titleBar.SystemOverlayLeftInset}, titleBarRightInset: {titleBar.SystemOverlayRightInset}";
        }

        
        // 进入全屏模式，退出全屏模式
        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            // 判断当前是否是全屏模式
            if (view.IsFullScreenMode)
            {
                // 退出全屏模式
                view.ExitFullScreenMode();
                lblMsg.Text = "退出全屏模式";
            }
            else
            {
                // 尝试进入全屏模式
                bool isSuccess = view.TryEnterFullScreenMode();
                if (isSuccess)
                {
                    lblMsg.Text = "进入全屏模式";
                }
                else
                {
                    lblMsg.Text = "尝试进入全屏模式失败";
                }
            }

            // 注意，进入全屏模式后，TitleBar 会消失，鼠标移动到顶部，则 TitleBar 会再次出现（当然这个行为的具体表现取决于 FullScreenSystemOverlayMode，参见 FullScreen.xaml）
            CoreApplicationViewTitleBar titleBar = CoreApplication.GetCurrentView().TitleBar;
            // TitleBar 是否是可见状态
            bool titleBarIsVisible = titleBar.IsVisible; 
            // TitleBar 的可见性发生变化时触发的事件
            titleBar.IsVisibleChanged += delegate { };
        }
    }
}
