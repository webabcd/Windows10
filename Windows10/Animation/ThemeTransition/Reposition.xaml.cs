using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeTransition
{
    public sealed partial class Reposition : Page
    {
        public Reposition()
        {
            this.InitializeComponent();
        }

        // 改变矩形的位置
        private void btnMove_Click(object sender, RoutedEventArgs e)
        {
            if (rectangle.Margin == new Thickness(0))
                rectangle.Margin = new Thickness(100);
            else
                rectangle.Margin = new Thickness(0);
        }
    }
}
