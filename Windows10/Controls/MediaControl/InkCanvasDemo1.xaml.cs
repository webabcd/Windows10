/*
 * InkCanvas - 涂鸦板控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     InkPresenter - 获取 InkPresenter 对象
 *        
 * InkPresenter - 涂鸦板
 *     IsInputEnabled - 是否启用涂鸦板
 *     InputDeviceTypes - 输入设备的类型（None, Touch, Pen, Mouse）
 *     InputProcessingConfiguration.Mode - 输入模式（None, Inking, Erasing）
 *     CopyDefaultDrawingAttributes() - 获取 InkDrawingAttributes 对象
 *     UpdateDefaultDrawingAttributes(InkDrawingAttributes value) - 设置 InkDrawingAttributes 对象
 *     
 * InkDrawingAttributes - 涂鸦笔尖属性
 *     IgnorePressure - 是否忽略触摸压力
 *     Color - 笔尖的颜色
 *     Size - 笔尖的尺寸（宽和高）
 *     DrawAsHighlighter - 覆盖之前的涂鸦时（false - 直接覆盖；true - 高亮显示覆盖区域）
 *     FitToCurve - 涂鸦时（true - 使用贝塞尔曲线生成涂鸦；false - 使用直线生成涂鸦）
 *     PenTip - 笔尖的形状（Circle, Rectangle）
 *     PenTipTransform - 用于转换 PenTip 的 Matrix3x2 仿射转换矩阵（Matrix3x2 提供了一些简便的方法：CreateRotation, CreateScale, CreateSkew, CreateTranslation 等）。通过它可以自定义笔尖的形状
 */

using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class InkCanvasDemo1 : Page
    {
        private InkPresenter _inkPresenter;

        public InkCanvasDemo1()
        {
            this.InitializeComponent();

            _inkPresenter = inkCanvas.InkPresenter;
            _inkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            UpdateDefaultDrawingAttributes();
        }
        
        private void UpdateDefaultDrawingAttributes_Handler(object sender, RoutedEventArgs e)
        {
            UpdateDefaultDrawingAttributes();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            _inkPresenter.StrokeContainer.Clear();
        }

        private void UpdateDefaultDrawingAttributes()
        {
            if (_inkPresenter != null)
            {
                InkDrawingAttributes drawingAttributes = _inkPresenter.CopyDefaultDrawingAttributes();
                drawingAttributes.IgnorePressure = true;

                switch (drawingColor.SelectedValue.ToString())
                {
                    case "Red":
                        drawingAttributes.Color = Colors.Red;
                        break;
                    case "Green":
                        drawingAttributes.Color = Colors.Green;
                        break;
                    case "Blue":
                        drawingAttributes.Color = Colors.Blue;
                        break;
                }

                drawingAttributes.Size = new Size(drawingSize.Value, drawingSize.Value);
                drawingAttributes.DrawAsHighlighter = drawingDrawAsHighlighter.IsChecked.Value;
                drawingAttributes.FitToCurve = drawingFitToCurve.IsChecked.Value;
                drawingAttributes.PenTip = drawingPenTip.IsOn ? PenTipShape.Circle : PenTipShape.Rectangle;



                if (drawingPenTipTransform.IsChecked == true)
                    drawingAttributes.PenTipTransform = Matrix3x2.CreateSkew(4, 4);
                else
                    drawingAttributes.PenTipTransform = Matrix3x2.Identity;

                _inkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);



                if (chkErasing.IsChecked == true)
                    _inkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
                else
                    _inkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;



                _inkPresenter.IsInputEnabled = chkIsInputEnabled.IsChecked.Value;
            }
        }
    }
}
