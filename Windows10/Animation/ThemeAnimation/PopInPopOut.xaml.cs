/*
 * PopInThemeAnimation - 控件出现时的动画
 * PopOutThemeAnimation - 控件消失时的动画
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class PopInPopOut : Page
    {
        public PopInPopOut()
        {
            this.InitializeComponent();
        }

        private void btnPopIn_Click(object sender, RoutedEventArgs e)
        {
            storyboardPopIn.Begin();
        }

        private void btnPopOut_Click(object sender, RoutedEventArgs e)
        {
            storyboardPopOut.Begin();
        }
    }
}
