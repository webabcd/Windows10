/*
 * Control - Control（继承自 UIElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     FocusState - 当前的焦点状态（FocusState 枚举）
 *         Unfocused - 无焦点
 *         Pointer - 通过指针获取的焦点
 *         Keyboard - 通过键盘获取的焦点
 *         Programmatic - 通过 api 获取的焦点
 *     bool Focus(FocusState value) - 设置焦点状态
 *     TabIndex - Tab 键导航顺序（按 tab 键正序导航；按 shift + tab 键倒序导航），默认值为 int.MaxValue
 *     IsTabStop - 是否包含在 Tab 导航中（即是否可获取到焦点）
 *     TabNavigation - Tab 键导航至 Control 内部时的效果
 *         Local - 当 focus 至 Control 时，会逐一 focus 此 Control 内部的所有 Control 元素，然后退出 focus
 *         Cycle - 当 focus 至 Control 时，会逐一 focus 此 Control 内部的所有 Control 元素，然后再继续无限循环 focus 此 Control 内部的所有 Control 元素
 *         Once - 当 focus 至 Control 时，只会 focus 此 Control 内部的第一个 Control 元素，然后退出 focus
 *     UseSystemFocusVisuals - 是否使用系统的焦点效果
 *         false - 在控件模板中设置焦点效果
 *         true - 使用系统的焦点效果（我这里测试的效果是，获取焦点后会显示一个虚线边框）
 *     IsTemplateFocusTarget - 是否是控件内用于获取焦点的元素（附加属性）
 *     GotFocus - 获取焦点时触发的事件（来自 UIElement）
 *     LostFocus - 丢失焦点时触发的事件（来自 UIElement）
 *     
 *     
 * 本例用于演示 Control 的焦点相关的知识点
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.BaseControl.ControlDemo
{
    public sealed partial class Demo2 : Page
    {
        public Demo2()
        {
            this.InitializeComponent();

            this.Loaded += Demo2_Loaded;
        }
        
        private void Demo2_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.GotFocus += TextBox1_GotFocus;
            textBox2.GotFocus += TextBox2_GotFocus;
            textBox3.GotFocus += TextBox3_GotFocus;
            textBox4.GotFocus += TextBox4_GotFocus;
            textBox5.GotFocus += TextBox5_GotFocus;
        }

        private void TextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox1.Text += textBox1.FocusState;
        }

        private void TextBox2_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox2.Text += textBox2.FocusState;
        }

        private void TextBox3_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox3.Text += textBox3.FocusState;
        }

        private void TextBox4_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox4.Text += textBox4.FocusState;
        }

        // 这里当 textBox5 获取到焦点时，立刻指定 textBox2 获取焦点，则会达到禁止 textBox5 获取焦点的同时手动指定下一个焦点对象
        // 如果只是禁止获取焦点的话可以设置 IsTabStop 为 false
        private void TextBox5_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox5.Text += textBox5.FocusState;

            // 设置为 FocusState.Unfocused 时会抛异常
            bool success = textBox2.Focus(FocusState.Programmatic);
        }

        private void cmbTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (itemsControl != null)
            {
                itemsControl.TabNavigation = (KeyboardNavigationMode)Enum.Parse(typeof(KeyboardNavigationMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
            }
        }
    }
}
