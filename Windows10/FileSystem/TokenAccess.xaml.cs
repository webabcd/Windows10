/*
 * 演示如何读写“最近访问列表”和“未来访问列表”
 * 
 * StorageApplicationPermissions - 文件/文件夹的访问列表
 *     MostRecentlyUsedList - 最近访问列表（实现了 IStorageItemAccessList 接口）
 *     FutureAccessList - 未来访问列表（实现了 IStorageItemAccessList 接口）
 *     IStorageItemAccessList
 *         Add(IStorageItem file, string metadata) - 添加文件或文件夹到“列表”，返回 token 值（一个字符串类型的标识），通过此值可以方便地检索到对应的文件或文件夹
 *             file - 需要添加到列表的文件或文件夹
 *             metadata - 自定义元数据，相当于上下文
 *         AddOrReplace(string token, IStorageItem file, string metadata) - 添加文件或文件夹到“列表”，如果已存在则替换
 *         GetFileAsync(string token) - 根据 token 值，在“列表”查找对应的文件
 *         GetFolderAsync(string token) - 根据 token 值，在“列表”查找对应的文件夹
 *         GetItemAsync(string token) - 根据 token 值，在“列表”查找对应的文件或文件夹
 *         Remove(string token) - 从“列表”中删除指定 token 值的文件或文件夹
 *         ContainsItem(string token) - “列表”中是否存储指定 token 值的文件或文件夹
 *         Clear() - 清除“列表”中的全部文件和文件夹
 *         CheckAccess(IStorageItem file) - 用于验证 app 是否可在“列表”中访问指定的文件或文件夹
 *         Entries - 返回 AccessListEntryView 类型的数据，其是 AccessListEntry 类型数据的集合
 *         MaximumItemsAllowed - “列表”可以保存的文件和文件夹的最大数目
 *         
 * AccessListEntry - 用于封装访问列表中的 StorageFile 或 StorageFolder 的 token 和元数据
 *     Token - token 值
 *     Metadata - 元数据
 *     
 *     
 * 注：
 * 1、通常情况下，MostRecentlyUsedList 最大可以保存 25 条数据，FutureAccessList 最大可以保存 1000 条数据。实际情况可通过 IStorageItemAccessList 的 MaximumItemsAllowed 来获取
 * 2、这个特性的好处就是：保存到 MostRecentlyUsedList 或 FutureAccessList 中的文件或文件夹，可以直接通过 token 访问到
 */

using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace Windows10.FileSystem
{
    public sealed partial class TokenAccess : Page
    {
        public TokenAccess()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 在指定的目录下创建指定的文件
            StorageFolder documentsFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await documentsFolder.CreateFileAsync("webabcdCacheAccess.txt", CreationCollisionOption.ReplaceExisting);

            // 在指定的文件中写入指定的文本
            string textContent = "I am webabcd";
            await FileIO.WriteTextAsync(storageFile, textContent, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            base.OnNavigatedTo(e);
        }


        private async void btnAddToMostRecentlyUsedList_Click(object sender, RoutedEventArgs e)
        {
            // 获取文件对象
            StorageFolder documentsFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await documentsFolder.GetFileAsync("webabcdCacheAccess.txt");

            if (storageFile != null && StorageApplicationPermissions.MostRecentlyUsedList.CheckAccess(storageFile))
            {
                // 将文件添加到“最近访问列表”，并获取对应的 token 值
                string token = StorageApplicationPermissions.MostRecentlyUsedList.Add(storageFile, storageFile.Name);
                lblMsg.Text = "token：" + token;
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "MostRecentlyUsedList MaximumItemsAllowed: " + StorageApplicationPermissions.MostRecentlyUsedList.MaximumItemsAllowed;
            }
        }

        private async void btnGetMostRecentlyUsedList_Click(object sender, RoutedEventArgs e)
        {
            AccessListEntryView entries = StorageApplicationPermissions.MostRecentlyUsedList.Entries;
            if (entries.Count > 0)
            {
                // 通过 token 值，从“最近访问列表”中获取文件对象
                AccessListEntry entry = entries[0];
                StorageFile storageFile = await StorageApplicationPermissions.MostRecentlyUsedList.GetFileAsync(entry.Token);

                string textContent = await FileIO.ReadTextAsync(storageFile);
                lblMsg.Text = "MostRecentlyUsedList 的第一个文件的文本内容：" + textContent;
            }
            else
            {
                lblMsg.Text = "最近访问列表中无数据";
            }
        }


        private async void btnAddToFutureAccessList_Click(object sender, RoutedEventArgs e)
        {
            // 获取文件对象
            StorageFolder documentsFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await documentsFolder.GetFileAsync("webabcdCacheAccess.txt");

            if (storageFile != null && StorageApplicationPermissions.FutureAccessList.CheckAccess(storageFile))
            {
                // 将文件添加到“未来访问列表”，并获取对应的 token 值
                string token = StorageApplicationPermissions.FutureAccessList.Add(storageFile, storageFile.Name);
                lblMsg.Text = "token：" + token;
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "FutureAccessList MaximumItemsAllowed: " + StorageApplicationPermissions.FutureAccessList.MaximumItemsAllowed;
            }
        }
        
        private async void btnGetFutureAccessList_Click(object sender, RoutedEventArgs e)
        {
            AccessListEntryView entries = StorageApplicationPermissions.FutureAccessList.Entries;
            if (entries.Count > 0)
            {
                // 通过 token 值，从“未来访问列表”中获取文件对象
                AccessListEntry entry = entries[0];
                StorageFile storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(entry.Token);

                string textContent = await FileIO.ReadTextAsync(storageFile);
                lblMsg.Text = "FutureAccessList 的第一个文件的文本内容：" + textContent;
            }
            else
            {
                lblMsg.Text = "未来访问列表中无数据";
            }
        }
    }
}
