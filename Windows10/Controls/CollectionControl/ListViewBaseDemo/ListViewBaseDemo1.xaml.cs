/*
 * ListViewBase(基类) - 列表控件基类（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 *     SelectedItems - 被选中的 items 集合（只读）
 *     SelectedRanges - 被选中的 items 的范围（只读）
 *     SelectAll() - 选中全部 items
 *     SelectRange(ItemIndexRange itemIndexRange) - 选中指定范围的 items
 *     DeselectRange(ItemIndexRange itemIndexRange) - 取消选中指定范围的 items
 *     ScrollIntoView(object item, ScrollIntoViewAlignment alignment) - 滚动到指定的 item
 *         ScrollIntoViewAlignment.Default - 不好解释，请自己看演示效果
 *         ScrollIntoViewAlignment.Leading - 不好解释，请自己看演示效果
 * 
 * ItemIndexRange - items 的范围
 *     FirstIndex - 范围的第一个 item 的索引位置
 *     LastIndex - 范围的最后一个 item 的索引位置
 *     Length - 范围的长度
 *     
 * 
 * 注：
 * ListViewBase 实现了 ISemanticZoomInformation 接口，所以可以在 SemanticZoom 的两个视图间有关联地切换。关于 ISemanticZoomInformation 请参见 /Controls/CollectionControl/SemanticZoomDemo/ISemanticZoomInformationDemo.xaml
 * 
 * 
 * 本例用于演示 ListViewBase 的基础知识
 */

using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using System.Linq;
using Windows10.Common;
using Windows.UI.Xaml;
using Windows.UI.Popups;

namespace Windows10.Controls.CollectionControl.ListViewBaseDemo
{
    public sealed partial class ListViewBaseDemo1 : Page
    {
        public ObservableCollection<Employee> Data { get; set; } = TestData.GetEmployees(10000);

        public ListViewBaseDemo1()
        {
            this.InitializeComponent();
        }

        // 单击行为的事件
        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 获取被单击的 item 的数据
            lblMsg1.Text = "被单击的 employee 的 name 为：" + (e.ClickedItem as Employee).Name;
        }

        // 选中行为的事件
        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // e.RemovedItems - 本次事件中，被取消选中的项
            // e.AddedItems - 本次事件中，新被选中的项

            lblMsg2.Text = $"新被选中的 item 共 {e.AddedItems.Count.ToString()} 条, 新被取消选中的 item 共 {e.RemovedItems.Count.ToString()} 条";
        }

        private void cmbSelectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.SelectionMode = (ListViewSelectionMode)Enum.Parse(typeof(ListViewSelectionMode), (e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }

        private void buttonScrollDefault_Click(object sender, RoutedEventArgs e)
        {
             listView.ScrollIntoView(Data.Skip(100).First(), ScrollIntoViewAlignment.Default);
        }

        private void buttonScrollLeading_Click(object sender, RoutedEventArgs e)
        {
            listView.ScrollIntoView(Data.Skip(100).First(), ScrollIntoViewAlignment.Leading);
        }

        private async void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectionMode == ListViewSelectionMode.Multiple || listView.SelectionMode == ListViewSelectionMode.Extended)
            {
                // 选中第 3, 4, 5, 6 项
                ItemIndexRange iir = new ItemIndexRange(2, 4);
                listView.SelectRange(iir);
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("SelectionMode 必须是 Multiple 或 Extended", "提示");
                await messageDialog.ShowAsync();
            }
        }
    }
}