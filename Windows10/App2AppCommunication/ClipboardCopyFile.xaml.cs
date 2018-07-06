/*
 * 演示如何复制指定的文件到剪切板，以及如何从剪切板中获取文件并保存到指定的路径 
 */

using System;
using System.Linq;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.App2AppCommunication
{
    public sealed partial class ClipboardCopyFile : Page
    {
        public ClipboardCopyFile()
        {
            this.InitializeComponent();
        }

        // 保存文件到剪切板
        private async void btnCopyFile_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdTest\clipboard.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, "I am webabcd: " + DateTime.Now.ToString());

            DataPackage dataPackage = new DataPackage();
            dataPackage.SetStorageItems(new List<StorageFile>() { file });

            dataPackage.RequestedOperation = DataPackageOperation.Move;
            try
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                lblMsg.Text = "已将文件复制到剪切板";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 从剪切板中获取文件并保存到指定的路径 
        private async void btnPasteFile_Click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.StorageItems))
            {
                try
                {
                    IReadOnlyList<IStorageItem> storageItems = await dataPackageView.GetStorageItemsAsync();
                    StorageFile file = storageItems.First() as StorageFile;
                    if (file != null)
                    {
                        StorageFile newFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
                        if (newFile != null)
                        {
                            lblMsg.Text = string.Format("已将文件从{0}复制到{1}", file.Path, newFile.Path);
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
            else
            {
                lblMsg.Text = "剪切板中无 StorageItems 内容";
            }
        }
    }
}
