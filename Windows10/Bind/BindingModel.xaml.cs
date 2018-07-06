/*
 * 演示如何通过 Binding 绑定对象
 * 
 * 
 * 如果需要数据源在属性值发生变化时对外通知，则需要实现 INotifyPropertyChanged 接口（为了简化实现，建议继承 Common/BindableBase.cs 这个类）
 *     PropertyChanged - 对象的属性的值发生变化时触发的事件
 */

using System;
using System.ComponentModel;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class BindingModel : Page
    {
        private Employee _employee;

        public BindingModel()
        {
            this.InitializeComponent();

            this.Loaded += BindingModel_Loaded;
        }

        void BindingModel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 创建一个需要绑定的实体对象（注：Employee 实现了 INotifyPropertyChanged 接口）
            _employee = new Employee();
            _employee.Name = "webabcd";
            _employee.Age = 33;
            _employee.IsMale = true;

            // Employee 对象的属性的值发生变化时触发的事件（源自 INotifyPropertyChanged 接口）
            _employee.PropertyChanged += _employee_PropertyChanged;

            // 指定数据上下文（绑定的数据源）
            root.DataContext = _employee;

            // 每 5 秒更新一次数据
            ThreadPoolTimer.CreatePeriodicTimer
            (
                (timer) =>
                {
                    var ignored = Dispatcher.RunAsync
                    (
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            Random random = new Random();
                            _employee.Age = random.Next(10, 100);
                            _employee.IsMale = random.Next() % 2 == 0 ? true : false;
                        }
                    );
                },
                TimeSpan.FromMilliseconds(5000)
            );
        }

        // 每次属性的值发生变化时，显示变化后的结果
        void _employee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            lblMsg.Text = "属性：“" + e.PropertyName + "”的值发生了变化";
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += string.Format("当前的值为：Name-{0}, Age-{1}, IsMale-{2}", _employee.Name, _employee.Age, _employee.IsMale);
        }
    }
}
