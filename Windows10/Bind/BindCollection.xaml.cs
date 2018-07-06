/*
 * 演示如何通过 x:Bind 绑定集合
 * 
 * 
 * 如果需要集合数据源在数据添加，删除，更新时对外通知，则需要实现 INotifyCollectionChanged 接口
 *     CollectionChanged - 集合数据在发生添加，删除，更新时触发的事件
 */

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class BindCollection : Page
    {
        // 数据源
        // ObservableCollection<T> 实现了 INotifyCollectionChanged 接口
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>(TestData.GetEmployees());

        public BindCollection()
        {
            this.InitializeComponent();

            this.Loaded += BindCollection_Loaded;
        }

        void BindCollection_Loaded(object sender, RoutedEventArgs e)
        {
            // 集合数据在发生添加，删除，更新时触发的事件（源自 INotifyCollectionChanged 接口）
            Employees.CollectionChanged += Employees_CollectionChanged;
        }

        void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*
             * e.Action - 引发此事件的操作类型（NotifyCollectionChangedAction 枚举）
             *     Add, Remove, Replace, Move, Reset
             * e.OldItems - Remove, Replace, Move 操作时影响的数据列表
             * e.OldStartingIndex - Remove, Replace, Move 操作发生处的索引
             * e.NewItems - 更改中所涉及的新的数据列表
             * e.NewStartingIndex - 更改中所涉及的新的数据列表的发生处的索引
             */
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // 此处的通知来自 INotifyCollectionChanged 接口
            Employees.RemoveAt(0);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            // 此处的通知来自实现了 INotifyPropertyChanged 接口的 Employee
            Employees.First().Name = random.Next(1000, 10000).ToString();

            // 此处的通知来自 INotifyCollectionChanged 接口
            Employees[1] = new Employee() { Name = random.Next(1000, 10000).ToString() };
        }
    }
}
