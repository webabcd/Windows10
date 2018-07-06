/*
 * 演示如何通过 Binding 绑定集合
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
    public sealed partial class BindingCollection : Page
    {
        // ObservableCollection<T> 实现了 INotifyCollectionChanged 接口
        private ObservableCollection<Employee> _employees;

        public BindingCollection()
        {
            this.InitializeComponent();

            this.Loaded += BindingCollection_Loaded;
        }

        void BindingCollection_Loaded(object sender, RoutedEventArgs e)
        {
            _employees = new ObservableCollection<Employee>(TestData.GetEmployees());

            // 集合数据在发生添加，删除，更新时触发的事件（源自 INotifyCollectionChanged 接口）
            _employees.CollectionChanged += _employees_CollectionChanged;

            // 指定 ListView 的数据源
            listView.ItemsSource = _employees;
        }

        void _employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
            _employees.RemoveAt(0);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            // 此处的通知来自实现了 INotifyPropertyChanged 接口的 Employee
            _employees.First().Name = random.Next(1000, 10000).ToString();

            // 此处的通知来自 INotifyCollectionChanged 接口
            _employees[1] = new Employee() { Name = random.Next(1000, 10000).ToString() };
        }
    }
}
