/*
 * 演示缓动（easing）的应用
 * 
 * WinRT 支持 11 种经典的缓动：
 * BackEase, BounceEase, CircleEase, CubicEase, ElasticEase, ExponentialEase, PowerEase, QuadraticEase, QuarticEase, QuinticEase, SineEase
 * 
 * EasingMode 有 3 种：
 * EaseIn, EaseOut, EaseInOut
 */

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Animation
{
    public sealed partial class EasingAnimation : Page
    {
        public EasingAnimation()
        {
            this.InitializeComponent();

            this.Loaded += EasingAnimation_Loaded;
        }

        void EasingAnimation_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            cboEasingFunction.SelectedIndex = 0;
            cboEasingMode.SelectedIndex = 0;
        }

        private void cboEasingFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EasingChanged();
        }

        private void cboEasingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EasingChanged();
        }

        private void EasingChanged()
        {
            if (cboEasingFunction.SelectedIndex == -1 || cboEasingMode.SelectedIndex == -1)
                return;

            storyboard.Stop();

            EasingFunctionBase easingFunction = null;

            // 确定 Easing Function
            switch ((cboEasingFunction.SelectedItem as ComboBoxItem).Content.ToString())
            {
                case "BackEase":
                    // Amplitude - 幅度，必须大于等于 0，默认值 1
                    easingFunction = new BackEase() { Amplitude = 1 };
                    break;
                case "BounceEase":
                    // Bounces - 弹跳次数，必须大于等于 0，默认值 3
                    // Bounciness - 弹跳程度，必须是正数，默认值 2
                    easingFunction = new BounceEase() { Bounces = 3, Bounciness = 2 };
                    break;
                case "CircleEase":
                    easingFunction = new CircleEase();
                    break;
                case "CubicEase":
                    easingFunction = new CubicEase();
                    break;
                case "ElasticEase":
                    // Oscillations - 来回滑动的次数，必须大于等于 0，默认值 3
                    // Springiness - 弹簧的弹度，必须是正数，默认值 3
                    easingFunction = new ElasticEase() { Oscillations = 3, Springiness = 3 };
                    break;
                case "ExponentialEase":
                    easingFunction = new ExponentialEase();
                    break;
                case "PowerEase":
                    easingFunction = new PowerEase();
                    break;
                case "QuadraticEase":
                    easingFunction = new QuadraticEase();
                    break;
                case "QuarticEase":
                    easingFunction = new QuarticEase();
                    break;
                case "QuinticEase":
                    easingFunction = new QuinticEase();
                    break;
                case "SineEase":
                    easingFunction = new SineEase();
                    break;
                default:
                    break;
            }

            // 确定 Easing Mode
            switch ((cboEasingMode.SelectedItem as ComboBoxItem).Content.ToString())
            {
                case "EaseIn": // 渐进
                    easingFunction.EasingMode = EasingMode.EaseIn;
                    break;
                case "EaseOut": // 渐出（默认值）
                    easingFunction.EasingMode = EasingMode.EaseOut;
                    break;
                case "EaseInOut": // 前半段渐进，后半段渐出
                    easingFunction.EasingMode = EasingMode.EaseInOut;
                    break;
                default:
                    break;
            }

            // 用于演示缓动效果
            aniEasingDemo.EasingFunction = easingFunction;
            // 用于演示缓动轨迹
            aniBallY.EasingFunction = easingFunction;

            // 画出当前缓动的曲线图
            DrawEasingGraph(easingFunction);

            storyboard.Begin();
        }

        /// <summary>
        /// 绘制指定的 easing 的曲线图
        /// </summary>
        private void DrawEasingGraph(EasingFunctionBase easingFunction)
        {
            graph.Children.Clear();

            Path path = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure() { StartPoint = new Point(0, 0) };
            PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();

            // 0 - 1 之间每隔 0.005 计算出一段 LineSegment，用于显示此 0.005 时间段内的缓动曲线
            for (double i = 0; i < 1; i += 0.005)
            {
                double x = i * graphContainer.Width;
                double y = easingFunction.Ease(i) * graphContainer.Height;

                LineSegment segment = new LineSegment();
                segment.Point = new Point(x, y);
                pathSegmentCollection.Add(segment);
            }

            pathFigure.Segments = pathSegmentCollection;
            pathGeometry.Figures.Add(pathFigure);
            path.Data = pathGeometry;
            path.Stroke = new SolidColorBrush(Colors.Black);
            path.StrokeThickness = 1;

            graph.Children.Add(path);
        }
    }
}
