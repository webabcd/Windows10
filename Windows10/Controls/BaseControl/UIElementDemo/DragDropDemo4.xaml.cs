/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     
 * 
 * DragEventArgs - Drop, DragEnter, DragOver, DragLeave 的事件参数（AllowDrop 的 UIElement 触发的事件）
 *     AcceptedOperation - 一个 DataPackageOperation 类型的枚举，用于指定操作的类型
 *         None - 无操作
 *         Copy - 复制操作
 *         Move - 移动操作
 *         Link - 链接操作
 *     DataView - 获取一个 DataPackageView 类型的对象，用于获取 DataPackage 中保存的数据（详见“分享”部分）
 *     DragUIOverride - 获取一个 DragUIOverride 类型的对象，用于设置 drag 过程中的 ui（在 drop 区域内）。如果此时和 drag 过程中的 DragUI 有冲突的话，则以此 DragUIOverride 为准
 *     Handled - 是否标记为已处理
 *     Modifiers - 获取一个 DragDropModifiers 类型的枚举（FlagsAttribute），用于获取当前的按键状态
 *         None, Shift, Control, Alt, LeftButton, MiddleButton, RightButton
 *     GetDeferral() - 获取类型为 DragOperationDeferral 的异步操作对象，同时开始异步操作，之后通过 DragOperationDeferral 的 Complete() 通知完成异步操作
 *     GetPosition(UIElement relativeTo) - 获取当前 drag 的点与指定 UIELement 的相对位置（传 null 则返回相对于 app 原点的位置）
 *      
 * DragUIOverride - 用于设置 drag 过程中的 ui（在 drop 区域内）。它包括 3 个部分，分别是 Caption, Glyph, Content
 *     Caption - 标题
 *     IsCaptionVisible - 是否显示标题
 *     IsGlyphVisible - 是否显示标题的左边的那个图标（这个图标会根据你的 DataPackageOperation 的不同而不同）
 *     IsContentVisible - 是否显示内容（就是除了 Caption 和 Glyph 之外的内容）
 *     Clear() - 清除 drag 过程中的 ui（但是实际测试发现并不能清除，如果需要的话还是分别设置 IsCaptionVisible, IsGlyphVisible, IsContentVisible 吧）
 *     SetContentFromBitmapImage() - 指定一个 BitmapImage 对象作为 ui，同时可以指定此 ui 相对于 drag 点的位置
 *     SetContentFromSoftwareBitmap() - 指定一个 SoftwareBitmap 对象作为 ui，同时可以指定此 ui 相对于 drag 点的位置
 *     
 *      
 * 本例用于演示 UIElement 的与 AllowDrop 相关的事件（DragEventArgs）
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
    public sealed partial class DragDropDemo4 : Page
    {
        public DragDropDemo4()
        {
            this.InitializeComponent();
        }

        private void dragGrid_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Data.SetText(sourceTextBlock.Text);
        }

        private void dropGrid1_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "我是文本";

            targetTextBlock1.Text += e.Modifiers;
            targetTextBlock1.Text += Environment.NewLine;
        }

        private void dropGrid2_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "我是文本";

            // DragUIOverride 包含 3 个部分，分别是 Caption, Glyph, Content
            e.DragUIOverride.IsCaptionVisible = false;
            e.DragUIOverride.IsGlyphVisible = false;
            e.DragUIOverride.IsContentVisible = false;

            targetTextBlock2.Text += e.Modifiers;
            targetTextBlock2.Text += Environment.NewLine;
        }

        private void dropGrid3_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "我是文本";

            // drag 到 drop 区域后，drag 过程中的 ui 改为指定的 BitmapImage
            e.DragUIOverride.SetContentFromBitmapImage(new BitmapImage(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute)));

            targetTextBlock3.Text += e.Modifiers;
            targetTextBlock3.Text += Environment.NewLine;
        }

        private async void dropGrid4_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "我是文本";

            // 获取异步操作对象
            DragOperationDeferral deferral = e.GetDeferral();
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(dragGrid);
            IBuffer buffer = await rtb.GetPixelsAsync();
            SoftwareBitmap bitmap = SoftwareBitmap.CreateCopyFromBuffer(buffer, BitmapPixelFormat.Bgra8, rtb.PixelWidth, rtb.PixelHeight, BitmapAlphaMode.Premultiplied);

            // drag 到 drop 区域后，drag 过程中的 ui 改为指定的 SoftwareBitmap
            e.DragUIOverride.SetContentFromSoftwareBitmap(bitmap);

            // 完成异步操作
            deferral.Complete();

            targetTextBlock4.Text += e.Modifiers;
            targetTextBlock4.Text += Environment.NewLine;
        }

        private async void dropGrid1_Drop(object sender, DragEventArgs e)
        {
            string text = await e.DataView.GetTextAsync();
            targetTextBlock1.Text += text;
            targetTextBlock1.Text += Environment.NewLine;
        }

        private async void dropGrid2_Drop(object sender, DragEventArgs e)
        {
            string text = await e.DataView.GetTextAsync();
            targetTextBlock2.Text += text;
            targetTextBlock2.Text += Environment.NewLine;
        }

        private async void dropGrid3_Drop(object sender, DragEventArgs e)
        {
            string text = await e.DataView.GetTextAsync();
            targetTextBlock3.Text += text;
            targetTextBlock3.Text += Environment.NewLine;
        }

        private async void dropGrid4_Drop(object sender, DragEventArgs e)
        {
            string text = await e.DataView.GetTextAsync();
            targetTextBlock4.Text += text;
            targetTextBlock4.Text += Environment.NewLine;
        }
    }
}
