/*
 * ComboBox - 下拉框控件（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 *     DropDownOpened - 下拉框打开（弹出选项列表）时触发的事件
 *     DropDownClosed - 下拉框关闭（隐藏选项列表）时触发的事件
 *     IsDropDownOpen - 下拉框是否处于打开状态
 *     MaxDropDownHeight - 下拉框打开后，其选项列表的最大高度
 *     SelectionBoxItem - 下拉框关闭后显示的数据对象（即下拉框的选项列表隐藏后，在下拉框中显示的数据对象
 *     
 *     
 * ComboBoxItem - 下拉框控件的 item（继承自 SelectorItem, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.SelectionControl
{
    public sealed partial class ComboBoxDemo : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(30);

        public ComboBoxDemo()
        {
            this.InitializeComponent();

            comboBox1.DropDownOpened += ComboBox1_DropDownOpened;
            comboBox1.DropDownClosed += ComboBox1_DropDownClosed;

            comboBox2.MaxDropDownHeight = 40;
            comboBox2.Loaded += (x, y) => 
            {
                // 注：如果要设置 IsDropDownOpen 属性的话，需要等到 ComboBox 加载后在设置
                comboBox2.IsDropDownOpen = true;
            };
        }
        
        private void ComboBox1_DropDownOpened(object sender, object e)
        {
            lblMsg1.Text = "comboBox1 DropDownOpened";
        }

        private void ComboBox1_DropDownClosed(object sender, object e)
        {
            // 通过 SelectionBoxItem 可获取 ComboBox 的选项列表隐藏后，在 ComboBox 中显示的数据对象
            lblMsg1.Text = $"comboBox1 DropDownClosed, SelectionBoxItem:{comboBox1.SelectionBoxItem}";
        }
    }
}
