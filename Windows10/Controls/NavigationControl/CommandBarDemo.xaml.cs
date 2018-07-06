/*
 * CommandBar - 应用程序栏控件。相对于 AppBar 来说， CommandBar 易用性强，扩展性弱（继承自 AppBar, 请参见 /Controls/NavigationControl/AppBarDemo.xaml）
 *
 * 注：
 * 1、当应用程序栏只有实现了 ICommandBarElement 接口（AppBarButton, AppBarToggleButton, AppBarSeparator）的控件的时候建议使用 CommandBar（文档推荐在 uwp 中使用 CommandBar）
 *    文档说 uwp 中的 CommandBar 内可以包含非 ICommandBarElement 接口的控件，但是实测发现并不可以
 * 2、如果除了实现了 ICommandBarElement 接口（AppBarButton, AppBarToggleButton, AppBarSeparator）的控件之外，应用程序栏还需要其他元素，则需要使用 AppBar
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.NavigationControl
{
    public sealed partial class CommandBarDemo : Page
    {
        public CommandBarDemo()
        {
            this.InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // 打开 CommandBar
            commandBar.IsOpen = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // 关闭 CommandBar
            commandBar.IsOpen = false;
        }

        private void chkIsSticky_Checked(object sender, RoutedEventArgs e)
        {
            // 点击非 CommandBar 区域时，不会关闭 CommandBar
            commandBar.IsSticky = true;
        }

        private void chkIsSticky_Unchecked(object sender, RoutedEventArgs e)
        {
            // 点击非 CommandBar 区域时，关闭 CommandBar
            commandBar.IsSticky = false;
        }

        private void radioButtonMinimal_Checked(object sender, RoutedEventArgs e)
        {
            if (commandBar != null)
                commandBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
        }

        private void radioButtonHidden_Checked(object sender, RoutedEventArgs e)
        {
            if (commandBar != null)
                commandBar.ClosedDisplayMode = AppBarClosedDisplayMode.Hidden;
        }

        private void radioButtonCompact_Checked(object sender, RoutedEventArgs e)
        {
            if (commandBar != null)
                commandBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
        }
    }
}
