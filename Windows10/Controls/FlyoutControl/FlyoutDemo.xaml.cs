/*
 * Flyout - 弹出框控件（继承自 FlyoutBase, 请参见 /Controls/FlyoutControl/FlyoutBaseDemo.xaml）
 *     Content - 获取或设置 Flyout 的内容
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class FlyoutDemo : Page
    {
        public FlyoutDemo()
        {
            this.InitializeComponent();
        }

        private void textBlock1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock flyoutContent = new TextBlock();
            flyoutContent.Text = "我是 Flyout 中的内容";

            Flyout flyout = new Flyout();
            flyout.Placement = FlyoutPlacementMode.Right;
            flyout.Content = flyoutContent;

            flyout.ShowAt(textBlock1);
        }
    }
}
