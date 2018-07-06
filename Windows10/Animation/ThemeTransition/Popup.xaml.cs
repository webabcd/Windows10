using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Popup : Page
    {
        public Popup()
        {
            this.InitializeComponent();
        }

        // 显示 Popup
        private void btnPopup_Click(object sender, RoutedEventArgs e)
        {
            if (!popup.IsOpen)
                popup.IsOpen = true;
        }
    }
}
