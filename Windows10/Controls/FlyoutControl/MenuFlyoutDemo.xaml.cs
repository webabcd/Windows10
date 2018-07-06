/*
 * MenuFlyout - 菜单弹出框控件（继承自 FlyoutBase, 请参见 /Controls/FlyoutControl/FlyoutBaseDemo.xaml）
 *     ShowAt(UIElement targetElement, Point point) - 在指定的 UIElement 的指定位置（point 是相对于 targetElement 的位置）显示 MenuFlyout 控件
 */

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class MenuFlyoutDemo : Page
    {
        public MenuFlyoutDemo()
        {
            this.InitializeComponent();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "MenuFlyoutItem 被 click 了";
        }

        private void menuFlyoutItem1_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "menuFlyoutItem1 被 click 了";
        }

        private void toggleMenuFlyoutItem1_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleMenuFlyoutItem1 被 click 了, IsChecked:{toggleMenuFlyoutItem1.IsChecked}";
        }

        private void toggleMenuFlyoutItem2_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleMenuFlyoutItem2 被 click 了, IsChecked:{toggleMenuFlyoutItem2.IsChecked}";
        }


        private void textBlock1_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MenuFlyout menuFlyout = this.Resources["MyMenuFlyout"] as MenuFlyout;

            // 对于基类的 ShowAt(FrameworkElement targetElement) 方法当然是支持的
            // menuFlyout.ShowAt(textBlock1);

            // 在 MenuFlyout 中重载了 ShowAt() 方法，即 ShowAt(UIElement targetElement, Point point)
            // 其中 point 代表 MenuFlyout 相对于 targetElement 左上角的显示位置（此时 MenuFlyout 的 Placement 参数就无效了）
            menuFlyout.ShowAt(textBlock1, new Point(10, 10));
        }
    }
}
