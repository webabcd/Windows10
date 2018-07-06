/*
 * 本例用于演示如何通过 Storyboard 使用线性动画，线性动画一共有 3 种类型：ColorAnimation, DoubleAnimation, PointAnimation, 它们均继承自 Timeline
 */

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Windows10.Animation
{
    public sealed partial class LinearAnimation : Page
    {
        public LinearAnimation()
        {
            this.InitializeComponent();

            this.Loaded += LinearAnimation_Loaded;
        }

        private void LinearAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            // 启动动画
            storyboardPoint.Begin();

            // 停止动画
            // storyboardPoint.Stop();



            // 用于演示如何在 CodeBehind 端定义 Storyboard
            // 定义一个 ColorAnimation
            ColorAnimation ca = new ColorAnimation();
            ca.BeginTime = TimeSpan.Zero;
            ca.From = Colors.Red;
            ca.To = Colors.Yellow;
            ca.Duration = TimeSpan.FromSeconds(3);
            ca.AutoReverse = true;
            ca.RepeatBehavior = new RepeatBehavior(3);
            Storyboard.SetTarget(ca, ellipse2);
            Storyboard.SetTargetProperty(ca, "(Fill).(Color)");

            // 定义一个 DoubleAnimation
            DoubleAnimation da = new DoubleAnimation()
            {
                To = 0.9,
                Duration = TimeSpan.FromSeconds(1),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(da, ellipse2);
            Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)");
            // 注：要用 Storyboard 控制 ScaleTransform 则必须先为元素声明好 ScaleTransform（否则运行时会报错）
            TransformGroup Group = new TransformGroup();
            Group.Children.Add(new ScaleTransform());
            ellipse2.RenderTransform = Group;

            // 将定义好的动画添加进 Storyboard 然后启动动画
            Storyboard sb = new Storyboard();
            sb.Children.Add(ca);
            sb.Children.Add(da);
            sb.Begin();


            /*
             * 注意：
             * 1、在 WinRT 中为了流畅的体验，部分动画被优化成了“独立动画”，即动画不依赖于 UI 线程
             * 2、但是也有一部分动画无法优化成“独立动画”，我们把这类动画称作“依赖动画”，其需要在 UI 线程上运行
             * 3、通过将 EnableDependentAnimation 设置为 true（默认为 false），开启“依赖动画”
             * 4、通过将 Timeline.AllowDependentAnimations 设置为 false（默认为 true），可以全局禁止开启“依赖动画”
            */
        }
    }
}
