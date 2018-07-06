/*
 * RichTextBlock - 富文本显示框（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     TextPointer GetPositionFromPoint(Point point) - 获取指定 Point 位置的 TextPointer 对象（关于 TextPointer 请参见 TextBlockDemo2.xaml.cs）
 */

using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.TextControl
{
    public sealed partial class RichTextBlockDemo : Page
    {
        public RichTextBlockDemo()
        {
            this.InitializeComponent();
        }

        private void richTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Point position = e.GetPosition(richTextBlock);
            TextPointer textPointer = richTextBlock.GetPositionFromPoint(position);

            textBlock.Text = $"TextPointer.Offset: {textPointer.Offset}";
        }
    }
}
