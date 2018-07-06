/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     IsHitTestVisible - 是否对命中测试可见
 *     AddHandler(RoutedEvent routedEvent, object handler, bool handledEventsToo) - 注册一个路由事件，注意最后一个参数：true 代表即使子辈 TappedRoutedEventArgs.Handled = true 也不会影响此元素事件的触发
 *     RemoveHandler(RoutedEvent routedEvent, object handler) - 移除指定的路由事件
 *     
 *     
 * RoutedEventArgs - 路由事件参数（有 n 多的派生类）
 *     OriginalSource - 引发此路由事件的对象
 * 
 * TappedRoutedEventArgs - Tapped 事件参数（继承自 RoutedEventArgs，详细说明请参见 /Controls/BaseControl/DependencyObjectDemo/TapDemo.xaml）
 *     Handled - 是否将路由事件标记为已处理
 *         true - 不再冒泡
 *         false - 继续冒泡
 *         
 *         
 * 本例用于演示 UIElement 的路由事件的注册，路由事件的冒泡，命中测试的可见性   
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class EventDemo : Page
    {
        public EventDemo()
        {
            this.InitializeComponent();

            // 为 borderRed 注册一个 TappedEventHandler 路由事件（注意最后一个参数：true 代表即使子辈 TappedRoutedEventArgs.Handled = true 也不会影响此元素事件的触发）
            borderRed.AddHandler(UIElement.TappedEvent, new TappedEventHandler(borderRed_Tapped), true);
        }

        private void borderRed_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblMsg.Text += "borderRed tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;
        }

        private void borderGreen_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblMsg.Text += "borderGreen tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;
        }

        private void borderBlue_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblMsg.Text += "borderBlue tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;

            // 不会再冒泡，也就是说 borderGreen 无法响应 Tapped 事件，但是 borderRed 注册 Tapped 事件时 handledEventsToo = true，所以 borderRed 会响应 Tapped 事件
            e.Handled = true;
        }

        private void borderOrange_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblMsg.Text += "borderOrange tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;
        }

        private void borderPurple_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // 不会响应此事件，因为 borderPurple 的 IsHitTestVisible = false
            lblMsg.Text += "borderPurple tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;
        }

        private void borderYellow_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // 不会响应此事件，因为 borderYellow 的爸爸 borderPurple 的 IsHitTestVisible = false
            lblMsg.Text += "borderYellow tapped, originalSource: " + (e.OriginalSource as FrameworkElement).Name;
            lblMsg.Text += Environment.NewLine;
        }
    }
}