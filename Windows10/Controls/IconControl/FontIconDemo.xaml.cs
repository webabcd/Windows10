/*
 * FontIcon - 字体图标（继承自 IconElement, 请参见 /Controls/IconControl/IconElementDemo.xaml）
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.IconControl
{
    public sealed partial class FontIconDemo : Page
    {
        public FontIconDemo()
        {
            this.InitializeComponent();

            this.Loaded += FontIconDemo_Loaded;
        }

        private void FontIconDemo_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 在 code-behind 中可以通过 \u 指定 Unicode 编码
            fontIcon3.Glyph = "\uEC52";
        }
    }
}
