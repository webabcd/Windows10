/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     GotFocus - 获取焦点时触发的事件（通过 api 获取焦点，或者通过鼠标获取焦点，或者通过 Tab 键获取焦点都会触发 GotFocus 事件）
 *     LostFocus - 丢失焦点时触发的事件
 *    
 *    
 * 本例用于演示 UIElement 的 Focus 相关事件的应用
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class FocusDemo : Page
    {
        public FocusDemo()
        {
            this.InitializeComponent();

            button1.GotFocus += Button1_GotFocus;
            button1.LostFocus += Button1_LostFocus;

            button2.GotFocus += Button2_GotFocus;
            button2.LostFocus += Button2_LostFocus;

            this.Loaded += FocusDemo_Loaded;
        }

        private void FocusDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 通过 api 获取焦点
            button2.Focus(FocusState.Programmatic);
        }

        private void Button1_GotFocus(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "Button1_GotFocus";
            lblMsg.Text += Environment.NewLine;
        }

        private void Button1_LostFocus(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "Button1_LostFocus";
            lblMsg.Text += Environment.NewLine;
        }

        private void Button2_GotFocus(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "Button2_GotFocus";
            lblMsg.Text += Environment.NewLine;
        }

        private void Button2_LostFocus(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += "Button2_LostFocus";
            lblMsg.Text += Environment.NewLine;
        }
    }
}
