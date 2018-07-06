/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     StartDragAsync(PointerPoint pointerPoint) - 将 UIElement 拖拽到指定的 PointerPoint 位置，返回一个 DataPackageOperation 类型的枚举（None, Copy, Move, Link）
 * 
 * 
 * CanDrag - 由系统决定何时开启拖放操作，一般就是鼠标按下后进行拖拽
 * StartDragAsync() - 由开发者手动决定何时何地开启拖放操作
 *     
 *     
 * 本例用于演示如何手动开启 UIElement 的拖放操作
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class DragDropDemo2 : Page
    {
        public DragDropDemo2()
        {
            this.InitializeComponent();
        }

        private void dragGrid_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock.Text);
        }

        private void dropGrid_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "我是文本";
        }

        private async void dropGrid_Drop(object sender, DragEventArgs e)
        {
            string text = await e.DataView.GetTextAsync();
            targetTextBlock.Text += text;
            targetTextBlock.Text += Environment.NewLine;
        }

        private async void dragGrid_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // 通过 StartDragAsync() 开启拖放操作，拖放操作的其他部分遵循相同的模式
            DataPackageOperation dpo = await dragGrid.StartDragAsync(e.GetCurrentPoint(dragGrid));
            if (dpo != DataPackageOperation.None)
            {
                targetTextBlock.Text += dpo;
                targetTextBlock.Text += Environment.NewLine;
            }
        }
    }
}
