/*
 * 演示如何通过 x:Bind 绑定对象
 */

using System;
using System.ComponentModel;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class BindModel : Page
    {
        // Employee 实现了 INotifyPropertyChanged 接口
        public Employee CurrentEmployee { get; set; } = new Employee() { Name = "wanglei", Age = 36, IsMale = true };

        public BindModel()
        {
            this.InitializeComponent();

            this.Loaded += BindingModel_Loaded;
        }

        void BindingModel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 每 5 秒更新一次数据
            ThreadPoolTimer.CreatePeriodicTimer
            (
                (timer) =>
                {
                    var ignored = Dispatcher.RunAsync
                    (
                        CoreDispatcherPriority.High,
                        () =>
                        {
                            Random random = new Random();
                            CurrentEmployee.Age = random.Next(10, 100);
                            CurrentEmployee.IsMale = random.Next() % 2 == 0 ? true : false;
                        }
                    );
                },
                TimeSpan.FromMilliseconds(5000)
            );

            // Employee 对象的属性的值发生变化时触发的事件（源自 INotifyPropertyChanged 接口）
            CurrentEmployee.PropertyChanged += CurrentEmployee_PropertyChanged;
        }

        private void CurrentEmployee_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            lblMsg.Text = "属性：“" + e.PropertyName + "”的值发生了变化";
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += string.Format("当前的值为：Name-{0}, Age-{1}, IsMale-{2}", CurrentEmployee.Name, CurrentEmployee.Age, CurrentEmployee.IsMale);
        }
    }
}
