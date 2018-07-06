/*
 * DependencyObject - 依赖对象（可以在 DependencyObject 上定义 DependencyProperty）
 *     SetValue(DependencyProperty dp, object value) - 设置依赖属性的值
 *     ClearValue(DependencyProperty dp) - 清除依赖属性的值
 *     GetValue(DependencyProperty dp) - 获取依赖属性的当前值
 *     GetAnimationBaseValue(DependencyProperty dp) - 获取依赖属性的基值
 *     ReadLocalValue(DependencyProperty dp) - 获取依赖属性的本地值
 *     
 *     
 * 本例用于演示 DependencyObject 的依赖属性的设置与获取
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Controls.BaseControl.DependencyObjectDemo
{
    public sealed partial class DependencyPropertyDemo : Page
    {
        public DependencyPropertyDemo()
        {
            this.InitializeComponent();

            this.Loaded += DependencyPropertyDemo_Loaded;
        }

        private void DependencyPropertyDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 直接设置依赖属性（rect1.Width = 400; 其实调用的就是这个）
            rect1.SetValue(FrameworkElement.WidthProperty, 400);


            // 直接设置依赖属性
            rect2.SetValue(FrameworkElement.WidthProperty, 200);
            // 通过动画的方式修改依赖属性的值
            SetRectWidth(rect2);


            // 通过 Style 的方式设置依赖属性
            Style style = new Style();
            style.TargetType = typeof(Rectangle);
            Setter setter = new Setter();
            setter.Property = Rectangle.WidthProperty;
            setter.Value = 200;
            style.Setters.Add(setter);
            rect3.Style = style;
            // 通过动画的方式修改依赖属性的值
            SetRectWidth(rect3);


            // 清除依赖属性的值
            rect4.ClearValue(FrameworkElement.WidthProperty);
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            DependencyProperty dp = FrameworkElement.WidthProperty;

            // 通过 SetValue 设置依赖属性后，再通过 GetValue, GetAnimationBaseValue, ReadLocalValue 取出的值和设置的值都是一样的
            lblMsg.Text = $"rect1 GetValue:{rect1.GetValue(dp)}, GetAnimationBaseValue:{rect1.GetAnimationBaseValue(dp)}, ReadLocalValue:{rect1.ReadLocalValue(dp)}";
            lblMsg.Text += Environment.NewLine;

            // 通过 SetValue 设置依赖属性后，再通过动画的方式修改其值
            // GetValue - 获取当前值
            // GetAnimationBaseValue - 获取基值，也就是动画之前的值
            // ReadLocalValue - 获取本地值，即通过 SetValue 或者资源或者绑定设置的值
            lblMsg.Text += $"rect2 GetValue:{rect2.GetValue(dp)}, GetAnimationBaseValue:{rect2.GetAnimationBaseValue(dp)}, ReadLocalValue:{rect2.ReadLocalValue(dp)}";
            lblMsg.Text += Environment.NewLine;

            // 通过 Style 设置依赖属性后，再通过动画的方式修改其值
            // GetValue - 获取当前值
            // GetAnimationBaseValue - 获取基值，也就是动画之前的值
            // ReadLocalValue - 获取本地值，即通过 SetValue 或者资源或者绑定设置的值（如果是通过其他方式，比如 Style 方式设置的值，则无本地值）
            lblMsg.Text += $"rect3 GetValue:{rect3.GetValue(dp)}, GetAnimationBaseValue:{rect3.GetAnimationBaseValue(dp)}, ReadLocalValue:{rect3.ReadLocalValue(dp)}";
            lblMsg.Text += Environment.NewLine;

            // 通过 ClearValue 清除了依赖属性的值，则再通过 GetValue, GetAnimationBaseValue, ReadLocalValue 均无法获取到值
            lblMsg.Text += $"rect4 GetValue:{rect4.GetValue(dp)}, GetAnimationBaseValue:{rect4.GetAnimationBaseValue(dp)}, ReadLocalValue:{rect4.ReadLocalValue(dp)}, ActualWidth:{rect4.ActualWidth}";
        }


        private void SetRectWidth(Rectangle rect)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.EnableDependentAnimation = true;
            da.BeginTime = TimeSpan.Zero;
            da.To = 400;
            da.Duration = TimeSpan.FromSeconds(5);
            Storyboard.SetTarget(da, rect);
            Storyboard.SetTargetProperty(da, nameof(rect.Width));

            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }
    }
}
