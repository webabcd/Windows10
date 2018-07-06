using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Entrance : Page
    {
        public Entrance()
        {
            this.InitializeComponent();
        }

        private void btnGotoFrame1_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Frame1));
        }

        private void btnGotoFrame2_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Frame2));
        }
    }
}
