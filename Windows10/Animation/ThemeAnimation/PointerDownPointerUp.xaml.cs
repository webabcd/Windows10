/*
 * PointerDownThemeAnimation - 鼠标（手指）在控件上按下时的动画
 * PointerUpThemeAnimation - 鼠标（手指）在控件上抬起时的动画
 * 
 * 
 * 注：
 * 调用 PointerDownThemeAnimation 的 Begin() 方法就是按下时的动画，再调用 PointerDownThemeAnimation 的 Stop() 方法就是抬起时的动画
 * 所以一般来说，只要使用 PointerDownThemeAnimation 的 Begin() 和 Stop() 即可，而不再需要 PointerUpThemeAnimation
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class PointerDownPointerUp : Page
    {
        public PointerDownPointerUp()
        {
            this.InitializeComponent();
        }

        private void btnPointerDownBegin_Click(object sender, RoutedEventArgs e)
        {
            storyboardPointerDown.Begin();
        }

        private void btnPointerDownStop_Click(object sender, RoutedEventArgs e)
        {
            storyboardPointerDown.Stop();
        }
    }
}
