/*
 * ListViewBase(基类) - 列表控件基类（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 *     IncrementalLoadingTrigger - 增量加载的触发器
 *         Edge - 允许触发增量加载，默认值
 *         None - 禁止触发增量加载
 *     DataFetchSize - 预提数据的大小，默认值 3.0
 *         本例将此值设置为 4.0 ，其效果为（注：本例中的 ListView 每页可显示的数据量为 6 条或 7 条，以下计算需基于此）
 *         1、先获取 1 条数据，为的是尽量快地显示数据
 *         2、再获取 4.0 * 1 条数据
 *         3、再获取 4.0 * (6 或 7，如果 ListView 当前显示了 6 条数据则为 6，如果 ListView 当前显示了 7 条数据则为 7) 条数据
 *         4、以后每次到达阈值后，均增量加载 4.0 * (6 或 7，如果 ListView 当前显示了 6 条数据则为 6，如果 ListView 当前显示了 7 条数据则为 7) 条数据
 *     IncrementalLoadingThreshold - 增量加载的阈值，默认值 0.0
 *         本例将此值设置为 2.0 ，其效果为（注：本例中的 ListView 每页可显示的数据量为 6 条或 7 条）
 *         1、滚动中，如果已准备好的数据少于 2.0 * (6 或 7，如果 ListView 当前显示了 6 条数据则为 6，如果 ListView 当前显示了 7 条数据则为 7) 条数据，则开始增量加载  
 *         
 *         
 * 本例用于演示如何实现 ListViewBase 的增量加载（数据源需要实现 ISupportIncrementalLoading 接口，详见：MyIncrementalLoading.cs）
 */

using Windows.UI.Xaml.Controls;
using System.Linq;
using System.Collections.Specialized;
using System;
using Windows10.Common;
using Windows.UI.Xaml;

namespace Windows10.Controls.CollectionControl.ListViewBaseDemo
{
    public sealed partial class ListViewBaseDemo3 : Page
    {
        // 实现了增量加载的数据源
        private MyIncrementalLoading<Employee> _employees;

        public ListViewBaseDemo3()
        {
            this.InitializeComponent();

            this.Loaded += ListViewBaseDemo3_Loaded;
        }

        private void ListViewBaseDemo3_Loaded(object sender, RoutedEventArgs e)
        {
            listView.IncrementalLoadingTrigger = IncrementalLoadingTrigger.Edge;
            listView.DataFetchSize = 4.0;
            listView.IncrementalLoadingThreshold = 2.0;

            _employees = new MyIncrementalLoading<Employee>(1000, (startIndex, count) =>
            {
                lblLog.Text += string.Format("从索引 {0} 处开始获取 {1} 条数据", startIndex, count);
                lblLog.Text += Environment.NewLine;

                return TestData.GetEmployees().Skip(startIndex).Take(count).ToList();
            });

            _employees.CollectionChanged += _employees_CollectionChanged;

            listView.ItemsSource = _employees;
        }

        void _employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lblMsg.Text = "已获取的数据量：" + _employees.Count.ToString();
        }
    }
}