/*
 * WrapGrid - 虚拟化布局控件，原 GridView 的默认布局控件（继承自 OrientedVirtualizingPanel, 请参见 /Controls/CollectionControl/ItemsControlDemo/LayoutControl/OrientedVirtualizingPanelDemo.xaml）
 *     在 uwp 中 GridView 的默认布局控件是 ItemsWrapGrid, 请参见 /Controls/CollectionControl/ItemsControlDemo/LayoutControl/ItemsWrapGridDemo.xaml
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo.LayoutControl
{
    public sealed partial class WrapGridDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(1000);

        public WrapGridDemo()
        {
            this.InitializeComponent();
        }
    }
}
