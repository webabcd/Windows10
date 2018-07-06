/*
 * RangeBase(基类) - 范围控件基类（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Windows10.Controls.ProgressControl
{
    public sealed partial class RangeBaseDemo : Page
    {
        public RangeBaseDemo()
        {
            this.InitializeComponent();
        }

        private void slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // RangeBaseValueChangedEventArgs
            //     OldValue - 范围控件的之前的值
            //     NewValue - 范围控件的当前的值
            lblMsg.Text = $"slider old value:{e.OldValue}, slider new value:{e.NewValue}";
        }
    }
}
