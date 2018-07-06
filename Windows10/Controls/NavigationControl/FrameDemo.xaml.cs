/*
 * Frame - 框架控件，用于导航内容（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 *     BackStackDepth - 返回 stack 中的条目数
 *     BackStack - 返回向后导航历史记录的 PageStackEntry 实例的集合（可以根据需求对集合增删改查）
 *     ForwardStack - 返回向前导航历史记录的 PageStackEntry 实例的集合（可以根据需求对集合增删改查）
 *     CanGoBack - 可否可以向后导航
 *     CanGoForward - 可否可以向前导航
 *     GoBack() - 向后导航，可以指定过渡效果
 *     GoForward() - 向前导航
 *     Navigate() - 导航到指定的 Type，可以传递一个 object 类型的参数，可以指定过渡效果
 *     CurrentSourcePageType - 获取 Frame 当前内容的 Type
 *     SourcePageType - 获取或设置 Frame 当前内容的 Type
 * 
 *     CacheSize - 所支持的最大缓存页数，默认值 10
 *         CacheSize 与被导航的页的 Page.NavigationCacheMode 属性相关（详见 Frame1.xaml.cs 和 Frame2.xaml.cs 的示例代码）
 *             NavigationCacheMode.Disabled - 每次导航到页时，都重新实例化此页，默认值（CacheSize 无效）
 *             NavigationCacheMode.Enabled - 每次导航到页时，首先缓存此页，此时如果已缓存的页数大于 CacheSize，则按先进先出的原则丢弃最早的缓存页（CacheSize 有效）
 *             NavigationCacheMode.Required - 每次导航到页时，都缓存此页（CacheSize 无效）
 * 
 *     Navigating - 导航开始时触发的事件
 *     Navigated - 导航完成后触发的事件
 *     NavigationFailed - 导航失败时触发的事件
 *     NavigationStopped - 导航过程中，又请求了一个新的导航时触发的事件
 *     
 *     GetNavigationState() - 获取 Frame 当前的导航状态，返回字符串类型的数据，仅当导航无参数传递或只传递简单参数（int, char, string, guid, bool 等）时有效
 *     SetNavigationState(string navigationState) - 将 Frame 还原到指定的导航状态
 *     
 *     
 * PageStackEntry - 保存在 stack 中的页对象
 *      SourcePageType - 获取此页的类型
 *      Parameter - 获取之前导航至此页时的参数
 *
 * 
 * NavigationEventArgs - 导航的事件参数
 *     NavigationMode - 导航方式，只读（Windows.UI.Xaml.Navigation.NavigationMode 枚举）
 *         New, Back, Forward, Refresh
 *     Parameter - 传递给导航目标页的参数，只读
 *     SourcePageType - 导航的目标页的类型，只读
 *     
 *     
 * 注：
 * 1、关于导航过程中的过渡效果请参见 /Animation/ThemeTransition/NavigationTransitionInfo/ 中的内容
 * 2、Frame 中与过渡效果有关的是 GoBack() 和 Navigate() 中的 NavigationTransitionInfo 类型的参数
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Controls.NavigationControl
{
    public sealed partial class FrameDemo : Page
    {
        public FrameDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            frame.Navigated += frame_Navigated;
        }

        void frame_Navigated(object sender, NavigationEventArgs e)
        {
            lblMsg.Text = "CacheSize: " + frame.CacheSize;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "BackStackDepth: " + frame.BackStackDepth;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "CanGoBack: " + frame.CanGoBack;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "CanGoForward: " + frame.CanGoForward;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "CurrentSourcePageType: " + frame.CurrentSourcePageType;
            lblMsg.Text += Environment.NewLine;

            // 显示 frame 的当前的导航状态，记录此值后，可以在需要的时候通过 SetNavigationState() 将 frame 还原到指定的导航状态
            lblMsg.Text += "NavigationState: " + frame.GetNavigationState();
        }

        private void btnGotoFrame1_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Frame1), "param1");
        }

        private void btnGotoFrame2_Click(object sender, RoutedEventArgs e)
        {
            frame.SourcePageType = typeof(Frame2);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoBack)
                frame.GoBack();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (frame.CanGoForward)
                frame.GoForward();
        }
    }
}
