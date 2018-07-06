/*
 * RepeatButton - 按住后会重复触发 Click 事件的按钮（继承自 ButtonBase, 请参见 /Controls/ButtonControl/ButtonBaseDemo.xaml）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ButtonControl
{
    public sealed partial class RepeatButtonDemo : Page
    {
        public RepeatButtonDemo()
        {
            this.InitializeComponent();
        }

        private void repeatButton_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "x";
        }
    }
}
