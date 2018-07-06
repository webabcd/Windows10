/*
 * TextBox - 文本输入框（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     IsColorFontEnabled - 是否以彩色方式显示 Segoe UI Emoji 之类的字符（默认值是 true） 
 *     SelectAll() - 选中全部内容（先要获取焦点后，才能做这个操作）
 *     Select(int start, int length) - 选中指定范围的内容（先要获取焦点后，才能做这个操作）
 *     SelectedText - 选中的内容
 *     SelectionStart - 选中内容的起始位置
 *     SelectionLength - 选中内容的字符数
 *     SelectionHighlightColor - 选中文本的颜色
 *     PreventKeyboardDisplayOnProgrammaticFocus - 通过 FocusState.Programmatic 让 TextBox 获取焦点时，是否不显示软键盘
 *     GetLinguisticAlternativesAsync() - 获取输入法编辑器 (IME) 窗口中的候选词列表（经测试仅微软输入法有效，也许是第三方输入法没有实现某个接口）
 *     GetRectFromCharacterIndex(int charIndex, bool trailingEdge) - 获取指定字符的边缘位置的 Rect 对象
 *         charIndex - 字符的位置
 *         trailingEdge - false 代表前边缘（左边缘），true 代表后边缘（右边缘）
 *
 *     TextBox 的相关事件的说明详见代码部分的注释
 */

