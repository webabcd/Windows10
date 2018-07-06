/*
 * ScrollContentPresenter - ScrollViewer 的内容呈现器，其用来呈现 ScrollViewer 的 Content（继承自 ContentPresenter, 请参见 /Controls/BaseControl/ContentControlDemo/ContentPresenterDemo.xaml）
 *     类似的有 ContentPresenter, ItemsPresenter 等
 */

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows10.Common;

namespace Windows10.Controls.ScrollViewerDemo
{
    public sealed partial class ScrollContentPresenterDemo : Page
    {
        public ScrollContentPresenterDemo()
        {
            this.InitializeComponent();

            this.Loaded += ScrollContentPresenterDemo_Loaded;
        }

        private void ScrollContentPresenterDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 找到 ScrollViewer 内的名为 ScrollContentPresenter 的 ScrollContentPresenter 控件
            var scrollContentPresenter = Helper.GetVisualChild<ScrollContentPresenter>(scrollViewer, "ScrollContentPresenter");

            scrollContentPresenter.BorderBrush = new SolidColorBrush(Colors.Red);
            scrollContentPresenter.BorderThickness = new Thickness(4);
        }
    }
}
