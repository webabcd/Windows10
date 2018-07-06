/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     ManipulationModes - 需要监测的手势（Windows.UI.Xaml.Input.ManipulationModes 枚举）
 *         None - 禁用手势监测
 *         TranslateX, TranslateY - 位移手势
 *         TranslateRailsX, TranslateRailsY - 带有轨道的位移手势
 *         Rotate - 旋转手势
 *         Scale - 缩放手势
 *         TranslateInertia - 带有惯性的位移手势
 *         RotateInertia - 带有惯性的旋转手势
 *         ScaleInertia - 带有惯性的缩放手势
 *         All - 监测全部手势
 *     ManipulationStarting - 触控操作开始时触发的事件
 *     ManipulationStarted - 触控操作开始后触发的事件
 *     ManipulationInertiaStarting - 触控操作的惯性开始时触发的事件
 *     ManipulationCompleted - 触控操作结束后触发的事件
 *     ManipulationDelta - 触控值发生变化时触发的事件
 * 
 * ManipulationStartingRoutedEventArgs
 *     Container - 此 Manipulation 的容器
 *     Mode - 获取或设置 ManipulationModes
 *     Pivot - 获取或设置轴对象，ManipulationPivot 类型的数据
 *         Center - 旋转中心点
 *         Radius - 有效的旋转半径
 * 
 * ManipulationStartedRoutedEventArgs
 *     Container - 此 Manipulation 的容器
 *     Cumulative - 自操作开始后的累计变化量，返回 ManipulationDelta 类型的对象
 *     Position - 触摸点相对于 UIElement 的位置
 *     Complete() - 马上完成 Manipulation 而不发生惯性
 * 
 * ManipulationDeltaRoutedEventArgs
 *     Container - 此 Manipulation 的容器
 *     Cumulative - 自操作开始后的累计变化量，返回 ManipulationDelta 类型的对象
 *     Delta - 当前变化量，返回 ManipulationDelta 类型的对象
 *     Velocities - 当前变化的速率，返回 ManipulationVelocities 类型的对象
 *     IsInertial - 是否在惯性运动之中
 *     Position - 触摸点相对于 UIElement 的位置
 *     Complete() - 马上完成 Manipulation 而不发生惯性
 *     
 * ManipulationInertiaStartingRoutedEventArgs
 *     Container - 此 Manipulation 的容器
 *     Cumulative - 自操作开始后的累计变化量，返回 ManipulationDelta 类型的对象
 *     Delta - 当前变化量，返回 ManipulationDelta 类型的对象
 *     Velocities - 当前变化的速率，返回 ManipulationVelocities 类型的对象
 *     ExpansionBehavior - 惯性的缩放行为，获取或设置 InertiaExpansionBehavior 类型的对象
 *         DesiredDeceleration - 惯性运动时，缩放的减慢速率
 *         DesiredExpansion - 惯性结束后，缩放的值
 *     RotationBehavior - 惯性的旋转行为，获取或设置 InertiaRotationBehavior 类型的对象
 *         DesiredDeceleration - 惯性运动时，旋转的减慢速率
 *         DesiredRotation - 惯性结束后，旋转的度数
 *     TranslationBehavior - 惯性的位移行为，获取或设置 InertiaTranslationBehavior 类型的对象
 *         DesiredDeceleration - 惯性运动时，直线位移的减慢速率
 *         DesiredDisplacement - 惯性结束后，直线位移的的值
 *         
 * ManipulationCompletedRoutedEventArgs
 *     Container - 此 Manipulation 的容器
 *     Cumulative - 自操作开始后的累计变化量，返回 ManipulationDelta 类型的对象
 *     Velocities - 当前变化的速率，返回 ManipulationVelocities 类型的对象
 *     IsInertial - 结束前是否发生了惯性运动
 *     Position - 触摸点相对于 UIElement 的位置
 * ManipulationDelta - 变化量
 *     Expansion - 触摸点间距离的变化，单位 dip
 *     Scale - 触摸点间距离的变化，以一个百分比表示
 *     Rotation - 旋转角度的变化，以角度为单位
 *     Translation - 位移的变化，Point 类型的对象
 * ManipulationVelocities - 变化速率
 *     Angular - 旋转速度，单位：度/毫秒
 *     Expansion - 缩放速度，单位：dip/毫秒
 *     Linear - 直线位移速度，单位：Point/毫秒
 *     
 * 
 * 什么是 dip: device independent pixels（设备独立像素），不管屏大小和分辨率，把屏幕分成 480 * 320 个点，其中每一点代表 1 dip
 * Manipulate 是 UIElement 级别的手势操作；GestureRecognizer 是 app 级别的手势识别
 * 
 * 
 * 本例用于演示 UIElement 的 Manipulate 的应用（位移手势，缩放手势，旋转手势）
 * 
 * 
 * 注：关于 Manipulate Pointer Tap 的区别如下
 * 1、Manipulate 是最底层，Pointer 在中间，Tap 是最高层，所以会先走 Manipulate 事件，再走 Pointer 事件，最后走 Tap 事件
 * 2、如果高层的事件被触发，最相对于它的底层的事件也会被触发，反之则不一定
 * 3、使用原则是能在高层处理的事件尽量在高层处理（开发会简单些）
 */

using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class ManipulateDemo : Page
    {
        private TransformGroup _transformGroup;
        private CompositeTransform _compositeTransform;
        private MatrixTransform _previousTransform;

        public ManipulateDemo()
        {
            this.InitializeComponent();

            // 监测全部手势
            rectangle.ManipulationMode = ManipulationModes.All;
            // 仅监测旋转手势和缩放手势
            // rectangle.ManipulationMode = ManipulationModes.Rotate | ManipulationModes.Scale;

            _transformGroup = new TransformGroup();
            _compositeTransform = new CompositeTransform();
            _previousTransform = new MatrixTransform() { Matrix = Matrix.Identity };

            _transformGroup.Children.Add(_previousTransform);
            _transformGroup.Children.Add(_compositeTransform);

            rectangle.RenderTransform = _transformGroup;

            rectangle.ManipulationStarting += rectangle_ManipulationStarting;
            rectangle.ManipulationStarted += rectangle_ManipulationStarted;
            rectangle.ManipulationInertiaStarting += rectangle_ManipulationInertiaStarting;
            rectangle.ManipulationCompleted += rectangle_ManipulationCompleted;
            rectangle.ManipulationDelta += rectangle_ManipulationDelta;
        }

        void rectangle_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            _previousTransform.Matrix = _transformGroup.Value;

            // 获取操作点相对于此 GeneralTransform 的位置
            Point center = _previousTransform.TransformPoint(new Point(e.Position.X, e.Position.Y));
            _compositeTransform.CenterX = center.X;
            _compositeTransform.CenterY = center.Y;

            _compositeTransform.Rotation = e.Delta.Rotation;
            _compositeTransform.ScaleX = e.Delta.Scale;
            _compositeTransform.ScaleY = e.Delta.Scale;
            _compositeTransform.TranslateX = e.Delta.Translation.X;
            _compositeTransform.TranslateY = e.Delta.Translation.Y;
        }

        void rectangle_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            lblMsg.Text += "ManipulationStarting";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            lblMsg.Text += "ManipulationStarted";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            lblMsg.Text += "ManipulationInertiaStarting";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            lblMsg.Text += "ManipulationCompleted";
            lblMsg.Text += Environment.NewLine;
        }
    }
}