/*
 * 演示“窗口全屏”相关知识点
 *
 * ApplicationView - 用于操作窗口以及获取窗口信息
 *     GetForCurrentView() - 返回 ApplicationView 实例
 */

using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UI
{
    public sealed partial class FullScreen : Page
    {
        private MainPage _rootPage;

        public FullScreen()
        {
            this.InitializeComponent();

            this.Loaded += FullScreen_Loaded;
        }

        private void FullScreen_Loaded(object sender, RoutedEventArgs e)
        {
            /*
             * ApplicationView.PreferredLaunchWindowingMode - 窗口的启动模式
             *     Auto - 系统自动决定
             *     PreferredLaunchViewSize - 由 ApplicationView.PreferredLaunchViewSize 决定（参见：UI/ScreenSize.xaml）
             *     FullScreen - 全屏启动
             *
             * ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode - 全屏状态下的，系统边缘手势的响应模式
             *     Standard - 标准方式。比如鼠标移动到顶端显示标题栏，移动到底端显示任务栏
             *     Minimal - 最小方式。比如鼠标移动到顶端显示一个小的临时 UI，移动到底端显示一个小的临时 UI，点击这个临时 UI 时再显示标题栏或任务栏
             */

            // unchecked:FullScreenSystemOverlayMode.Standard, checked:FullScreenSystemOverlayMode.Minimal
            chkFullScreenSystemOverlayMode.IsChecked = ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode == FullScreenSystemOverlayMode.Minimal;

            // unchecked:ApplicationViewWindowingMode.Auto, unchecked:ApplicationViewWindowingMode.FullScreen
            chkPreferredLaunchWindowingMode.IsChecked = ApplicationView.PreferredLaunchWindowingMode == ApplicationViewWindowingMode.FullScreen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _rootPage = MainPage.Current;
            _rootPage.KeyDown += _rootPage_KeyDown;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _rootPage.KeyDown -= _rootPage_KeyDown;
        }

        private void _rootPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // 判断是否按下了 escape 键
            if (e.Key == VirtualKey.Escape)
            {
                var view = ApplicationView.GetForCurrentView();
                if (view.IsFullScreenMode)
                {
                    // 退出全屏状态
                    view.ExitFullScreenMode();
                }
            }
        }

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
        }

        private void btnShowStandardSystemOverlays_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            // 在全屏状态下，是否显示系统 UI，比如标题栏和任务栏
            view.ShowStandardSystemOverlays();
        }

        private void chkFullScreenSystemOverlayMode_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView(); 
            view.FullScreenSystemOverlayMode = chkFullScreenSystemOverlayMode.IsChecked.Value ? FullScreenSystemOverlayMode.Minimal : FullScreenSystemOverlayMode.Standard;
        }

        private void chkPreferredLaunchWindowingMode_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.PreferredLaunchWindowingMode = chkPreferredLaunchWindowingMode.IsChecked.Value ? ApplicationViewWindowingMode.FullScreen : ApplicationViewWindowingMode.Auto;
        }
    }
}
