/*
 * PasswordBox - 密码输入框（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     Password - 密码值
 *     SelectAll() - 选中 PasswordBox 中的所有字符（先要获取焦点后，才能做这个操作）
 *     SelectionHighlightColor - 选中文本的颜色
 *     PreventKeyboardDisplayOnProgrammaticFocus - 通过 FocusState.Programmatic 让 PasswordBox 获取焦点时，是否不显示软键盘
 *     PasswordChanged - Password 属性值发生变化时触发的事件
 *     Paste - 在 PasswordBox 中做粘贴操作时触发的事件
 *     ContextMenuOpening - 在 PasswordBox 中打开上下文菜单时触发的事件（触摸屏长按或鼠标右键）
 */

using System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.TextControl
{
    public sealed partial class PasswordBoxDemo : Page
    {
        public PasswordBoxDemo()
        {
            this.InitializeComponent();

            this.Loaded += PasswordBoxDemo_Loaded;
        }

        private void PasswordBoxDemo_Loaded(object sender, RoutedEventArgs e)
        {
            passwordBox2.Password = "123456";

            // 当通过 FocusState.Programmatic 让 passwordBox2 获取焦点时，不显示软键盘
            passwordBox2.PreventKeyboardDisplayOnProgrammaticFocus = true;
            // 通过 FocusState.Programmatic 让 passwordBox2 获取焦点
            passwordBox2.Focus(FocusState.Programmatic);

            passwordBox2.SelectAll(); // 先要获取焦点后，才能做这个操作
            passwordBox2.SelectionHighlightColor = new SolidColorBrush(Colors.Orange);

            passwordBox2.PasswordChanged += (x, y) =>
            {
                textBlock.Text = passwordBox2.Password;
            };

            passwordBox2.Paste += async (x, y) =>
            {
                await new MessageDialog("禁用粘贴").ShowAsync();

                // 将路由事件设置为已处理，从而禁用粘贴功能
                y.Handled = true;
            };

            passwordBox2.ContextMenuOpening += (x, y) =>
            {
                // 触发条件：触摸屏长按或鼠标右键 
            };
        }
    }
}
