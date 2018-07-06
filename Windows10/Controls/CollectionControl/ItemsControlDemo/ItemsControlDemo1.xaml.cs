/*
 * ItemsControl - 集合控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     ItemsPanelRoot - 用于获取 items 的布局控件（返回一个 Panel 类型的对象）
 *     
 * 
 * 本例用于演示 ItemsControl 的基础知识
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class ItemsControlDemo1 : Page
    {
        public ItemsControlDemo1()
        {
            this.InitializeComponent();

            this.Loaded += ItemsControlDemo1_Loaded;
        }

        private void ItemsControlDemo1_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "items 的布局控件: " + itemsControl.ItemsPanelRoot.GetType().ToString();
        }
    }
}