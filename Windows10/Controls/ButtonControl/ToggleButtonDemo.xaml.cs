/*
 * ToggleButton - 可切换状态的按钮（继承自 ButtonBase, 请参见 /Controls/ButtonControl/ButtonBaseDemo.xaml）
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.ButtonControl
{
    public sealed partial class ToggleButtonDemo : Page
    {
        public ToggleButtonDemo()
        {
            this.InitializeComponent();

            this.Loaded += ToggleButtonDemo_Loaded;
        }

        private void toggleButton1_Checked(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton1_Checked, IsChecked:{toggleButton1.IsChecked}";
        }

        private void toggleButton1_Unchecked(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton1_Unchecked, IsChecked:{toggleButton1.IsChecked}";
        }

        // 这个事件不会被触发，因为 toggleButton1 的 IsThreeState 的值为 false
        private void toggleButton1_Indeterminate(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton1_Indeterminate, IsChecked:{toggleButton1.IsChecked}";
        }



        private void toggleButton2_Checked(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton2_Checked, IsChecked:{toggleButton2.IsChecked}";
        }

        private void toggleButton2_Unchecked(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton2_Unchecked, IsChecked:{toggleButton2.IsChecked}";
        }

        private void toggleButton2_Indeterminate(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleButton2_Indeterminate, IsChecked:{toggleButton2.IsChecked}";
        }



        private void ToggleButtonDemo_Loaded(object sender, RoutedEventArgs e)
        {
            lblToggleButton3.Text += "Page_Loaded";
            lblToggleButton3.Text += Environment.NewLine;
        }

        private void toggleButton3_Loaded(object sender, RoutedEventArgs e)
        {
            lblToggleButton3.Text += "toggleButton3_Loaded";
            lblToggleButton3.Text += Environment.NewLine;
        }

        private void toggleButton3_Checked(object sender, RoutedEventArgs e)
        {
            lblToggleButton3.Text += "toggleButton3_Checked";
            lblToggleButton3.Text += Environment.NewLine;
        }
    }
}
