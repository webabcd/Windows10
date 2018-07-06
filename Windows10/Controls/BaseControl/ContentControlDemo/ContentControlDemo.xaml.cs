/*
 * ContentControl - ContentControl（继承自 Control, 请参见 /Controls/BaseControl/ControlDemo/）
 *     Content - 呈现的内容
 *     ContentTemplate - 数据模板（DataTemplate）
 *     ContentTemplateSelector - 数据模板选择器（如果指定了 ContentTemplate 则此配置无效）
 *     ContentTemplateRoot - 用于获取当前数据模板的根元素（写自定义 ContentControl 时会用到）
 *     ContentTransitions - Content 发生变化时的过度效果
 */

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Controls.BaseControl.ContentControlDemo
{
    public sealed partial class ContentControlDemo : Page
    {
        private IList<Employee> Employees { get; set; } = TestData.GetEmployees(100);

        public ContentControlDemo()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 注：
            // 在 Content 发生变化时会触发 ContentTemplateSelector 和 ContentTransitions（如果只是 DataContext 发生变化则不会有此效果）
            // 所以如果需要 ContentTemplateSelector 和 ContentTransitions 的话，则应该直接设置 ContentControl 的 Content 而不是 DataContext
            contentControl.Content =  Employees[new Random().Next(0, 100)];
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
