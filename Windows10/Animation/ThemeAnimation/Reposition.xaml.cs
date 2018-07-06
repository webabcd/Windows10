/*
 * RepositionThemeAnimation - 控件重新定位时的动画
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class Reposition : Page
    {
        public Reposition()
        {
            this.InitializeComponent();
        }

        private void btnReposition_Click(object sender, RoutedEventArgs e)
        {
            storyboardReposition.Begin();
        }
    }
}
