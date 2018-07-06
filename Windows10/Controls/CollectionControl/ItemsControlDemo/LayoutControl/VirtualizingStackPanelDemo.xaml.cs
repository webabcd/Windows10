/*
 * VirtualizingStackPanel - 虚拟化布局控件，ListBox 的默认布局控件（继承自 OrientedVirtualizingPanel, 请参见 /Controls/CollectionControl/ItemsControlDemo/LayoutControl/OrientedVirtualizingPanelDemo.xaml）
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo.LayoutControl
{
    public sealed partial class VirtualizingStackPanelDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(1000);

        public VirtualizingStackPanelDemo()
        {
            this.InitializeComponent();
        }

        // 虚拟化缓存中的某一项数据被移除时触发的事件
        // 对于 Recycling 模式来说，老的数据被移除后会有新的数据添加进来 
        private void VirtualizingStackPanel_CleanUpVirtualizedItemEvent(object sender, CleanUpVirtualizedItemEventArgs e)
        {
            // 此次被移除虚拟化缓存的数据对象
            lblMsg.Text += "cleanUp: " + (e.Value as Employee).Name;
            lblMsg.Text += Environment.NewLine;

            // 此次被移除虚拟化缓存的 UIElement
            // UIElement element = e.UIElement;

            // 是否禁止此条虚拟化数据被移除
            // e.Cancel = false;
        }
    }
}
