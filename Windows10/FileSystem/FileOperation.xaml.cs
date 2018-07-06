/*
 * 演示如何创建文件，复制文件，移动文件，重命名文件，删除文件
 * 
 * StorageFile - 文件操作类
 *     public IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder);
 *     public IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName);
 *     public IAsyncOperation<StorageFile> CopyAsync(IStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option);
 *     public IAsyncAction CopyAndReplaceAsync(IStorageFile fileToReplace);
 *     public IAsyncAction MoveAsync(IStorageFolder destinationFolder);
 *     public IAsyncAction MoveAsync(IStorageFolder destinationFolder, string desiredNewName);
 *     public IAsyncAction MoveAsync(IStorageFolder destinationFolder, string desiredNewName, NameCollisionOption option);
 *     public IAsyncAction MoveAndReplaceAsync(IStorageFile fileToReplace);
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
    public sealed partial class FileOperation : Page
    {
        private StorageFolder _myFolder = null;

        public FileOperation()
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

        // 复制文件
        private async void btnCopyFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                try
                {
                    StorageFile myFile = await _myFolder.GetFileAsync("MyFile");
                    StorageFile myFile_copy = await myFile.CopyAsync(_myFolder, "MyFile_Copy", NameCollisionOption.ReplaceExisting);
                    lblMsg.Text = "复制了文件";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        // 移动文件
        private async void btnMoveFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                try
                {
                    StorageFile myFile = await _myFolder.GetFileAsync("MyFile");
                    await myFile.MoveAsync(_myFolder, "MyFile_Move", NameCollisionOption.ReplaceExisting);
                    lblMsg.Text = "移动了文件";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        // 重命名文件
        private async void btnRenameFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                try
                {
                    StorageFile myFile = await _myFolder.GetFileAsync("MyFile_Move");
                    await myFile.RenameAsync("MyFile_Rename", NameCollisionOption.ReplaceExisting);
                    lblMsg.Text = "重命名了文件";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        // 删除文件
        private async void btnDeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (_myFolder != null)
            {
                try
                {
                    StorageFile myFile = await _myFolder.GetFileAsync("MyFile_Rename");
                    await myFile.DeleteAsync(StorageDeleteOption.Default);
                    lblMsg.Text = "删除了文件";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }
    }
}
