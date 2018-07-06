/*
 * AppBar - 应用程序栏控件（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 * 
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
    public sealed partial class AppBarDemo : Page
    {
        public AppBarDemo()
        {
            this.InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // 打开 AppBar
            appBar.IsOpen = true;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // 关闭 AppBar
            appBar.IsOpen = false;
        }

        private void chkIsSticky_Checked(object sender, RoutedEventArgs e)
        {
            // 点击非 AppBar 区域时，不会关闭 AppBar
            appBar.IsSticky = true;
        }

        private void chkIsSticky_Unchecked(object sender, RoutedEventArgs e)
        {
            // 点击非 AppBar 区域时，关闭 AppBar
            appBar.IsSticky = false;
        }

        private void chkIsCompact_Checked(object sender, RoutedEventArgs e)
        {
            var elements = buttonPanel.Children;
            foreach (var element in elements)
            {
                var button = element as ICommandBarElement;
                if (button != null)
                {
                    // IsCompact - 是否使用紧凑按钮，即是否隐藏按钮文本（来自 ICommandBarElement 接口。AppBarButton, AppBarToggleButton, AppBarSeparator 均实现了此接口）
                    //     true - 只显示按钮图标
                    //     false - 显示按钮图标和按钮文本
                    button.IsCompact = true;
                }
            }
        }

        private void chkIsCompact_Unchecked(object sender, RoutedEventArgs e)
        {
            var elements = buttonPanel.Children;
            foreach (var element in elements)
            {
                var button = element as ICommandBarElement;
                if (button != null)
                {
                    button.IsCompact = false;
                }
            }
        }

        private void radioButtonMinimal_Checked(object sender, RoutedEventArgs e)
        {
            if (appBar != null)
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
        }

        private void radioButtonHidden_Checked(object sender, RoutedEventArgs e)
        {
            if (appBar != null)
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Hidden;
        }

        private void radioButtonCompact_Checked(object sender, RoutedEventArgs e)
        {
            if (appBar != null)
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
        }
    }
}
