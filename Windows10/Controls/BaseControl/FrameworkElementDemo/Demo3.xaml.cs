/*
 * FrameworkElement - FrameworkElement（继承自 UIElement, 请参见 /Controls/BaseControl/UIElementDemo/）
 *     HorizontalAlignment - 水平对齐方式（Left, Center, Right, Stretch）
 *     VerticalAlignment - 垂直对齐方式（Top, Center, Bottom, Stretch）
 *     
 *     
 * 本例用于演示 FrameworkElement 的 HorizontalAlignment 和 VerticalAlignment 的应用
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.FrameworkElementDemo
{
    public sealed partial class Demo3 : Page
    {
        public Demo3()
        {
            this.InitializeComponent();
        }
        
        private void cmbHorizontalAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stackPanel.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

        private void cmbVerticalAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stackPanel.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

    }
}
