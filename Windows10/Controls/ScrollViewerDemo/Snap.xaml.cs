/*
 * Snap: 对齐，在触摸模式下，如果 ScrollViewer 有多个元素，在滚动结束后会定位到其中某一个具体的元素，这就叫对齐
 * 
 * HorizontalSnapPointsType - 水平方向上的对齐行为，Windows.UI.Xaml.Controls.SnapPointsType枚举
 *     SnapPointsType.None - 不需要对齐，默认值
 *     SnapPointsType.Optional - 看情况，如果离某个元素很近则对齐此元素
 *     SnapPointsType.Mandatory - 强制对齐，必须对齐到某一元素
 *     SnapPointsType.OptionalSingle - 仅对 Zoom 对齐有用（参看 /Controls/ScrollViewerDemo/Zoom.xaml）
 *     SnapPointsType.MandatorySingle - 仅对 Zoom 对齐有用（参看 /Controls/ScrollViewerDemo/Zoom.xaml）
 * VerticalSnapPointsType - 垂直方向上的对齐行为
 * 
 * 
 * HorizontalSnapPointsAlignment - 水平方向上的对齐方式，Windows.UI.Xaml.Controls.Primitives.SnapPointsAlignment枚举
 *     SnapPointsAlignment.Near - 元素的左侧对齐 ScrollViewer 的左侧边界，默认值
 *     SnapPointsAlignment.Center - 元素的中心点对齐 ScrollViewer 的中心点
 *     SnapPointsAlignment.Far - 元素的右侧对齐 ScrollViewer 的右侧边界
 * VerticalSnapPointsAlignment - 垂直方向上的对齐方式
 * 
 * 
 * BringIntoViewOnFocusChange - ScrollViewer 内的某元素获得焦点后，是否需要定位到此元素，默认值为 true
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ScrollViewerDemo
{
    public sealed partial class Snap : Page
    {
        public Snap()
        {
            this.InitializeComponent();
            this.Loaded += Snap_Loaded;
        }

        void Snap_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            scrollViewer.HorizontalSnapPointsAlignment = Windows.UI.Xaml.Controls.Primitives.SnapPointsAlignment.Near;
            scrollViewer.BringIntoViewOnFocusChange = true;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (scrollViewer != null && comboBox != null)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        scrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
                        break;
                    case 1:
                        scrollViewer.HorizontalSnapPointsType = SnapPointsType.Optional;
                        break;
                    case 2:
                        scrollViewer.HorizontalSnapPointsType = SnapPointsType.Mandatory;
                        break;
                    default:
                        scrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
                        break;
                }
            }
        }

        private void btnScroll_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 当 BringIntoViewOnFocusChange 为 true 时，如果 txtMsg2 获得焦点，则 ScrollViewer 会自动滚动到 txtMsg2
            txtMsg2.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
    }
}
