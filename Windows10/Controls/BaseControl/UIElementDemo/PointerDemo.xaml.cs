/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     PointerEntered - 指针进入时触发的事件
 *     PointerExited - 指针离开时触发的事件
 *     PointerPressed - 指针按下时触发的事件
 *     PointerReleased -  指针释放时触发的事件
 *     PointerCanceled - 接触中的指针异常地失去接触时触发的事件（比如修改分辨率，用户注销等）
 *         所以要注意：PointerPressed 和 PointerReleased 也许并不会成对出现，因为如果触发了 PointerCanceled 则不会再触发 PointerReleased
 *     PointerMoved - 指针移动时触发的事件
 *     PointerWheelChanged - 滚轮操作时触发的事件
 *     PointerCaptureLost - 指针捕获丢失时触发的事件
 *     CapturePointer(Pointer value) - 捕获此 UIElement 上的指针，即在 UIElement 之外也可以响应 PointerReleased 事件
 *     ReleasePointerCapture(Pointer value) - 释放此 UIElement 上的指定指针的捕获
 *     ReleasePointerCaptures() - 释放此 UIElement 上的全部指针的捕获
 *     IReadOnlyList<Pointer> PointerCaptures - 获取此 UIElement 上的被捕获的指针集合
 * 
 * 
 * PointerRoutedEventArgs - 指针路由事件的事件参数
 *     OriginalSource - 引发此路由事件的对象
 *     Handled - 是否将事件标记为已处理
 *     KeyModifiers - 获取当前按下的辅助键（Windows.System.VirtualKeyModifiers 枚举）
 *         None, Control, Menu, Shift, Windows
 *     Pointer - 获取 Pointer 对象
 *         Pointer.PointerDeviceType - 指针设备的类型（Touch, Pen, Mouse）
 *         Pointer.PointerId - 指针标识，可以根据此属性来区分多点触摸场景下的不同指针或者不同设备的指针
 *     GetCurrentPoint(UIElement relativeTo) - 返回当前指针相对于指定元素的 PointerPoint 对象
 *         PointerPoint.Position -  指针的位置
 *         PointerPoint.Properties - 返回 PointerPointProperties 对象，有一堆 PointerPoint 的相关属性（示例中有说明）
 *     GetIntermediatePoints(UIElement relativeTo) - 返回当前指针相对于指定元素的 PointerPoint 对象的全部历史记录集合
 *    
 *    
 * 本例用于演示 UIElement 的 Pointer 相关事件的应用
 * 
 * 
 * 注：关于 Manipulate Pointer Tap 的区别如下
 * 1、Manipulate 是最底层，Pointer 在中间，Tap 是最高层，所以会先走 Manipulate 事件，再走 Pointer 事件，最后走 Tap 事件
 * 2、如果高层的事件被触发，最相对于它的底层的事件也会被触发，反之则不一定
 * 3、使用原则是能在高层处理的事件尽量在高层处理（开发会简单些）
 */

using System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class PointerDemo : Page
    {
        public PointerDemo()
        {
            this.InitializeComponent();

            // 修改指针的样式
            // Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Cross, 1);

            rectangle.PointerEntered += rectangle_PointerEntered;
            rectangle.PointerExited += rectangle_PointerExited;
            rectangle.PointerPressed += rectangle_PointerPressed;
            rectangle.PointerReleased += rectangle_PointerReleased;
            rectangle.PointerMoved += rectangle_PointerMoved;
            rectangle.PointerWheelChanged += rectangle_PointerWheelChanged;
            rectangle.PointerCanceled += rectangle_PointerCanceled;
            rectangle.PointerCaptureLost += rectangle_PointerCaptureLost;
        }

        void rectangle_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerCaptureLost " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerCanceled " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            // 判断鼠标滚轮的滚动方向
            lblMsg.Text += "PointerWheelChanged " + e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // lblMsg.Text += "PointerMoved " + e.Pointer.PointerId;
            // lblMsg.Text += Environment.NewLine;
        }

        void rectangle_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerReleased " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;

            ((Rectangle)sender).ReleasePointerCapture(e.Pointer);
        }

        void rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerPressed " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;

            bool hasCapture = ((Rectangle)sender).CapturePointer(e.Pointer);
            lblMsg.Text += "Got Capture: " + hasCapture;
            lblMsg.Text += Environment.NewLine;

            PointerPointProperties props = e.GetCurrentPoint(null).Properties;
            lblMsg.Text += "接触区域的边框: " + props.ContactRect.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "原始输入的边框: " + props.ContactRectRaw.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "触笔设备的筒状按钮是否按下: " + props.IsBarrelButtonPressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否已由指针设备取消: " + props.IsCanceled.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自橡皮擦: " + props.IsEraser.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自滚轮: " + props.IsHorizontalMouseWheel.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针是否在触摸屏的范围内: " + props.IsInRange.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "是否是反转的值: " + props.IsInverted.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自鼠标左键: " + props.IsLeftButtonPressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自鼠标中键: " + props.IsMiddleButtonPressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自鼠标右键: " + props.IsRightButtonPressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "输入是否来自主要指针: " + props.IsPrimary.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "第一个扩展按钮的按下状态: " + props.IsXButton1Pressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "第二个扩展按钮的按下状态: " + props.IsXButton2Pressed.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针施加到触摸屏上的力度（0.0-1.0）: " + props.Pressure.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "触摸是否被拒绝了: " + props.TouchConfidence.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针状态的更改类型: " + props.PointerUpdateKind.ToString(); // PointerUpdateKind 枚举：LeftButtonPressed, LeftButtonReleased 等（详见文档）
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针设备相关的 Orientation: " + props.Orientation.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针设备相关的 Twist: " + props.Twist.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针设备相关的 XTilt: " + props.XTilt.ToString();
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "指针设备相关的 YTiltYTilt: " + props.YTilt.ToString();
            lblMsg.Text += Environment.NewLine;

            // 输入设备相关
            // props.HasUsage(uint usagePage, uint usageId)
            // props.GetUsageValue(uint usagePage, uint usageId)
        }

        void rectangle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerExited " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;
        }

        void rectangle_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            lblMsg.Text += "PointerEntered " + e.Pointer.PointerId;
            lblMsg.Text += Environment.NewLine;
        }
    }
}