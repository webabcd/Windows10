/*
 * SwipeHintThemeAnimation - 控件的 Swipe 动画（当你的控件在收到 Swipe 后会做响应时）
 * SwipeBackThemeAnimation - 控件的 Swipe 动画（当你的控件在收到 Swipe 后不需要做任何响应时）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class SwipeHintSwipeBack : Page
    {
        public SwipeHintSwipeBack()
        {
            this.InitializeComponent();
        }

        private void btnSwipeHint_Click(object sender, RoutedEventArgs e)
        {
            storyboardSwipeHint.Begin();
        }

        private void btnSwipeBack_Click(object sender, RoutedEventArgs e)
        {
            storyboardSwipeBack.Begin();
        }
    }
}
