/*
 * ScrollViewer - 滚动视图控件（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 *     ComputedHorizontalScrollBarVisibility - 当前水平滚动条的可见性（Visible, Collapsed）
 *     ComputedVerticalScrollBarVisibility - 当前垂直滚动条的可见性（Visible, Collapsed）
 *     ExtentWidth - ScrollViewer 内的内容的宽
 *     ExtentHeight - ScrollViewer 内的内容的高
 *     ViewportWidth - 可视区的宽
 *     ViewportHeight - 可视区的高
 *     HorizontalOffset - 滚动内容的水平方向的偏移量
 *     VerticalOffset - 滚动内容的垂直方向的偏移量
 *     ScrollableWidth - 水平滚动区域的大小（即 HorizontalOffset 的最大值，也就是 ExtentWidth - ViewportWidth）
 *     ScrollableHeight - 垂直滚动区域的大小（即 VerticalOffset 的最大值，也就是 ExtentHeight - ViewportHeight）
 *     
 *     bool ChangeView(double? horizontalOffset, double? verticalOffset, float? zoomFactor, bool disableAnimation) - 改变内容的显示
 *         用于取代如下这些已经弃用的方法 ScrollToHorizontalOffset(double offset), ScrollToVerticalOffset(double offset), ZoomToFactor(float factor)
 *         
 *     另外还有一堆对应的附加属性和静态方法，内嵌 ScrollViewer 的控件一般均支持，不再详述。简单示例可参见：/Controls/CollectionControl/ListViewBaseDemo/ListViewBaseDemo1.xaml
 * 
 * 
 * 本例用于演示 ScrollViewer 的基本用法
 */

using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ScrollViewerDemo
{
    public sealed partial class ScrollViewerDemo : Page
    {
        public ScrollViewerDemo()
        {
            this.InitializeComponent();
        }

        private void scrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            Debug.WriteLine("scrollViewer_ViewChanging");
        }

        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            Debug.WriteLine("scrollViewer_ViewChanged");

            lblMsg.Text = "ComputedHorizontalScrollBarVisibility: " + scrollViewer.ComputedHorizontalScrollBarVisibility;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ComputedVerticalScrollBarVisibility: " + scrollViewer.ComputedVerticalScrollBarVisibility;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ExtentWidth: " + scrollViewer.ExtentWidth;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ExtentHeight: " + scrollViewer.ExtentHeight;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ViewportWidth: " + scrollViewer.ViewportWidth;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ViewportHeight: " + scrollViewer.ViewportHeight;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "HorizontalOffset: " + scrollViewer.HorizontalOffset;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "VerticalOffset: " + scrollViewer.VerticalOffset;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ScrollableWidth: " + scrollViewer.ScrollableWidth;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ScrollableHeight: " + scrollViewer.ScrollableHeight;
            lblMsg.Text += Environment.NewLine;

            // 在操作中返回 true, 操作结束返回 false
            lblMsg.Text += "ScrollViewerViewChangedEventArgs.IsIntermediate: " + e.IsIntermediate;
        }

        private void scrollViewer_DirectManipulationCompleted(object sender, object e)
        {
            Debug.WriteLine("scrollViewer_DirectManipulationCompleted");
        }

        private void scrollViewer_DirectManipulationStarted(object sender, object e)
        {
            Debug.WriteLine("scrollViewer_DirectManipulationStarted");
        }

        private void btnChangeView_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(scrollViewer.ScrollableWidth / 2, scrollViewer.ScrollableHeight / 2, null, false);
        }
    }
}
