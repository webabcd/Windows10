/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     IsTapEnabled, IsDoubleTapEnabled, IsRightTapEnabled, IsHoldingEnabled - 是否允许监听相关的事件
 *     Tapped, DoubleTapped, RightTapped, Holding - Tap 相关事件
 *     
 * TappedRoutedEventArgs - Tap 路由事件的事件参数
 *     Handled - 是否将事件标记为已处理
 *     PointerDeviceType - 指针设备的类型（Touch, Pen, Mouse）
 *     GetPosition(UIElement relativeTo) - 返回当前指针相对于指定元素的位置
 *     
 * DoubleTappedRoutedEventArgs - DoubleTap 路由事件的事件参数
 *     Handled - 是否将事件标记为已处理
 *     PointerDeviceType - 指针设备的类型（Touch, Pen, Mouse）
 *     GetPosition(UIElement relativeTo) - 返回当前指针相对于指定元素的位置
 *     
 * RightTappedRoutedEventArgs - RightTap 路由事件的事件参数
 *     Handled - 是否将事件标记为已处理
 *     PointerDeviceType - 指针设备的类型（Touch, Pen, Mouse）
 *     GetPosition(UIElement relativeTo) - 返回当前指针相对于指定元素的位置
 * 
 * HoldingRoutedEventArgs - Holding 路由事件的事件参数
 *     Handled - 是否将事件标记为已处理
 *     PointerDeviceType - 指针设备的类型（Touch, Pen, Mouse）
 *     GetPosition(UIElement relativeTo) - 返回当前指针相对于指定元素的位置
 *     HoldingState - Holding 状态（Windows.UI.Input.HoldingState 枚举）
 *         Started, Completed, Canceled
 *         
 *         
 * 本例用于演示 UIElement 的 Tap 相关事件的应用
 * 
 * 
 * 注：关于 Manipulate Pointer Tap 的区别如下
 * 1、Manipulate 是最底层，Pointer 在中间，Tap 是最高层，所以会先走 Manipulate 事件，再走 Pointer 事件，最后走 Tap 事件
 * 2、如果高层的事件被触发，最相对于它的底层的事件也会被触发，反之则不一定
 * 3、使用原则是能在高层处理的事件尽量在高层处理（开发会简单些）
 */

using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class TapDemo : Page
    {
        public TapDemo()
        {
            this.InitializeComponent();

            rectangle.IsTapEnabled = true; // 默认值就是 true
            rectangle.IsDoubleTapEnabled = true; // 默认值就是 true
            rectangle.IsRightTapEnabled = true; // 默认值就是 true
            rectangle.IsHoldingEnabled = true; // 默认值就是 true

            rectangle.Tapped += rectangle_Tapped;
            rectangle.DoubleTapped += rectangle_DoubleTapped;
            rectangle.RightTapped += rectangle_RightTapped;
            rectangle.Holding += rectangle_Holding; // 鼠标操作时不会触发 Holding 事件
        }

        void rectangle_Holding(object sender, HoldingRoutedEventArgs e)
        {
            lblMsg.Text += "Holding";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            lblMsg.Text += "RightTapped";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            lblMsg.Text += "DoubleTapped";
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            lblMsg.Text += "Tapped";
            lblMsg.Text += Environment.NewLine;
        }
    }
}