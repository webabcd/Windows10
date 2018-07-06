/*
 * ToggleSwitch - 状态切换控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.SelectionControl
{
    public sealed partial class ToggleSwitchDemo : Page
    {
        public ToggleSwitchDemo()
        {
            this.InitializeComponent();
        }

        private void toggleSwitch1_Toggled(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"toggleSwitch1_Toggled, IsOn:{toggleSwitch1.IsOn}";
        }
    }
}
