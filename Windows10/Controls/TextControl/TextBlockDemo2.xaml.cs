/*
 * TextBlock - 文本显示框（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     SelectedText - 获取选中的文本内容
 *     SelectionChanged - 选中的文本内容发生变化后触发的事件
 *     ContentStart - 开头内容的 TextPointer 对象
 *     ContentEnd - 结尾内容的 TextPointer 对象
 *     Focus(FocusState value) - 获取焦点
 *     SelectAll() - 选中全部内容（先要获取焦点后，才能做这个操作）
 *     Select(TextPointer start, TextPointer end) - 选中指定范围的内容（先要获取焦点后，才能做这个操作）
 *     SelectionStart - 选中内容的起始位置（TextPointer 对象）
 *     SelectionEnd - 选中内容的结束位置（TextPointer 对象）
 *     BaselineOffset - 获取基线的位置（什么是基线：英文字符的基线基本相当于单词本4条线中的第3条线）
 *     IsColorFontEnabled - 是否以彩色方式显示 Segoe UI Emoji 之类的字符（默认值是 true） 
 *
 *
 * TextPointer - 文本框中的指针对象
 *     Offset - 指针的位置
 *     LogicalDirection - 指针的逻辑方向
 *         Backward - 向后，即从右到左（比如，如果插入字符的话，就会在指针位置的左边插入）
 *         Forward - 向前，即从左到右（比如，如果插入字符的话，就会在指针位置的右边插入）
 *     Rect GetCharacterRect(LogicalDirection direction) - 返回当前指针的矩形框
 *     TextPointer GetPositionAtOffset(offset, LogicalDirection direction) - 将指针位置偏移指定的距离（正代表向右偏移，负代表向左偏移）
 */

using System;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Windows10.Controls.TextControl
{
    public sealed partial class TextBlockDemo2 : Page
    {
        public TextBlockDemo2()
        {
            this.InitializeComponent();

            this.Loaded += TextBlockDemo2_Loaded;
        }

        private void TextBlockDemo2_Loaded(object sender, RoutedEventArgs e)
        {
            textBlock1.SelectionChanged += (x, y) =>
            {
                // 显示用户选中的文本内容
                lblMsg1.Text = textBlock1.SelectedText;
            };

            textBlock1.ContextMenuOpening += (x, y) =>
            {
                // 触发条件：触摸屏长按或鼠标右键 
            };



            // 获取焦点
            textBlock2.Focus(FocusState.Programmatic);
            TextPointer start = textBlock2.ContentStart;
            TextPointer end = textBlock2.ContentEnd;
            // textBlock2 的 Text 的内容一共有 26 个字符，但是这里的指针位置 offset 是从 0 到 34，一共 35 个指针位置
            // <LineBreak /> 占 4 个指针位置，开头占 2 个指针位置，结尾占 2 个指针位置。剩下的就比较好理解了，就是 26 个字符拥有 27 个指针位置
            lblMsg2.Text = $"ContentStart.Offset: {start.Offset}, ContentEnd.Offset: {end.Offset}";
            lblMsg2.Text += Environment.NewLine;

            // 从左到右偏移 3 个位置，即指针位置在第 1 个字符的右边
            start = start.GetPositionAtOffset(3, LogicalDirection.Backward);
            // 从右到左偏移 3 个位置，即指针位置在最后一个字符的左边
            end = end.GetPositionAtOffset(-3, LogicalDirection.Forward);
            textBlock2.Select(start, end); // SelectAll() - 选中全部
            lblMsg2.Text += $"SelectionStart.Offset: {textBlock2.SelectionStart.Offset}, SelectionEnd.Offset: {textBlock2.SelectionEnd.Offset}";



            // 获取基线的位置，并通过 Line 绘制基线
            lblMsg3.Text += $"BaselineOffset: {textBlock3.BaselineOffset}";
            line.Y1 = textBlock3.BaselineOffset;
            line.Y2 = textBlock3.BaselineOffset;

            

            // 显示 Segoe UI Emoji 字符
            StringBuilder strContect = new StringBuilder();
            for (int code = 0x1F600; code < 0x1F6C6; code++)
            {
                strContect.Append(char.ConvertFromUtf32(code));
            }
            // 是否以彩色方式显示 Segoe UI Emoji 之类的字符（默认值是 true） 
            textBlock4.IsColorFontEnabled = true;
            textBlock4.Text = strContect.ToString();



            // 通过封一层 Grid 的方式计算 TextBlock 的实际宽度和实际高度
            lblMsg5.Text = $"textBlock5 的实际高度: {grid.ActualHeight}";
            lblMsg5.Text += Environment.NewLine;
            lblMsg5.Text += $"textBlock5 的实际宽度: {gridColumn1.ActualWidth}";
            lblMsg5.Text += Environment.NewLine;

            // 通过 TextPointer 的方式计算 TextBlock 的实际高度
            TextPointer last = textBlock5.ContentEnd;
            last = last.GetPositionAtOffset(-3, LogicalDirection.Forward);
            Rect rectLast = last.GetCharacterRect(LogicalDirection.Forward);
            lblMsg5.Text += $"textBlock5 的实际高度: {rectLast.Bottom}";
            lblMsg5.Text += Environment.NewLine;

            // 通过 TextPointer 的方式计算 TextBlock 的实际宽度
            int count = textBlock5.ContentEnd.Offset - textBlock5.ContentStart.Offset;
            double width = 0;
            for (int i = 0; i < count; i++)
            {
                TextPointer current = textBlock5.ContentStart.GetPositionAtOffset(i, LogicalDirection.Backward);
                width = Math.Max(width, current.GetCharacterRect(LogicalDirection.Backward).Right);
            }
            lblMsg5.Text += $"textBlock5 的实际宽度: {width}";
        }
    }
}
