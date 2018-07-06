using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ScrollViewerDemo
{
    public sealed partial class Zoom : Page
    {
        public Zoom()
        {
            this.InitializeComponent();

            this.Loaded += Zoom_Loaded;
        }

        private void Zoom_Loaded(object sender, RoutedEventArgs e)
        {
            /*
             * ZoomSnapPoints - “放大/缩小”的对齐点的集合，默认是空的
             * 
             * ZoomSnapPointsType - “放大/缩小”的对齐行为
             *     SnapPointsType.None - 不需要对齐，默认值
             *     SnapPointsType.Optional - 看情况，如果离某个对齐点很近则对齐
             *     SnapPointsType.Mandatory - 强制对齐，必须对齐到某一个对齐点
             *     SnapPointsType.OptionalSingle - 同 Optional，但不能跳过对齐点
             *     SnapPointsType.MandatorySingle - 同 Mandatory，但不能跳过对齐点
             *     
             * IsZoomChainingEnabled - 是否启用 Zoom 的 Chaining
             * IsZoomInertiaEnabled - 是否启用 Zoom 的 Inertia
             * ZoomFactor - 获取当前的 Zoom 的倍数
             * 
             * ZoomToFactor() - Zoom 到指定的倍数
             */

            scrollViewer.ZoomSnapPointsType = SnapPointsType.OptionalSingle;

            scrollViewer.ZoomSnapPoints.Add(0.5f);
            scrollViewer.ZoomSnapPoints.Add(0.8f);
            scrollViewer.ZoomSnapPoints.Add(1.0f);
            scrollViewer.ZoomSnapPoints.Add(1.5f);
            scrollViewer.ZoomSnapPoints.Add(2.0f);
        }

        private void bntZoom_Click(object sender, RoutedEventArgs e)
        {
            scrollViewer.ChangeView(null, null, 0.5f);
        }
    }
}
