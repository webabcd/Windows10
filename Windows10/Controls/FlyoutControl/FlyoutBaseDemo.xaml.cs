/*
 * FlyoutBase（基类） - 弹出框控件基类（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo.xaml）
 *     Hide() - 隐藏 Flyout
 *     ShowAt(FrameworkElement targetElement) - 在指定的 FrameworkElement 上显示指定的 Flyout
 *
 *     FlyoutBase.SetAttachedFlyout(FrameworkElement element, FlyoutBase value) - 在指定的 FrameworkElement 上绑定指定的 Flyout
 *     FlyoutBase.GetAttachedFlyout(FrameworkElement element) - 获取指定的 FrameworkElement 上绑定的 Flyout
 *     FlyoutBase.ShowAttachedFlyout(FrameworkElement flyoutOwner) - 在指定的 FrameworkElement 上显示其绑定的 Flyout
 *
 *
 * 注：点击 FlyoutBase 外部的话，FlyoutBase 会自动关闭
 * 
 * 另外，在 uwp 中不再支持 SettingsFlyout 了
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class FlyoutBaseDemo : Page
    {
        public FlyoutBaseDemo()
        {
            this.InitializeComponent();
        }


        private void flyout1_Opening(object sender, object e)
        {
            lblMsg1.Text = "Flyout 打开中";
        }
        private void flyout1_Opened(object sender, object e)
        {
            lblMsg1.Text = "Flyout 已打开";
        }
        private void flyout1_Closed(object sender, object e)
        {
            lblMsg1.Text = "Flyout 已关闭";
        }
        private void button1CloseFlyout_Click(object sender, RoutedEventArgs e)
        {
            flyout1.Hide();
        }


        private void textBlock1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(textBlock1);
        }


        private void textBlock2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(textBlock2);
            flyout.Placement = FlyoutPlacementMode.Right;

            FlyoutBase.ShowAttachedFlyout(textBlock2);
        }


        // 在 CodeBehind 中创建 FlyoutBase 控件后，将其绑定到指定的控件上，并显示
        private void textBlock3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock flyoutContent = new TextBlock();
            flyoutContent.Text = "我是 Flyout 中的内容";

            Flyout flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.Right;
            flyout.Content = flyoutContent;

            FlyoutBase.SetAttachedFlyout(textBlock3, flyout);
            FlyoutBase.ShowAttachedFlyout(textBlock3);
        }


        // 在 CodeBehind 中创建 FlyoutBase 控件后，在不设置绑定的情况下，使其显示在指定的控件上
        private void textBlock4_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock flyoutContent = new TextBlock();
            flyoutContent.Text = "我是 Flyout 中的内容";

            Flyout flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.Right;
            flyout.Content = flyoutContent;

            flyout.ShowAt(textBlock4);
        }
    }
}
