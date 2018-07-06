/*
 * 本例用于演示如何通过 Pointer 相关事件的处理，来实现一个简单的涂鸦板
 */

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class InkSimple : Page
    {
        // 用于保存触摸点（PointerId - Point）
        private Dictionary<uint, Point?> _dicPoint;

        public InkSimple()
        {
            this.InitializeComponent();

            canvas.PointerPressed += canvas_PointerPressed;
            canvas.PointerMoved += canvas_PointerMoved;
            canvas.PointerReleased += canvas_PointerReleased;
            canvas.PointerExited += canvas_PointerExited;

            _dicPoint = new Dictionary<uint, Point?>();
        }

        void canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 指针按下后，保存此触摸点
            PointerPoint pointerPoint = e.GetCurrentPoint(canvas);
            _dicPoint[pointerPoint.PointerId] = pointerPoint.Position;
        }

        void canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(canvas);

            if (_dicPoint.ContainsKey(pointerPoint.PointerId) && _dicPoint[pointerPoint.PointerId].HasValue)
            {
                Point currentPoint = pointerPoint.Position;
                Point previousPoint = _dicPoint[pointerPoint.PointerId].Value;

                // 如果指针移动过程中，两个点间的距离超过 4 则在两点间绘制一条直线，以完成涂鸦
                if (ComputeDistance(currentPoint, previousPoint) > 4)
                {
                    Line line = new Line()
                    {
                        X1 = previousPoint.X,
                        Y1 = previousPoint.Y,
                        X2 = currentPoint.X,
                        Y2 = currentPoint.Y,
                        StrokeThickness = 5,
                        Stroke = new SolidColorBrush(Colors.Orange),
                        StrokeEndLineCap = PenLineCap.Round
                    };

                    _dicPoint[pointerPoint.PointerId] = currentPoint;
                    canvas.Children.Add(line);
                }
            }
        }

        void canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            // 指针释放后，从字典中删除此 PointerId 的数据
            PointerPoint pointerPoint = e.GetCurrentPoint(canvas);
            if (_dicPoint.ContainsKey(pointerPoint.PointerId))
                _dicPoint.Remove(pointerPoint.PointerId);
        }

        void canvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // 指针离开相当于指针释放
            canvas_PointerReleased(sender, e);
        }

        // 清除涂鸦
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _dicPoint.Clear();
        }

        // 计算两个点（Point）之间的距离
        private double ComputeDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}
