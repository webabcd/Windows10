/*
 * ItemsControl - 集合控件（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 * 
 * 
 * 本例用于演示 ItemsControl 如何通过 item 的不同而使用不同的数据模板
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.CollectionControl.ItemsControlDemo
{
    public sealed partial class ItemsControlDemo3 : Page
    {
        public ObservableCollection<Employee> Employees { get; set; } = TestData.GetEmployees(100);

        public ItemsControlDemo3()
        {
            this.InitializeComponent();
        }
    }

    // 自定义 DataTemplateSelector（数据模板选择器）
    // 可以实现在 runtime 时，根据 item 的不同选择不同的数据模板
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        // 数据模板 1（配置在 xaml 端）
        public DataTemplate DataTemplate1 { get; set; }

        // 数据模板 2（配置在 xaml 端）
        public DataTemplate DataTemplate2 { get; set; }

        // 根据 item 的数据的不同，指定的不同的模板
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var employee = item as Employee;
            if (employee == null || employee.IsMale)
                return DataTemplate1; // 男员工用数据模板 1
            return DataTemplate2; // 女员工用数据模板 2

            // 如果想直接返回指定的资源也是可以的（但是不灵活），类似：return (DataTemplate)Application.Current.Resources["DataTemplateMale"];
        }
    }
}
