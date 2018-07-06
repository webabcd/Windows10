/*
 * Pivot - Pivot 控件（继承自 ItemsControl, 请参见 /Controls/CollectionControl/ItemsControlDemo/）
 *     IsLocked - 是否锁定 Pivot，锁定后只会显示当前选中的 item，而不能切换
 *     SelectedItem - 当前选中的 item
 *     SelectedIndex - 当前选中的 item 的索引位置
 *     
 * PivotItem - PivotItem 控件（继承自 ContentControl, 请参见 /Controls/BaseControl/ContentControlDemo/）
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.CollectionControl
{
    public sealed partial class PivotDemo : Page
    {
        public PivotDemo()
        {
            this.InitializeComponent();
        }

        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            pivot.IsLocked ^= true; 
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // e.RemovedItems - 本次事件中，被取消选中的项
            // e.AddedItems - 本次事件中，新被选中的项

            lblMsg1.Text = "SelectionChangedEventArgs.AddedItems: " + (e.AddedItems[0] as PivotItem).Header.ToString();
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "Pivot.SelectedIndex: " + pivot.SelectedIndex;
            lblMsg1.Text += Environment.NewLine;
            lblMsg1.Text += "Pivot.SelectedItem: " + (pivot.SelectedItem as PivotItem).Header.ToString();
        }

        // 某 PivotItem 准备变成选中项
        private void pivot_PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            // args.Item - 相关的 PivotItem 对象

            lblMsg2.Text += "pivot_PivotItemLoading: " + args.Item.Header.ToString();
            lblMsg2.Text += Environment.NewLine;
        }

        // 某 PivotItem 已经变成选中项
        private void pivot_PivotItemLoaded(Pivot sender, PivotItemEventArgs args)
        {
            lblMsg2.Text += "pivot_PivotItemLoaded: " + args.Item.Header.ToString();
            lblMsg2.Text += Environment.NewLine;
        }

        // 某 PivotItem 准备从选中项变为非选中项
        private void pivot_PivotItemUnloading(Pivot sender, PivotItemEventArgs args)
        {
            lblMsg2.Text += "pivot_PivotItemUnloading: " + args.Item.Header.ToString();
            lblMsg2.Text += Environment.NewLine;
        }

        // 某 PivotItem 已经从选中项变为非选中项
        private void pivot_PivotItemUnloaded(Pivot sender, PivotItemEventArgs args)
        {
            lblMsg2.Text += "pivot_PivotItemUnloaded: " + args.Item.Header.ToString();
            lblMsg2.Text += Environment.NewLine;
        }
    }
}
