/*
 * ListBox - 列表框控件（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 *     SelectionMode - 选择的模式
 *         Single - 单选（默认）
 *         Multiple - 仅通过鼠标多选
 *         Extended - 通过鼠标和辅助键（ctrl, shift）多选
 *     ScrollIntoView(object item) - 滚动到指定数据对象
 *     SelectAll() - 选中所有项
 *     SelectedItems - 获取当前选中的数据对象集合
 *     
 *     
 * ListBoxItem - 列表框控件的 item（继承自 SelectorItem, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;
using System.Linq;

namespace Windows10.Controls.SelectionControl
{
    public sealed partial class ListBoxDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(30);

        public ListBoxDemo()
        {
            this.InitializeComponent();

            // 通过鼠标结合 ctrl键 shift键 多选
            listBox1.SelectionMode = SelectionMode.Extended;

            // 仅通过鼠标多选
            listBox2.SelectionMode = SelectionMode.Multiple;
            listBox2.Loaded += ListBox2_Loaded;
        }

        private void ListBox2_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            listBox2.SelectAll();
            // 滚动到最后一条数据
            listBox2.ScrollIntoView(this.Employees.Last());

            lblMsg2.Text = string.Join(", ", listBox2.SelectedItems.Cast<Employee>().Select(p => p.Name));
        }
    }
}
