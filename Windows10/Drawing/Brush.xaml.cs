using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Drawing
{
    public sealed partial class Brush : Page
    {
        public Brush()
        {
            this.InitializeComponent();
        }

        private void webView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            // WebView 加载完毕后重绘 WebViewBrush
            webViewBrush.Redraw();
        }
    }
}
