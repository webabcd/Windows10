/*
 * SplitOpenThemeAnimation - 打开“拆分”控件的动画
 * SplitCloseThemeAnimation - 关闭“拆分”控件的动画
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class SplitOpenSplitClose : Page
    {
        public SplitOpenSplitClose()
        {
            this.InitializeComponent();
        }

        private void btnSplitOpen_Click(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Name = "textBlock";
            textBlock.Text = "我是 Border 里的内容";
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;

            border.Child = textBlock;

            storyboardSplitOpen.Begin();
        }

        private void btnSplitClose_Click(object sender, RoutedEventArgs e)
        {
            storyboardSplitClose.Begin();
        }
    }
}
