/*
 * RichEditBox - 富文本编辑框（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     Document - 文档对象，富文本编辑基本都是通过它实现的，本例中的示例代码简单介绍了如何使用，更详细的说明请参见文档
 *     其他属性、方法和事件与 TextBox 基本相同，相关演示请参见 TextBoxDemo1.xaml 和 TextBoxDemo2.xaml
 *
 * 
 * 本例通过开发一个简单的文本编辑器演示如何使用 RichEditBox 编辑文本
 */

using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.TextControl
{
    public sealed partial class RichEditBoxDemo : Page
    {
        public RichEditBoxDemo()
        {
            this.InitializeComponent();
        }

        // 使选中的文字变为斜体
        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            // 获取选中的文本
            ITextSelection selectedText = txtEditor.Document.Selection;
            if (selectedText != null)
            {
                // 实体化一个 ITextCharacterFormat，指定字符格式为斜体
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = FormatEffect.Toggle;

                // 设置选中文本的字符格式
                selectedText.CharacterFormat = charFormatting;
            }
        }

        // 使选中的文字加粗
        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            // 获取选中的文本
            ITextSelection selectedText = txtEditor.Document.Selection;
            if (selectedText != null)
            {
                // 实体化一个 ITextCharacterFormat，指定字符格式为加粗
                ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = FormatEffect.Toggle;

                // 设置选中文本的字符格式
                selectedText.CharacterFormat = charFormatting;
            }
        }

        // 保存已经被高亮的 ITextRange
        List<ITextRange> _highlightedWords = new List<ITextRange>();
        // 高亮显示用户搜索的字符
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // 清除高亮字符的高亮效果
            ITextCharacterFormat charFormat;
            for (int i = 0; i < _highlightedWords.Count; i++)
            {
                charFormat = _highlightedWords[i].CharacterFormat;
                charFormat.BackgroundColor = Colors.Transparent;
                _highlightedWords[i].CharacterFormat = charFormat;
            }
            _highlightedWords.Clear();

            // 获取全部文本，并将操作点移动到文本的起点
            ITextRange searchRange = txtEditor.Document.GetRange(0, TextConstants.MaxUnitCount);
            searchRange.Move(0, 0);

            bool textFound = true;
            do
            {
                // 在全部文本中搜索指定的字符串
                if (searchRange.FindText(txtSearch.Text, TextConstants.MaxUnitCount, FindOptions.None) < 1)
                {
                    textFound = false;
                }
                else
                {
                    _highlightedWords.Add(searchRange.GetClone());

                    // 实体化一个 ITextCharacterFormat，指定字符背景颜色为黄色
                    ITextCharacterFormat charFormatting = searchRange.CharacterFormat;
                    charFormatting.BackgroundColor = Colors.Orange;

                    // 设置指定文本的字符格式（高亮效果）
                    searchRange.CharacterFormat = charFormatting;
                }
            } while (textFound);
        }
    }
}
