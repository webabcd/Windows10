/*
 * 演示“窗口尺寸”相关知识点
 *
 * ApplicationView - 用于操作窗口以及获取窗口信息
 *     GetForCurrentView() - 返回 ApplicationView 实例
 */

using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UI
{
    public sealed partial class ScreenSize : Page
    {
        public ScreenSize()
        {
            this.InitializeComponent();

            this.Loaded += ScreenSize_Loaded;
        }

        private void ScreenSize_Loaded(object sender, RoutedEventArgs e)
        {
            // Window.Current.Bounds - 当前窗口的大小（单位是有效像素，没有特别说明就都是有效像素）
            //     注：窗口大小不包括标题栏，标题栏属于系统级 UI
            lblMsg.Text = string.Format("window size: {0} * {1}", Window.Current.Bounds.Width, Window.Current.Bounds.Height);

            ApplicationView applicationView = ApplicationView.GetForCurrentView();

            // SetPreferredMinSize(Size minSize) - 指定窗口允许的最小尺寸（最小：192×48，最大：500×500）
            applicationView.SetPreferredMinSize(new Size(200, 200));

            // PreferredLaunchViewSize - 窗口启动时的初始尺寸
            // 若要使 PreferredLaunchViewSize 设置有效，需要将 ApplicationView.PreferredLaunchWindowingMode 设置为 ApplicationViewWindowingMode.PreferredLaunchViewSize
            ApplicationView.PreferredLaunchViewSize = new Size(800, 480);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            
            /*
             * ApplicationView.PreferredLaunchWindowingMode - 窗口的启动模式
             *     Auto - 系统自动调整
             *     PreferredLaunchViewSize - 由 ApplicationView.PreferredLaunchViewSize 决定
             *     FullScreen - 全屏启动
             */
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 窗口尺寸发生变化时触发的事件
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            lblMsg.Text = string.Format("window size: {0} * {1}", e.Size.Width, e.Size.Height);
        }

        private void btnChangeSize_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            
            Size size = new Size(600, 600);

            // TryResizeView(Size value) - 尝试将窗口尺寸设置为指定的大小
            bool success = applicationView.TryResizeView(size);
            if (success)
            {
                lblMsg.Text = "尝试修改窗口尺寸成功";
            }
            else
            {
                lblMsg.Text = "尝试修改窗口尺寸失败";
            }

            // 注：怎么修改窗口的显示位置呢？暂时不知道
        }
    }
}
