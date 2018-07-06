/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     
 * 
 * DragStartingEventArgs - DragStarting 的事件参数（CanDrag 的 UIElement 触发的事件）
 *     Cancel - 是否取消 drag 操作
 *     Data - 获取一个 DataPackage 类型的对象，用于保存数据（详见“分享”部分）
 *     DragUI - 获取一个 DragUI 类型的对象，用于设置 drag 过程中的 ui
 *     GetDeferral() - 获取类型为 DragOperationDeferral 的异步操作对象，同时开始异步操作，之后通过 DragOperationDeferral 的 Complete() 通知完成异步操作
 *     GetPosition(UIElement relativeTo) - 获取当前 drag 的点与指定 UIELement 的相对位置（传 null 则返回相对于 app 原点的位置）
 *     
 * DragUI - 用于设置 drag 过程中的 ui
 *     SetContentFromDataPackage() - 由系统根据 DataPackage 中保存的数据的类型来决定 ui
 *     SetContentFromBitmapImage() - 指定一个 BitmapImage 对象作为 ui，同时可以指定此 ui 相对于 drag 点的位置
 *     SetContentFromSoftwareBitmap() - 指定一个 SoftwareBitmap 对象作为 ui，同时可以指定此 ui 相对于 drag 点的位置
 * 
 * DropCompletedEventArgs - DropCompleted 的事件参数（CanDrag 的 UIElement 触发的事件）
 *     DropResult - 获取 drop 的结果，一个 DataPackageOperation 类型的枚举（None, Copy, Move, Link）
 *     
 *     
 * 本例用于演示 UIElement 的与 CanDrag 相关的事件（DragStartingEventArgs, DropCompletedEventArgs）
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class DragDropDemo3 : Page
    {
        public DragDropDemo3()
        {
            this.InitializeComponent();
        }

        private void dragGrid1_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock1.Text);
        }

        private void dragGrid2_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock2.Text);

            // 由系统根据 DataPackage 中保存的数据的类型来决定 drag 过程中的 ui
            args.DragUI.SetContentFromDataPackage();
        }

        private void dragGrid3_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock3.Text);

            // drag 过程中的 ui 为指定的 BitmapImage
            args.DragUI.SetContentFromBitmapImage(new BitmapImage(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute)));
        }

        private async void dragGrid4_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock4.Text);

            // 获取异步操作对象
            DragOperationDeferral deferral = args.GetDeferral();

            // 将 dragGrid4 截图，并以此创建一个 SoftwareBitmap 对象
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(dragGrid4);
            IBuffer buffer = await rtb.GetPixelsAsync();
            SoftwareBitmap bitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer, BitmapPixelFormat.Bgra8, rtb.PixelWidth, rtb.PixelHeight, BitmapAlphaMode.Premultiplied);

            // drag 过程中的 ui 为指定的 SoftwareBitmap
            args.DragUI.SetContentFromSoftwareBitmap(bitmap);

            // 完成异步操作
            deferral.Complete();
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
    }
}