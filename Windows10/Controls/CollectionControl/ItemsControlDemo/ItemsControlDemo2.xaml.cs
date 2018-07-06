/*
 * ItemsControl - 集合控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     DependencyObject ContainerFromIndex(int index) - 获取指定索引位置的 item 的 container
 *     DependencyObject ContainerFromItem(object item) - 获取指定数据对象的 item 的 container
 *     int IndexFromContainer(DependencyObject container) - 获取指定 ItemContainer 的索引位置
 *     object ItemFromContainer(DependencyObject container) - 获取指定 ItemContainer 的绑定对象
 *     
 * 
 * 本例用于演示 ItemsControl 的数据绑定
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class ItemsControlDemo2 : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(100);

        public ItemsControlDemo2()
        {
            this.InitializeComponent();

            listView.Loaded += ListView_Loaded;
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取第 4 条数据的 ItemContainer
            ListViewItem itemContainer1 = listView.ContainerFromIndex(3) as ListViewItem;

            // 获取第 1 条数据的 ItemContainer
            ListViewItem itemContainer2 = listView.ContainerFromItem(Employees.First()) as ListViewItem;

            // 获取 itemContainer1 的索引位置
            int index = listView.IndexFromContainer(itemContainer1);

            // 获取 itemContainer2 的绑定对象
            Employee employee = listView.ItemFromContainer(itemContainer2) as Employee;
        }
    }
}
