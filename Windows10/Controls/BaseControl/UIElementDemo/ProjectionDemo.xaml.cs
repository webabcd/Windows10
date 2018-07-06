/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     Projection - 投影（模拟 3D 效果）
 *         PlaneProjection - 将对象投影到平面（通过 x,y,z 方向的旋转和位移控制投影）
 *         Matrix3DProjection - 将对象投影到平面（通过 Matrix3D 矩阵控制投影）
 *     
 *     
 * 本例用于演示 UIElement 的投影（模拟 3D 效果）的应用
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class ProjectionDemo : Page
    {
        public ProjectionDemo()
        {
            this.InitializeComponent();
        }
    }
}