using System;
using System.Text;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.TextControl
{
    public sealed partial class TextBoxDemo2 : Page
    {
        public TextBoxDemo2()
        {
            this.InitializeComponent();

            this.Loaded += TextBoxDemo2_Loaded;
        }

        private void TextBoxDemo2_Loaded(object sender, RoutedEventArgs e)
        {
            // 显示 Segoe UI Emoji 字符
            StringBuilder strContect = new StringBuilder();
            for (int code = 0x1F600; code < 0x1F6C6; code++)
            {
                strContect.Append(char.ConvertFromUtf32(code));
            }
            // 是否以彩色方式显示 Segoe UI Emoji 之类的字符（默认值是 true） 
            textBox1.IsColorFontEnabled = true;
            textBox1.Text = strContect.ToString();



            textBox2.Text = "123456";
            // 当通过 FocusState.Programmatic 让 textBox2 获取焦点时，不显示软键盘
            textBox2.PreventKeyboardDisplayOnProgrammaticFocus = true;
            // 通过 FocusState.Programmatic 让 textBox2 获取焦点
            textBox2.Focus(FocusState.Programmatic);
            // 获取焦点后，才能 Select() 或 SelectAll()
            textBox2.Select(1, 4); // SelectAll() - 选中全部
            textBox2.SelectionHighlightColor = new SolidColorBrush(Colors.Orange);

            textBlock2.Text = "SelectedText: " + textBox2.SelectedText;
            textBlock2.Text += Environment.NewLine;
            textBlock2.Text += "SelectionStart: " + textBox2.SelectionStart;
            textBlock2.Text += Environment.NewLine;
            textBlock2.Text += "SelectionLength: " + textBox2.SelectionLength;
            textBlock2.Text += Environment.NewLine;

            // 获取第 1 个字符的左边缘的 Rect 对象
            textBlock2.Text += "GetRectFromCharacterIndex(0, false): " + textBox2.GetRectFromCharacterIndex(0, false).ToString();
            textBlock2.Text += Environment.NewLine;
            // 获取第 1 个字符的右边缘的 Rect 对象
            textBlock2.Text += "GetRectFromCharacterIndex(0, true): " + textBox2.GetRectFromCharacterIndex(0, true).ToString();

            

            // TextBox 的相关事件的说明及演示如下

            // 当文本发生变化时触发的事件
            textBox3.TextChanging += TextBox3_TextChanging;
            // 当文本发生变化后触发的事件
            textBox3.TextChanged += TextBox3_TextChanged;

            // 选中的文本发生变化后触发的事件
            textBox3.SelectionChanged += TextBox3_SelectionChanged;
            // 在 TextBox 中做粘贴操作时触发的事件
            textBox3.Paste += TextBox3_Paste;
            // 在 TextBox 中打开上下文菜单时触发的事件（触摸屏长按或鼠标右键）
            textBox3.ContextMenuOpening += TextBox3_ContextMenuOpening;

            // 在打开、更新或关闭输入法编辑器 (IME) 窗口时触发的事件（经测试仅微软输入法有效，也许是第三方输入法没有实现某个接口）
            textBox3.CandidateWindowBoundsChanged += TextBox3_CandidateWindowBoundsChanged;
            // 当通过输入方法编辑器 (IME) 组成的文本出现变化时触发的事件
            textBox3.TextCompositionChanged += TextBox3_TextCompositionChanged;
            // 当用户开始通过输入方法编辑器 (IME) 组成文本时触发的事件
            textBox3.TextCompositionStarted += TextBox3_TextCompositionStarted;
            // 当用户停止通过输入方法编辑器 (IME) 组成文本时触发的事件
            textBox3.TextCompositionEnded += TextBox3_TextCompositionEnded;
        }

        private void TextBox3_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            textBlock3.Text += "TextChanging: " + textBox3.Text;
            textBlock3.Text += Environment.NewLine;
        }

        private void TextBox3_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBlock3.Text += "TextChanged: " + textBox3.Text;
            textBlock3.Text += Environment.NewLine;
        }

        private void TextBox3_SelectionChanged(object sender, RoutedEventArgs e)
        {
            textBlock3.Text += "SelectionChanged: " + textBox3.SelectedText;
            textBlock3.Text += Environment.NewLine;
        }

        private async void TextBox3_Paste(object sender, TextControlPasteEventArgs e)
        {
            await new MessageDialog("禁用粘贴").ShowAsync();

            // 将路由事件设置为已处理，从而禁用粘贴功能
            e.Handled = true;
        }

        private void TextBox3_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            textBlock3.Text += "ContextMenuOpening";
            textBlock3.Text += Environment.NewLine;
        }

        private void TextBox3_CandidateWindowBoundsChanged(TextBox sender, CandidateWindowBoundsChangedEventArgs args)
        {
            // CandidateWindowBoundsChangedEventArgs.Bounds - 获取 IME 窗口的 Rect 对象
            textBlock3.Text += "CandidateWindowBoundsChanged: " + args.Bounds.ToString();
            textBlock3.Text += Environment.NewLine;
        }

        private async void TextBox3_TextCompositionChanged(TextBox sender, TextCompositionChangedEventArgs args)
        {
            // TextCompositionChangedEventArgs.StartIndex - 通过 IME 组成的文本的起始位置
            // TextCompositionChangedEventArgs.Length - 通过 IME 组成的文本的长度
            textBlock3.Text += $"TextCompositionChanged StartIndex:{args.StartIndex}, Length:{args.Length}";
            textBlock3.Text += Environment.NewLine;


            // GetLinguisticAlternativesAsync() - 获取输入法编辑器 (IME) 窗口中的候选词列表（经测试仅微软输入法有效，也许是第三方输入法没有实现某个接口）
            var candidateWords = await textBox3.GetLinguisticAlternativesAsync();
            textBlock3.Text += "candidate words: " + string.Join(",", candidateWords); ;
            textBlock3.Text += Environment.NewLine;
        }

        private void TextBox3_TextCompositionStarted(TextBox sender, TextCompositionStartedEventArgs args)
        {
            // TextCompositionStartedEventArgs.StartIndex - 通过 IME 组成的文本的起始位置
            // TextCompositionStartedEventArgs.Length - 通过 IME 组成的文本的长度
            textBlock3.Text += $"TextCompositionStarted StartIndex:{args.StartIndex}, Length:{args.Length}";
            textBlock3.Text += Environment.NewLine;
        }

        private void TextBox3_TextCompositionEnded(TextBox sender, TextCompositionEndedEventArgs args)
        {
            // TextCompositionEndedEventArgs.StartIndex - 通过 IME 组成的文本的起始位置
            // TextCompositionEndedEventArgs.Length - 通过 IME 组成的文本的长度
            textBlock3.Text += $"TextCompositionEnded StartIndex:{args.StartIndex}, Length:{args.Length}";
            textBlock3.Text += Environment.NewLine;
        }
    }
}
