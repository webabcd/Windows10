/*
 * ListViewBase(基类) - 列表控件基类（继承自 Selector, 请参见 /Controls/SelectionControl/SelectorDemo.xaml）
 * 
 *      
 * DragItemsStartingEventArgs
 *     Items - 被拖动的 items 集合
 *     Cancel - 是否取消拖动操作
 *     Data - 一个 DataPackage 类型的对象，用于传递数据
 *     
 * DragItemsCompletedEventArgs
 *     DropResult - drop 的结果（None, Copy, Move, Link）
 *     Items - 被拖动的 items 集合
 *     
 * 
 * 注：
 * 1、drag-drop 传递数据，剪切板传递数据，分享传递数据，以及其他场景的数据传递均通过 DataPackage 类型的对象来完成
 * 2、本例通过一个私有字段传递数据，通过 DataPackage 传递数据请参见：/Controls/BaseControl/UIElementDemo/DragDropDemo.xaml
 * 3、关于 UIElement 拖放的详细说明请参见：/Controls/BaseControl/UIElementDemo/DragDropDemo.xaml
 * 
 * 
 * 本例用于演示如何在 ListViewBase 内拖动 item 以对 item 排序，以及如何拖动 item 到 ListViewBase 外的指定位置以删除 item，以及如何拖动一个 UIElement 到 ListViewBase 内以添加这个 item
 */

using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows10.Common;
using Windows.ApplicationModel.DataTransfer;
using System.Collections.Specialized;
using System;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.CollectionControl.ListViewBaseDemo
{
    public sealed partial class ListViewBaseDemo2 : Page
    {
        // gridView1 的数据源
        public ObservableCollection<Employee> Data1 { get; set; } = new ObservableCollection<Employee>(TestData.GetEmployees());
        // gridView2 的数据源
        public ObservableCollection<Employee> Data2 { get; set; } = new ObservableCollection<Employee>();

        // lblEmployee 的数据源
        public Employee Employee { get; set; } = new Employee() { Name = "wanglei", Age = 36, IsMale = true };

        // 拖动中的 Employee 对象
        private Employee _draggingEmployee;

        public ListViewBaseDemo2()
        {
            this.InitializeComponent();

            // 这个用来证明在 gridView1 中拖动 item 排序时，其结果会同步到数据源
            Data1.CollectionChanged += Data1_CollectionChanged;
        }

        private void Data1_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += $"Action: {e.Action}, OldStartingIndex: {e.OldStartingIndex}, NewStartingIndex: {e.NewStartingIndex}";
        }

        // 开始 item 拖动
        private void gridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            _draggingEmployee = e.Items.First() as Employee;
        }

        // 完成 item 拖动
        private void gridView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += $"DropResult: {args.DropResult}";

            _draggingEmployee = null;
        }

        // item 被拖进了 borderDelete
        private void borderDelete_DragEnter(object sender, DragEventArgs e)
        {
            // 关于 DragEventArgs 的详细介绍，以及其他属于 UIElement 拖放方面的详细介绍请参见：/Controls/BaseControl/UIElementDemo/DragDropDemo.xaml

            e.AcceptedOperation = DataPackageOperation.Move;

            e.DragUIOverride.IsGlyphVisible = false;
            e.DragUIOverride.Caption = "松开则删除";

            Debug.WriteLine("DragEnter");
        }

        // item 被 drop 到了 borderDelete
        private void borderDelete_Drop(object sender, DragEventArgs e)
        {
            // 从数据源中删除指定的 Employee 对象
            Data1.Remove(_draggingEmployee);
            Data2.Remove(_draggingEmployee);

            _draggingEmployee = null;

            // 在 borderDelete 放下了拖动项
            Debug.WriteLine("Drop");
        }

        // item 被拖出了 borderDelete
        private void borderDelete_DragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine("DragLeave");
        }

        // item 在 borderDelete 上面拖动着
        private void borderDelete_DragOver(object sender, DragEventArgs e)
        {
            Debug.WriteLine("DragOver");
        }


        // item 被拖进了 gridView2
        private void gridView2_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }

        // item 被 drop 到了 gridView2
        private void gridView2_Drop(object sender, DragEventArgs e)
        {
            Data2.Add(_draggingEmployee);
        }


        // lblEmployee 被按下了
        private async void lblEmployee_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // 启动 lblEmployee 的拖动操作
            await lblEmployee.StartDragAsync(e.GetCurrentPoint(lblEmployee));
        }

        // lblEmployee 开始被拖动
        private void lblEmployee_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.RequestedOperation = DataPackageOperation.Copy;

            _draggingEmployee = Employee;
        }


        // 通过 api 将 gridView1 中的第 1 项数据移动到第 3 项数据的位置
        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            // 利用数据源 ObservableCollection 的 Move 方法
            Data1.Move(0, 2);
        }
    }
}
