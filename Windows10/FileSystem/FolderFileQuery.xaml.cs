/*
 * 演示如何分组文件夹，排序过滤文件夹和文件，搜索文件
 * 
 * StorageFolder - 文件夹操作类。与分组，排序，过滤，搜索相关的接口如下（File 代表文件，Folder 代表文件夹，Item 代表文件和文件夹）
 *     public StorageFileQueryResult CreateFileQuery();
 *     public StorageFileQueryResult CreateFileQuery(CommonFileQuery query);
 *     public StorageFileQueryResult CreateFileQueryWithOptions(QueryOptions queryOptions);
 *     public StorageFolderQueryResult CreateFolderQuery();
 *     public StorageFolderQueryResult CreateFolderQuery(CommonFolderQuery query);
 *     public StorageFolderQueryResult CreateFolderQueryWithOptions(QueryOptions queryOptions);
 *     public StorageItemQueryResult CreateItemQuery();
 *     public StorageItemQueryResult CreateItemQueryWithOptions(QueryOptions queryOptions);
 *     public IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync(CommonFileQuery query, uint startIndex, uint maxItemsToRetrieve);
 *     public IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync(CommonFileQuery query);
 *     public IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync(CommonFolderQuery query, uint startIndex, uint maxItemsToRetrieve);
 *     public IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync(CommonFolderQuery query);
 *     public IAsyncOperation<IReadOnlyList<IStorageItem>> GetItemsAsync(uint startIndex, uint maxItemsToRetrieve);
 *     public bool AreQueryOptionsSupported(QueryOptions queryOptions);
 *     public bool IsCommonFolderQuerySupported(CommonFolderQuery query);
 *     public bool IsCommonFileQuerySupported(CommonFileQuery query);
 *     
 * CommonFolderQuery - 文件夹分组方式枚举
 * 
 * CommonFileQuery - 文件排序方式枚举
 * 
 * QueryOptions - 查询参数
 *     可以通过 FolderDepth 指定是只查询根目录还是查询根目录和所有子目录
 *     可以通过 IndexerOption 指定查询时，如何使用系统索引
 *     可以指定 CommonFolderQuery
 *     可以指定 CommonFileQuery 和需要过滤的文件类型
 *     通过 SetPropertyPrefetch(), SetThumbnailPrefetch() 可以预加载指定的属性和指定规格的缩略图（耗费更多的资源，加快检索速度）
 *     
 * StorageFileQueryResult, StorageFolderQueryResult, StorageItemQueryResult - 查询实例
 *     可以执行这个查询，可以获取这个查询的结果的总数，可以按指的 startIndex 和 maxNumber 执行这个查询，可以设置新的查询参数
 *     
 *     
 * 注：以上接口不再一一说明，看看下面的示例代码就基本都明白了
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class FolderFileQuery : Page
    {
        public FolderFileQuery()
        {
            this.InitializeComponent();
        }

        // 分组文件夹
        private async void btnFolderGroup_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "";

            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);

            // 文件夹按月分组查询参数，其他多种分组方式请参见 CommonFolderQuery 枚举
            CommonFolderQuery folderQuery = CommonFolderQuery.GroupByMonth;

            // 判断一下 picturesFolder 是否支持指定的查询参数
            if (picturesFolder.IsCommonFolderQuerySupported(folderQuery))
            {
                // 创建查询
                StorageFolderQueryResult queryResult = picturesFolder.CreateFolderQuery(folderQuery);

                // 执行查询
                IReadOnlyList<StorageFolder> folderList = await queryResult.GetFoldersAsync();

                foreach (StorageFolder storageFolder in folderList) // 这里的 storageFolder 就是按月份分组后的月份文件夹（当然，物理上并没有月份文件夹）
                {
                    IReadOnlyList<StorageFile> fileList = await storageFolder.GetFilesAsync();
                    lblMsg.Text += storageFolder.Name + " (" + fileList.Count + ")";
                    lblMsg.Text += Environment.NewLine;
                    foreach (StorageFile file in fileList) // 月份文件夹内的文件
                    {
                        lblMsg.Text += "    " + file.Name;
                        lblMsg.Text += Environment.NewLine;
                    }
                }
            }
        }

        // 排序过滤文件夹和文件
        private async void btnFolderFileOrderFilter_Click(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = "";

            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            
            // 设置需要过滤的文件的扩展名
            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add(".txt");

            // 创建一个查询参数，可以指定文件的排序方式和文件的类型过滤。文件的各种排序方式请参见 CommonFileQuery 枚举
            QueryOptions query = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);

            // 默认是正序的，如果需要倒序的话可以这样写
            SortEntry se = query.SortOrder[0];
            se.AscendingOrder = false;
            query.SortOrder.RemoveAt(0);
            query.SortOrder.Add(se);

            // 判断一下 picturesFolder 是否支持指定的查询参数
            if (picturesFolder.AreQueryOptionsSupported(query))
            {
                // 创建查询
                StorageItemQueryResult queryResult = picturesFolder.CreateItemQueryWithOptions(query);

                // 执行查询
                IReadOnlyList<IStorageItem> storageItems = await queryResult.GetItemsAsync();

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

        // 搜索文件
        private async void btnFileSearch_Click(object sender, RoutedEventArgs e)
        {
            // 准备在“音乐库”中进行搜索
            StorageFolder musicFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.MusicLibrary);

            // 准备搜索所有类型的文件
            List<string> fileTypeFilter = new List<string>();
            fileTypeFilter.Add("*");

            // 搜索的查询参数
            QueryOptions queryOptions = new QueryOptions(CommonFileQuery.OrderByDate, fileTypeFilter);
            // 指定 AQS 字符串（Advanced Query Syntax），参见 http://msdn.microsoft.com/zh-cn/library/windows/apps/aa965711.aspx
            queryOptions.UserSearchFilter = "五月天";
            // 搜索根目录和所有子目录
            queryOptions.FolderDepth = FolderDepth.Deep;

            // 根据指定的参数创建一个查询
            StorageFileQueryResult fileQuery = musicFolder.CreateFileQueryWithOptions(queryOptions);

            lblMsg.Text = "在音乐库中搜索“五月天”，结果如下：";
            lblMsg.Text += Environment.NewLine;

            // 开始搜索，并返回检索到的文件列表
            IReadOnlyList<StorageFile> files = await fileQuery.GetFilesAsync();

            if (files.Count == 0)
            {
                lblMsg.Text += "什么都没搜到";
            }
            else
            {
                foreach (StorageFile file in files)
                {
                    lblMsg.Text += file.Name;
                    lblMsg.Text += Environment.NewLine;
                }
            }
        }
    }
}
