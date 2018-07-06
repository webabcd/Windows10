using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class EdgeUI : Page
    {
        public EdgeUI()
        {
            this.InitializeComponent();
        }

        // 显示 EdgeUI
        private void btnShowEdgeUI_Click(object sender, RoutedEventArgs e)
        {
            if (!popup.IsOpen)
                popup.IsOpen = true;
        }
    }
}
