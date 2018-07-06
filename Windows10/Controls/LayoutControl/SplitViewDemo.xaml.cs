/*
 * SplitView - Pane/Content 控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 */

using System;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.LayoutControl
{
    public sealed partial class SplitViewDemo : Page
    {
        public SplitViewDemo()
        {
            this.InitializeComponent();
        }

        private void cmbDisplayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            splitView.DisplayMode = (SplitViewDisplayMode)Enum.Parse(typeof(SplitViewDisplayMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

        private void cmbPanePlacement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            splitView.PanePlacement = (SplitViewPanePlacement)Enum.Parse(typeof(SplitViewPanePlacement), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }
    }
}
