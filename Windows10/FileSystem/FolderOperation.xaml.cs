/*
 * 演示如何创建文件夹，重命名文件夹，删除文件夹，在指定的文件夹中创建文件
 * 
 * StorageFolder - 文件夹操作类
 *     public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName);
 *     public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options);
 *     public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName);
 *     public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options);
 *     public IAsyncAction RenameAsync(string desiredName);
 *     public IAsyncAction RenameAsync(string desiredName, NameCollisionOption option);
 *     public IAsyncAction DeleteAsync();
 *     public IAsyncAction DeleteAsync(StorageDeleteOption option);
 * 
 * CreationCollisionOption
 *     GenerateUniqueName - 存在则在名称后自动追加编号
 *     ReplaceExisting - 存在则替换
 *     FailIfExists - 存在则抛出异常
 *     OpenIfExists - 存在则返回现有项
 * 
 * NameCollisionOption
 *     GenerateUniqueName - 存在则在名称后自动追加编号
 *     ReplaceExisting - 存在则替换
 *     FailIfExists - 存在则抛出异常
 * 
 * StorageDeleteOption
 *     Default - 默认行为
 *     PermanentDelete - 永久删除（不会移至回收站）
 *     
 *     
 * 注：以上接口不再一一说明，看看下面的示例代码就基本都明白了
 */

using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class FolderOperation : Page
    {
        private StorageFolder _myFolder = null;

        public FolderOperation()
        {
            this.InitializeComponent();
        }

        // 创建文件夹
        private async void btnCreateFolder_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            _myFolder = await picturesFolder.CreateFolderAsync("MyFolder", CreationCollisionOption.OpenIfExists);

            // 创建文件夹时也可以按照下面这种方式创建多级文件夹 
            // _myFolder = await picturesFolder.CreateFolderAsync(@"MyFolder\sub\subsub", CreationCollisionOption.OpenIfExists);

            lblMsg.Text = "创建了文件夹";
        }

        // 重命名文件夹
        private async void btnRenameFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                await _myFolder.RenameAsync("MyFolder_Rename", NameCollisionOption.FailIfExists);
                lblMsg.Text = "重命名了文件夹";
            }
        }

        // 删除文件夹
        private async void btnDeleteFolder_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                await _myFolder.DeleteAsync(StorageDeleteOption.Default);
                lblMsg.Text = "删除了文件夹";

                _myFolder = null;
            }
        }

        // 在指定的文件夹中创建文件
        private async void btnCreateFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                StorageFile myFile = await _myFolder.CreateFileAsync("MyFile", CreationCollisionOption.OpenIfExists);

                // 创建文件时也可以按照下面这种方式指定子目录（目录不存在的话会自动创建）
                // StorageFile myFile = await _myFolder.CreateFileAsync(@"folder1\folder2\MyFile", CreationCollisionOption.OpenIfExists);

                lblMsg.Text = "在指定的文件夹中创建了文件";
            }
        }
    }
}
