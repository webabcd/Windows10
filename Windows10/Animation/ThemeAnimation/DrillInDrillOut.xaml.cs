/*
 * DrillInThemeAnimation - 有层次关系的，从上级到下级的导航动画（master 到 details）
 * DrillOutThemeAnimation - 有层次关系的，从下级到上级的导航动画（details 到 master）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Animation.ThemeAnimation
{
    public sealed partial class DrillInDrillOut : Page
    {
        public DrillInDrillOut()
        {
            this.InitializeComponent();
        }

        private void btnDrillIn_Click(object sender, RoutedEventArgs e)
        {
            storyboardDrillIn.Begin();
        }

        private void btnDrillOut_Click(object sender, RoutedEventArgs e)
        {
            storyboardDrillOut.Begin();
        }
    }
}
