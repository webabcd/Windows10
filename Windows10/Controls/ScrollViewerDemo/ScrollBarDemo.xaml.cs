/*
 * ScrollBar - 滚动条控件（继承自 RangeBase, 请参见 /Controls/ProgressControl/RangeBaseDemo.xaml）
 * 
 * 本例通过访问 ScrollViewer 内的名为 VerticalScrollBar 的 ScrollBar 控件，来简要说明 ScrollBar 控件
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows10.Common;

namespace Windows10.Controls.ScrollViewerDemo
{
    public sealed partial class ScrollBarDemo : Page
    {
        public ScrollBarDemo()
        {
            this.InitializeComponent();

            this.Loaded += ScrollBarDemo_Loaded;
        }

        private void ScrollBarDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 找到 ScrollViewer 内的名为 VerticalScrollBar 的 ScrollBar 控件，即 ScrollViewer 内的垂直滚动条
            var scrollBar = Helper.GetVisualChild<ScrollBar>(scrollViewer, "VerticalScrollBar");

            // ValueChanged - 当滚动条的值发生改变是所触发的事件
            scrollBar.ValueChanged += scrollBar_ValueChanged;
        }

        void scrollBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            // 显示垂直滚动条的当前值
            lblMsg.Text = e.NewValue.ToString();
        }
    }
}
