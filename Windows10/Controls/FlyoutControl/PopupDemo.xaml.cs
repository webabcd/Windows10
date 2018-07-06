/*
 * Popup - 弹出框控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     IsOpen - 弹出框是否是打开的状态（如果要设置此属性，需要在控件加载之后）
 *     Opened - 弹出框打开后触发的事件
 *     Closed - 弹出框关闭后触发的事件
 */

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class PopupDemo : Page
    {
        // 仿 toast 的 Popup
        private Popup _popupToast = new Popup();

        public PopupDemo()
        {
            this.InitializeComponent();

            popup.Opened += delegate { lblMsg.Text = "popup.Opened"; };
            popup.Closed += delegate { lblMsg.Text = "popup.Closed"; };
        }

        private void btnOpenPopup_Click(object sender, RoutedEventArgs e)
        {
            if (!popup.IsOpen)
                popup.IsOpen = true;
        }

        private void btnClosePopup_Click(object sender, RoutedEventArgs e)
        {
            if (popup.IsOpen)
                popup.IsOpen = false;
        }

        private void btnOpenPopupToast_Click(object sender, RoutedEventArgs e)
        {
            if (!_popupToast.IsOpen)
            {
                // 设置 Popup 中的内容
                Border border = new Border();
                border.BorderBrush = new SolidColorBrush(Colors.Red);
                border.BorderThickness = new Thickness(1);
                border.Background = new SolidColorBrush(Colors.Blue);
                border.Width = 600;
                border.Height = 100;
                border.Child = new TextBlock() { Text = "我是 Popup", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };

                // 设置 Popup 的相关属性
                _popupToast.IsLightDismissEnabled = true;
                _popupToast.Child = border;
                _popupToast.VerticalOffset = 100d; // 设置 Popup 的显示位置（Popup 的默认显示位置在窗口 0,0 点）
                _popupToast.ChildTransitions = new TransitionCollection() { new PaneThemeTransition() { Edge = EdgeTransitionLocation.Left } };

                _popupToast.IsOpen = true;
            }
        }
    }
}
