/*
 * 演示 x:Bind 绑定的相关知识点
 * 
 * 
 * 如果需要数据源在属性值发生变化时对外通知，则需要实现 INotifyPropertyChanged 接口（为了简化实现，建议继承 Common/BindableBase.cs 这个类）
 *     PropertyChanged - 对象的属性的值发生变化时触发的事件
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Bind
{
    // x:Bind 的数据上下文就是它所属的 Page 或 UserControl
    public sealed partial class BindDemo : Page
    {
        public BindDemo()
        {
            this.InitializeComponent();
        }

        // 事件绑定到方法，无参数
        private void EventBindNoArgs()
        {
            CurrentEmployee.Name = "wanglei" + new Random().Next(1000, 10000).ToString();
        }

        // 事件绑定到方法，参数与对应的事件的参数相同
        private void EventBindRegularArgs(object sender, RoutedEventArgs e)
        {
            CurrentEmployee.Name = "wanglei" + new Random().Next(1000, 10000).ToString();
        }

        // 事件绑定到方法，参数与对应的事件的参数相同，但是其中的事件参数为 object 类型
        private void EventBindBaseArgs(object sender, object e)
        {
            CurrentEmployee.Name = "wanglei" + new Random().Next(1000, 10000).ToString();
        }

        public Employee CurrentEmployee { get; set; } = new Employee() { Name = "wanglei", Age = 36, IsMale = true };

        public ObservableCollection<Employee> AllEmployees { get; set; } = TestData.GetEmployees(5);
    }
}
