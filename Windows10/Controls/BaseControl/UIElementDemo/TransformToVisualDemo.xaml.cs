/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     TransformToVisual(UIElement visual) - 返回相对于指定 UIElement 原点（左上角顶点）的 GeneralTransform 类型的对象，传 null 值则为相对于 app 原点（左上角顶点）
 *     
 *     
 * GeneralTransform
 *     Point TransformPoint(Point point) - 获取相对于指定位置的位置
 *     
 *     
 * 本例用于演示如何获取 UIElement 的位置
 */

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class TransformToVisualDemo : Page
    {
        public TransformToVisualDemo()
        {
            this.InitializeComponent();

            this.Loaded += TransformToVisualDemo_Loaded;
        }

        void TransformToVisualDemo_Loaded(object sender, RoutedEventArgs e)
        {
            Demo1();
            Demo2();
        }

        // 演示如何获取 UIElement 相对于 app 原点（左上角顶点）的位置
        private void Demo1()
        {
            GeneralTransform generalTransform = rectangle1.TransformToVisual(null); // 获取 rectangle1 相对于 app 原点（左上角顶点）的 GeneralTransform
            Point point = generalTransform.TransformPoint(new Point(0, 0)); // rectangle1 的原点（左上角顶点）相对于屏幕 0,0 点的位置

            lblMsg.Text += "红色矩形的原点（左上角顶点）相对于屏幕的原点（左上角顶点）的位置：" + point.ToString();
            lblMsg.Text += Environment.NewLine;

        }

        // 演示如何获取 UIElement 相对于另一个 UIElement 原点（左上角顶点）的位置
        private void Demo2()
        {
            GeneralTransform generalTransform = rectangle1.TransformToVisual(rectangle2); // 获取 rectangle1 相对于 rectangle2 原点（左上角顶点）的 GeneralTransform
            Point point = generalTransform.TransformPoint(new Point(0, 0)); // rectangle1 的原点（左上角顶点）相对于 rectangle2 的原点（左上角顶点）的位置

            lblMsg.Text += "红色矩形的原点（左上角顶点）相对于绿色矩形的原点（左上角顶点）的位置：" + point.ToString();
        }
    }
}