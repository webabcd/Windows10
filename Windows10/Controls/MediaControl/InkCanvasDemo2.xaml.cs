/*
 * InkCanvas - 涂鸦板控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     InkPresenter - 获取 InkPresenter 对象
 *        
 * InkPresenter - 涂鸦板
 *     StrokeContainer - 返回 InkStrokeContainer 类型的对象
 *     InputProcessingConfiguration.RightDragAction - 涂鸦的输入按键
 *         AllowProcessing - 所有按键均可用于涂鸦，比如鼠标的左键和右键均可用于涂鸦。默认值
 *         LeaveUnprocessed - 特殊按键不可用于涂鸦，比如鼠标的右键不可用于涂鸦
 *     UnprocessedInput - 用于监听指针的一些事件
 *         PointerEntered, PointerExited, PointerHovered, PointerLost, PointerMoved, PointerPressed, PointerReleased - 顾名思义的一些事件
 *         注：当 InputProcessingConfiguration.RightDragAction 为 AllowProcessing 时，则只会触发 PointerEntered, PointerExited, PointerHovered 事件
 *     SetPredefinedConfiguration(InkPresenterPredefinedConfiguration value) - 设置涂鸦行为
 *         SimpleSinglePointer - 单接触点涂鸦
 *         SimpleMultiplePointer - 多接触点涂鸦，需要先调用 ActivateCustomDrying()
 *     StrokesCollected - 当一个或多个 InkStroke 被画进来时触发的事件
 *     StrokesErased - 当一个或多个 InkStroke 被擦除时触发的事件
 *     StrokeInput - 用于监听一些涂鸦事件
 *         StrokeStarted, StrokeEnded, StrokeContinued, StrokeCanceled - 顾名思义的一些事件
 *     
 * InkStrokeContainer - 用于管理涂鸦
 *     GetStrokes() - 获取当前涂鸦板的所有 InkStroke 对象集合
 *     BoundingRect - 获取当前涂鸦板的所有 InkStroke 对象的 Rect 区域
 *     Clear() - 清除所有涂鸦
 *     SelectWithLine(Point from, Point to) - 返回指定对角线内的涂鸦所在的 Rect 区域
 *     SelectWithPolyLine(IEnumerable<Point> polyline) - 返回指定 Polyline 内的涂鸦所在的 Rect 区域
 *     CopySelectedToClipboard() - 复制选中的涂鸦到剪切板
 *     CanPasteFromClipboard() - 剪切板中是否有涂鸦数据
 *     PasteFromClipboard(Point position) - 从剪切板粘贴涂鸦到指定的位置，并返回粘贴涂鸦的 Rect 区域
 *     MoveSelected(Point translation) - 指定偏移量，并移动选中的涂鸦，返回的是移动后的涂鸦的 Rect 区域
 *     DeleteSelected() - 删除选中的涂鸦，并返回删除涂鸦的 Rect 区域
 *     AddStroke(InkStroke stroke) - 在涂鸦板上绘制指定的 InkStroke 对象
 *     AddStrokes(IEnumerable<InkStroke> strokes) - 在涂鸦板上绘制指定的 InkStroke 对象集合
 *     
 * InkStroke - 涂鸦对象（这是一次的涂鸦对象，即鼠标按下后移动然后再抬起后所绘制出的涂鸦）
 *     BoundingRect - 获取当前 InkStroke 的 Rect 区域
 *     DrawingAttributes - 涂鸦笔尖属性（参见 InkCanvasDemo1.xaml.cs）
 *     PointTransform - 用于转换 InkStroke 的 Matrix3x2 仿射转换矩阵（Matrix3x2 提供了一些简便的方法：CreateRotation, CreateScale, CreateSkew, CreateTranslation 等）
 *     Selected - 是否被选中
 *     Clone() - 克隆一份，返回克隆后的 InkStroke 对象
 *     GetInkPoints(), GetRenderingSegments() - 用于获取组成 InkStroke 的一堆贝塞尔曲线的数据
 *     
 * 
 * 注：在 InkCanvas 有湿（wet）和干（dry）的感念
 * 所谓的湿就是指鼠标按下后移动，在鼠标抬起来之前绘制出的涂鸦
 * 所谓的干就是指鼠标抬起后，真正显示在 InkCanvas 上的涂鸦（鼠标抬起后可以将 wet 做一些自定义处理，然后再 dry 到 InkCanvas 上）
 * 如何在 wet 和 dry 之间做自定义处理呢？需要通过 InkPresenter 的 ActivateCustomDrying() 方法获取到 InkSynchronizer 对象来实现，具体的示例在之后介绍 Win2D 的时候再写
 */

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class InkCanvasDemo2 : Page
    {
        // 右键轨迹（用于选择涂鸦）
        private Polyline _polyline;
        // 被选中的涂鸦所在的 Rect 区域
        private Rect _rect;

        public InkCanvasDemo2()
        {
            this.InitializeComponent();

            inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen | CoreInputDeviceTypes.Touch;

            InkDrawingAttributes drawingAttributes = inkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.IgnorePressure = true;
            drawingAttributes.Color = Colors.Red;
            drawingAttributes.Size = new Size(4, 4);
            inkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);

            // 此设置用于禁用鼠标右键的涂鸦功能，从而在后面实现鼠标右键的选择涂鸦的功能
            inkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            inkCanvas.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;

            inkCanvas.InkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
            inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }
       
        // 右键按下
        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            // 右键轨迹
            _polyline = new Polyline()
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
            };
            _polyline.Points.Add(args.CurrentPoint.RawPosition);

            // 绘制右键轨迹
            selectionCanvas.Children.Add(_polyline);
        }

        // 右键按下后在 InkCanvas 中移动
        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            // 更新右键轨迹
            _polyline.Points.Add(args.CurrentPoint.RawPosition);
        }

        // 右键抬起
        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {
            // 更新右键轨迹
            _polyline.Points.Add(args.CurrentPoint.RawPosition);

            // 获取右键圈起来的区域内的涂鸦所在的 Rect 区域
            _rect = inkCanvas.InkPresenter.StrokeContainer.SelectWithPolyLine(_polyline.Points);

            DrawBoundingRect();
        }

        // 绘制 _rect 区域（即被选中的涂鸦所在的 Rect 区域）
        private void DrawBoundingRect()
        {
            selectionCanvas.Children.Clear();

            if ((_rect.Width == 0) || (_rect.Height == 0) || _rect.IsEmpty)
            {
                return;
            }

            var rectangle = new Rectangle()
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 5, 2 },
                Width = _rect.Width,
                Height = _rect.Height
            };

            Canvas.SetLeft(rectangle, _rect.X);
            Canvas.SetTop(rectangle, _rect.Y);

            selectionCanvas.Children.Add(rectangle);
        }


        // 开始此次涂鸦后
        private void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            ClearSelection();
        }

        // 擦除某些涂鸦后
        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            ClearSelection();
        }
        
        // 清除涂鸦的选中状态，以及选中框
        private void ClearSelection()
        {
            IReadOnlyList<InkStroke> strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (var stroke in strokes)
            {
                stroke.Selected = false;
            }
            ClearDrawnBoundingRect();
        }

        // 清除涂鸦的选中框
        private void ClearDrawnBoundingRect()
        {
            if (selectionCanvas.Children.Any())
            {
                selectionCanvas.Children.Clear();
                _rect = Rect.Empty;
            }
        }


        // 清除全部涂鸦
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ClearDrawnBoundingRect();

            inkCanvas.InkPresenter.StrokeContainer.Clear();
        }

        // 剪切选中涂鸦到剪切板
        private void cut_Click(object sender, RoutedEventArgs e)
        {
            ClearDrawnBoundingRect();

            inkCanvas.InkPresenter.StrokeContainer.CopySelectedToClipboard();
            Rect rect = inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
        }

        // 复制选中涂鸦到剪切板
        private void copy_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.InkPresenter.StrokeContainer.CopySelectedToClipboard();
        }

        // 从剪切板粘贴涂鸦
        private void paste_Click(object sender, RoutedEventArgs e)
        {
            if (inkCanvas.InkPresenter.StrokeContainer.CanPasteFromClipboard())
            {
                Rect rect = inkCanvas.InkPresenter.StrokeContainer.PasteFromClipboard(new Point(0, 0));
            }
        }

        // 移动选中涂鸦
        private void move_Click(object sender, RoutedEventArgs e)
        {
            Rect rect = inkCanvas.InkPresenter.StrokeContainer.MoveSelected(new Point(10, 10));

            _rect = rect;
            DrawBoundingRect();
        }

        // 克隆全部涂鸦
        private void cloneAll_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            IReadOnlyList<InkStroke> oldStrokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            List<InkStroke> newStrokes = new List<InkStroke>();
            foreach (InkStroke oldStroke in oldStrokes)
            {
                InkStroke newStroke = oldStroke.Clone();
                newStroke.Selected = true;
                newStrokes.Add(newStroke);
            }
            inkCanvas.InkPresenter.StrokeContainer.AddStrokes(newStrokes);
            inkCanvas.InkPresenter.StrokeContainer.MoveSelected(new Point(random.Next(5, 30), random.Next(5, 30)));

            ClearSelection();
        }

        // 选中全部涂鸦
        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            Rect rect = inkCanvas.InkPresenter.StrokeContainer.BoundingRect;
            _rect = rect;

            IReadOnlyList<InkStroke> strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (var stroke in strokes)
            {
                stroke.Selected = true;
            }

            DrawBoundingRect();
        }

        // 改变全部涂鸦的颜色
        private void changeColor_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            Color color = Color.FromArgb(255, (byte)random.Next(0, 256), (byte)random.Next(0, 256), (byte)random.Next(0, 256));

            IReadOnlyList<InkStroke> strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (var stroke in strokes)
            {
                InkDrawingAttributes drawingAttributes = stroke.DrawingAttributes;
                drawingAttributes.Color = color;
                stroke.DrawingAttributes = drawingAttributes;
            }
        }
    }
}
