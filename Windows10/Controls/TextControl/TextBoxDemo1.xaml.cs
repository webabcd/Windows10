/*
 * TextBox - 文本输入框（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.TextControl
{
    public sealed partial class TextBoxDemo1 : Page
    {
        public TextBoxDemo1()
        {
            this.InitializeComponent();

            this.Loaded += TextBoxDemo1_Loaded;
        }

        private void TextBoxDemo1_Loaded(object sender, RoutedEventArgs e)
        {
            // 在 CodeBehind 端设置 TextBox 的 InputScope
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName();

            name.NameValue = InputScopeNameValue.ChineseFullWidth;
            scope.Names.Add(name);

            textBox4.InputScope = scope;
        }
    }
}