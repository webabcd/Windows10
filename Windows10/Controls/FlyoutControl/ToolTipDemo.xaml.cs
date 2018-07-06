/*
 * ToolTip - 提示框控件（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.FlyoutControl
{
    public sealed partial class ToolTipDemo : Page
    {
        public ToolTipDemo()
        {
            this.InitializeComponent();
        }

        private void toolTip_Opened(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "textBlock2 toolTip_Opened";
        }

        private void toolTip_Closed(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "textBlock2 toolTip_Closed";
        }
    }
}
