/*
 * FadeInThemeAnimation - 控件淡入的动画
 * FadeOutThemeAnimation - 控件淡出的动画
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class FadeInFadeOut : Page
    {
        public FadeInFadeOut()
        {
            this.InitializeComponent();
        }

        private void btnFadeIn_Click(object sender, RoutedEventArgs e)
        {
            storyboardFadeIn.Begin();
        }

        private void btnFadeOut_Click(object sender, RoutedEventArgs e)
        {
            storyboardFadeOut.Begin();
        }
    }
}
