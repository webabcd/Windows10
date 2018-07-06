/*
 * 演示如何在可移动存储中对文件进行操作
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem.RemovableDevice
{
    public sealed partial class Demo : Page
    {
        public Demo()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取全部可移动存储
            StorageFolder removableDevices = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.RemovableDevices);
            IReadOnlyList<StorageFolder> folderList = await removableDevices.GetFoldersAsync();
            listBox.ItemsSource = folderList.Select(p => p.Name).ToList();

            base.OnNavigatedTo(e);
        }

        private async void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 用户选中的可移动存储
            string folderName = (string)listBox.SelectedItem;
            StorageFolder removableDevices = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.RemovableDevices);
            StorageFolder storageFolder = await removableDevices.GetFolderAsync(folderName);

            // 用户选中的可移动存储中的根目录下的文件和文件夹总数
            IReadOnlyList<IStorageItem> storageItems = await storageFolder.GetItemsAsync();
            lblMsg.Text = "items count: " + storageItems.Count;
        }
        
        private async void btnCopyFile_Click(object sender, RoutedEventArgs e)
        {
            // 用户选中的可移动存储
            string folderName = (string)listBox.SelectedItem;

            if (folderName != null)
            {
                StorageFolder removableDevices = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.RemovableDevices);
                StorageFolder storageFolder = await removableDevices.GetFolderAsync(folderName);

                // 复制包内文件到指定的可移动存储
                StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/hololens.jpg"));
                try
                {
                    await storageFile.CopyAsync(storageFolder, "hololens.jpg", NameCollisionOption.ReplaceExisting);
                    lblMsg.Text = "复制成功";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }
    }
}
