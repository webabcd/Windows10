using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Pane : Page
    {
        public Pane()
        {
            this.InitializeComponent();
        }

        // 显示 Pane
        private void btnShowPane_Click(object sender, RoutedEventArgs e)
        {
            if (!popup.IsOpen)
                popup.IsOpen = true;
        }
    }
}
