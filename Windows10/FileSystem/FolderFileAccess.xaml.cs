/*
 * 演示如何获取文件夹和文件
 * 
 * KnownFolders - 已知文件夹
 *     GetFolderForUserAsync(User user, KnownFolderId folderId) - 获取指定用户的指定文件夹的 StorageFolder 对象
 *         user - 指定用户，传 null 则为当前用户（关于 User 相关请参见 /UserAndAccount/UserInfo.xaml）
 *         folderId - 指定文件夹，一个 KnownFolderId 类型的枚举值，常用的有 RemovableDevices, DocumentsLibrary, PicturesLibrary, VideosLibrary, MusicLibrary 等，其他更多的请参见文档
 * 
 * StorageFolder - 文件夹操作类
 *     GetFileAsync(string name) - 在当前 StorageFolder 下获取指定名字的 StorageFile
 *         不存在的话会抛出 FileNotFoundException 异常
 *     GetFolderAsync(string name) - 在当前 StorageFolder 下获取指定名字的 StorageFolder
 *         不存在的话会抛出 FileNotFoundException 异常
 *     GetItemAsync(string name) - 在当前 StorageFolder 下获取指定名字的 IStorageItem
 *         不存在的话会抛出 FileNotFoundException 异常
 *     GetFilesAsync() - 获取当前 StorageFolder 下的 StorageFile 集合
 *     GetFoldersAsync() = 获取当前 StorageFolder 下的 StorageFolder 集合
 *     GetItemsAsync() - 获取当前 StorageFolder 下的 IStorageItem 集合
 *     IsOfType(StorageItemTypes type) - 判断当前的 IStorageItem 是 StorageItemTypes.File 还是 StorageItemTypes.Folder
 *     GetParentAsync() - 获取当前 StorageFolder 的父 StorageFolder，找不到就返回 null
 *     IsEqual(IStorageItem item) - 判断两个 StorageFolder 是否相等
 *     TryGetItemAsync(string name) - 在当前 StorageFolder 下获取指定名字的 IStorageItem
 *         不存在的话会也不会抛出 FileNotFoundException 异常，而是会返回 null
 *     GetFolderFromPathAsync(string path) - 静态方法，用于获取指定路径的 StorageFolder 对象（没有权限的话会抛出异常）
 *     GetIndexedStateAsync() - 返回当前文件夹的被系统索引的状态（一个 IndexedState 类型的枚举）
 *     
 * StorageFile - 文件操作类
 *     IsOfType(StorageItemTypes type) - 判断当前的 IStorageItem 是 StorageItemTypes.File 还是 StorageItemTypes.Folder
 *     GetParentAsync() - 获取当前 StorageFile 的父 StorageFolder，找不到就返回 null
 *     IsEqual(IStorageItem item) - 判断两个 StorageFile 是否相等
 *     
 *     
 * 注：
 * 1、如果想要获取任意路径的 StorageFolder 或 StorageFile 的话，可以通过 Picker 让用户选择
 * 2、对于处理文件夹和文件来说，最好都放到 try catch 中，因为不定会有什么异常呢
 * 3、StorageFile 和 StorageFolder 有很多共同的接口（File 代表文件，Folder 代表文件夹，Item 代表文件和文件夹），详见文档
 * 4、对于处理 KnownFolders 已知文件夹来说
 *    需要在 Package.appxmanifest 中配置 <Capability Name="removableStorage" />, <Capability Name="picturesLibrary" />, <Capability Name="videosLibrary" />,  <Capability Name="musicLibrary" />, <Capability Name="documentsLibrary" />
 * 5、对于处理 KnownFolderId.DocumentsLibrary 中的文件来说
 *    需要在 Package.appxmanifest 对相关类型的文件配置好文件关联
 */

using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class FolderFileAccess : Page
    {
        public FolderFileAccess()
        {
            this.InitializeComponent();
        }

        // 获取文件夹
        private async void btnGetFolder_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "";

            // 获取当前用户的“图片库”的 StorageFolder 对象
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);

            // 获取“图片库”所包含的全部文件夹
            IReadOnlyList<StorageFolder> folderList = await picturesFolder.GetFoldersAsync();
            foreach (StorageFolder storageFolder in folderList)
            {
                lblMsg.Text += "    " + storageFolder.Name;
                lblMsg.Text += Environment.NewLine;
            }


            // 在当前 StorageFolder 下获取指定名字的 StorageFolder，不存在的话会抛出 FileNotFoundException 异常
            try
            {
                await picturesFolder.GetFolderAsync("aabbcc");
            }
            catch (FileNotFoundException)
            {
                await new MessageDialog("在“图片库”中找不到名为“aabbcc”的文件夹").ShowAsync();
            }

            // 在当前 StorageFolder 下获取指定名字的 IStorageItem，不存在的话会也不会抛出 FileNotFoundException 异常，而是会返回 null
            IStorageItem item = await picturesFolder.TryGetItemAsync("aabbcc");
            if (item == null)
            {
                await new MessageDialog("在“图片库”中找不到名为“aabbcc”的文件夹或文件").ShowAsync();
            }
        }

        // 获取文件夹和文件
        private async void btnGetFolderFile_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "";

            // 获取当前用户的“图片库”的 StorageFolder 对象
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);

            // 获取“图片库”所包含的全部文件夹和文件
            IReadOnlyList<IStorageItem> storageItems = await picturesFolder.GetItemsAsync();
            foreach (IStorageItem storageItem in storageItems)
            {
                if (storageItem.IsOfType(StorageItemTypes.Folder)) // 是文件夹
                {
                    StorageFolder storageFolder = storageItem as StorageFolder;
                    lblMsg.Text += "folder: " + storageFolder.Name;
                    lblMsg.Text += Environment.NewLine;
                }
                else if (storageItem.IsOfType(StorageItemTypes.File)) // 是文件
                {
                    StorageFile storageFile = storageItem as StorageFile;
                    lblMsg.Text += "file: " + storageFile.Name;
                    lblMsg.Text += Environment.NewLine;
                }
            }
        }
    }
}
