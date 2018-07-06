/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     CanDrag - 此 UIElement 是否可以 drag
 *     DragStarting - 可以 drag 的 UIElement 开始 drag 时触发的事件
 *     DropCompleted - 可以 drag 的 UIElement 完成 drop 后触发的事件
 *     
 *     AllowDrop - 此 UIElement 是否可以 drop
 *     Drop -  可以 drop 的 UIElement 在 drop 操作发生时触发的事件
 *     DragEnter - drag 操作进入可以 drop 的 UIElement 时触发的事件
 *     DragOver - drag 操作在可以 drop 的 UIElement 上移动时触发的事件
 *     DragLeave - drag 操作离开可以 drop 的 UIElement 时触发的事件
 *
 *     
 * 注：关于 ListView 和 GridView 的 Item 的 drag & drop 请参见 /Controls/CollectionControl/ListViewBaseDemo/ListViewBaseDemo2.xaml
 *     
 *     
 * 本例用于演示 UIElement 的 drag & drop 的基本应用
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class DragDropDemo1 : Page
    {
        public DragDropDemo1()
        {
            this.InitializeComponent();
        }

        // dragGrid1 开始 drag 时触发的事件
        private void dragGrid1_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            lblMsg.Text += "dragGrid1_DragStarting";
            lblMsg.Text += Environment.NewLine;

            // 通过 DataPackage 保存文本数据（关于 DataPackage 的详细说明请参见“分享”部分）
            // 一个 DataPackage 对象可以包含多种类型的数据：ApplicationLink, WebLink, Bitmap, Html, Rtf, StorageItems, Text
            args.Data.SetText(sourceTextBlock.Text);
        }

        // dragGrid1 结束 drop 时触发的事件
        private void dragGrid1_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            lblMsg.Text += "dragGrid1_DropCompleted";
            lblMsg.Text += Environment.NewLine;
        }

        // dragGrid2 开始 drag 时触发的事件
        private void dragGrid2_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            lblMsg.Text += "dragGrid2_DragStarting";
            lblMsg.Text += Environment.NewLine;

            RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute));
            // 通过 DataPackage 保存图片数据
            args.Data.SetBitmap(imageStreamRef);
        }

        // dragGrid2 结束 drop 时触发的事件
        private void dragGrid2_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            lblMsg.Text += "dragGrid2_DropCompleted";
            lblMsg.Text += Environment.NewLine;
        }

        // 拖拽进入 dropGrid 时触发的事件
        private void dropGrid_DragEnter(object sender, DragEventArgs e)
        {
            lblMsg.Text += "dropGrid_DragEnter";
            lblMsg.Text += Environment.NewLine;

            // 指定拖拽操作的类型（None, Copy, Move, Link）
            e.AcceptedOperation = DataPackageOperation.None;

            // 根据 DataPackage 中的数据类型的不同做不同的处理（注：一个 DataPackage 中也可以同时包括各种不同类型的数据）
            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                e.DragUIOverride.Caption = "我是文本"; // 跟随 drag 点显示的文本
            }
            else if (e.DataView.Contains(StandardDataFormats.Bitmap))
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                e.DragUIOverride.Caption = "我是图片";
            }
            else if (e.DataView.Contains(StandardDataFormats.StorageItems)) // 当从 app 外部拖拽一个或多个文件进来时，系统会自动为 DataPackage 赋值
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                e.DragUIOverride.Caption = "我是文件";
            }
        }

        // 在 dropGrid 内拖拽移动时触发的事件
        private void dropGrid_DragOver(object sender, DragEventArgs e)
        {
            // lblMsg.Text += "dropGrid_DragOver";
            // lblMsg.Text += Environment.NewLine;
        }

        // 拖拽离开 dropGrid 时触发的事件
        private void dropGrid_DragLeave(object sender, DragEventArgs e)
        {
            lblMsg.Text += "dropGrid_DragLeave";
            lblMsg.Text += Environment.NewLine;
        }

        // 在 dropGrid 内 drop 后触发的事件
        private async void dropGrid_Drop(object sender, DragEventArgs e)
        {
            lblMsg.Text += "dropGrid_Drop";
            lblMsg.Text += Environment.NewLine;

            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                // 获取 DataPackage 中的文本数据
                string text = await e.DataView.GetTextAsync();
                targetTextBlock.Text += text;
                targetTextBlock.Text += Environment.NewLine;
            }
            else if (e.DataView.Contains(StandardDataFormats.Bitmap))
            {
                // 获取 DataPackage 中的图片数据
                RandomAccessStreamReference imageStreamRef = await e.DataView.GetBitmapAsync();
                IRandomAccessStream imageStream = await imageStreamRef.OpenReadAsync();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(imageStream);
                targetImage.Source = bitmapImage;
            }
            else if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                // 获取 DataPackage 中的文件数据（当从 app 外部拖拽一个或多个文件进来时，系统会自动为 DataPackage 赋值）
                IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();
                foreach (var storageFile in items.OfType<StorageFile>())
                {
                    if (storageFile != null)
                    {
                        targetTextBlock.Text += storageFile.Path;
                        targetTextBlock.Text += Environment.NewLine;
                    }
                }
            }
        }
    }
}
