/*
 * 演示如何获取文件的属性，修改文件的属性
 * 
 * StorageFile - 文件操作类
 *     直接通过调用 Name, Path, DisplayName, DisplayType, FolderRelativeId, Provider, DateCreated, Attributes, ContentType, FileType, IsAvailable 获取相关属性，详见文档
 *     GetBasicPropertiesAsync() - 返回一个 BasicProperties 类型的对象 
 *     Properties - 返回一个 StorageItemContentProperties 类型的对象
 *
 * BasicProperties
 *     可以获取的数据有 Size, DateModified, ItemDate
 * 
 * StorageItemContentProperties
 *     通过 GetImagePropertiesAsync() 获取图片属性，详见文档
 *     通过 GetVideoPropertiesAsync() 获取视频属性，详见文档
 *     通过 GetMusicPropertiesAsync() 获取音频属性，详见文档
 *     通过 GetDocumentPropertiesAsync() 获取文档属性，详见文档
 *     通过调用 RetrievePropertiesAsync() 方法来获取指定的属性，然后可以调用 SavePropertiesAsync() 方法来保存修改后的属性，详见下面的示例
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem
{
    public sealed partial class FileProperties : Page
    {
        public FileProperties()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取“图片库”目录下的所有根文件
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            listBox.ItemsSource = fileList.Select(p => p.Name).ToList();

            base.OnNavigatedTo(e);
        }

        private async void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 用户选中的文件
            string fileName = (string)listBox.SelectedItem;
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            StorageFile storageFile = await picturesFolder.GetFileAsync(fileName);

            // 显示文件的各种属性
            ShowProperties1(storageFile);
            await ShowProperties2(storageFile);
            await ShowProperties3(storageFile);
            await ShowProperties4(storageFile);
        }

        // 通过 StorageFile 获取文件的属性
        private void ShowProperties1(StorageFile storageFile)
        {
            lblMsg.Text = "Name：" + storageFile.Name;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Path：" + storageFile.Path;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DisplayName：" + storageFile.DisplayName;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DisplayType：" + storageFile.DisplayType;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "FolderRelativeId：" + storageFile.FolderRelativeId;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Provider：" + storageFile.Provider;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DateCreated：" + storageFile.DateCreated;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Attributes：" + storageFile.Attributes; // 返回一个 FileAttributes 类型的枚举（FlagsAttribute），可以从中获知文件是否是 ReadOnly 之类的信息
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ContentType：" + storageFile.ContentType;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "FileType：" + storageFile.FileType;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "IsAvailable：" + storageFile.IsAvailable;
            lblMsg.Text += Environment.NewLine;
        }

        // 通过 StorageFile.GetBasicPropertiesAsync() 获取文件的属性
        private async Task ShowProperties2(StorageFile storageFile)
        {
            BasicProperties basicProperties = await storageFile.GetBasicPropertiesAsync();
            lblMsg.Text += "Size：" + basicProperties.Size;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DateModified：" + basicProperties.DateModified;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ItemDate：" + basicProperties.ItemDate;
            lblMsg.Text += Environment.NewLine;
        }

        // 通过 StorageFolder.Properties 的 RetrievePropertiesAsync() 方法获取文件的属性
        private async Task ShowProperties3(StorageFile storageFile)
        {
            /*
             * 获取文件的其它各种属性
             * 详细的属性列表请参见结尾处的“附录一: 属性列表”或者参见：http://msdn.microsoft.com/en-us/library/windows/desktop/ff521735(v=vs.85).aspx
             */
            List<string> propertiesName = new List<string>();
            propertiesName.Add("System.DateAccessed");
            propertiesName.Add("System.DateCreated");
            propertiesName.Add("System.FileOwner");
            propertiesName.Add("System.FileAttributes");

            StorageItemContentProperties storageItemContentProperties = storageFile.Properties;
            IDictionary<string, object> extraProperties = await storageItemContentProperties.RetrievePropertiesAsync(propertiesName);

            lblMsg.Text += "System.DateAccessed：" + extraProperties["System.DateAccessed"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "System.DateCreated：" + extraProperties["System.DateCreated"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "System.FileOwner：" + extraProperties["System.FileOwner"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "System.FileAttributes：" + extraProperties["System.FileAttributes"];
            lblMsg.Text += Environment.NewLine;
        }

        // 通过 StorageFolder.Properties 的 GetImagePropertiesAsync(), GetVideoPropertiesAsync(), GetMusicPropertiesAsync(), GetDocumentPropertiesAsync() 方法获取文件的属性
        private async Task ShowProperties4(StorageFile storageFile)
        {
            StorageItemContentProperties storageItemContentProperties = storageFile.Properties;
            ImageProperties imageProperties = await storageItemContentProperties.GetImagePropertiesAsync(); // 图片属性
            VideoProperties videoProperties = await storageItemContentProperties.GetVideoPropertiesAsync(); // 视频属性
            MusicProperties musicProperties = await storageItemContentProperties.GetMusicPropertiesAsync(); // 音频属性
            DocumentProperties documentProperties = await storageItemContentProperties.GetDocumentPropertiesAsync(); // 文档属性

            lblMsg.Text += "image width：" + imageProperties.Width;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "image height：" + imageProperties.Height;
            lblMsg.Text += Environment.NewLine;
        }

        // 修改文件的属性
        private async void btnModifyProperty_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 用户选中的文件
            string fileName = (string)listBox.SelectedItem;
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            StorageFile storageFile = await picturesFolder.GetFileAsync(fileName);

            // 在 System.FileAttributes 中保存有文件是否是“只读”的信息
            string[] retrieveList = new string[] { "System.FileAttributes" };
            StorageItemContentProperties storageItemContentProperties = storageFile.Properties;
            IDictionary<string, object> extraProperties = await storageItemContentProperties.RetrievePropertiesAsync(retrieveList);

            uint FILE_ATTRIBUTES_READONLY = 1;
            if (extraProperties != null && extraProperties.ContainsKey("System.FileAttributes"))
            {
                // 切换文件的只读属性
                uint temp = (UInt32)extraProperties["System.FileAttributes"] ^ FILE_ATTRIBUTES_READONLY;

                // 设置文件的只读属性为 true
                // uint temp = (UInt32)extraProperties["System.FileAttributes"] | 1;

                // 设置文件的只读属性为 false
                // uint temp = (UInt32)extraProperties["System.FileAttributes"] & 0;

                extraProperties["System.FileAttributes"] = temp;
            }
            else
            {
                // 设置文件的只读属性为 true
                extraProperties = new PropertySet();
                extraProperties.Add("System.FileAttributes", FILE_ATTRIBUTES_READONLY);
            }

            // 保存修改后的属性（用这种方法可以修改部分属性，大部分属性都是无权限修改的）
            await storageFile.Properties.SavePropertiesAsync(extraProperties);
        }
    }
}


/*
----------------------------------------------------------------------
附录一: 属性列表

System.AcquisitionID
System.ApplicationName
System.Author
System.Capacity
System.Category
System.Comment
System.Company
System.ComputerName
System.ContainedItems
System.ContentStatus
System.ContentType
System.Copyright
System.DataObjectFormat
System.DateAccessed
System.DateAcquired
System.DateArchived
System.DateCompleted
System.DateCreated
System.DateImported
System.DateModified
System.DefaultSaveLocationIconDisplay
System.DueDate
System.EndDate
System.FileAllocationSize
System.FileAttributes
System.FileCount
System.FileDescription
System.FileExtension
System.FileFRN
System.FileName
System.FileOwner
System.FileVersion
System.FindData
System.FlagColor
System.FlagColorText
System.FlagStatus
System.FlagStatusText
System.FreeSpace
System.FullText
System.Identity
System.Identity.Blob
System.Identity.DisplayName
System.Identity.IsMeIdentity
System.Identity.PrimaryEmailAddress
System.Identity.ProviderID
System.Identity.UniqueID
System.Identity.UserName
System.IdentityProvider.Name
System.IdentityProvider.Picture
System.ImageParsingName
System.Importance
System.ImportanceText
System.IsAttachment
System.IsDefaultNonOwnerSaveLocation
System.IsDefaultSaveLocation
System.IsDeleted
System.IsEncrypted
System.IsFlagged
System.IsFlaggedComplete
System.IsIncomplete
System.IsLocationSupported
System.IsPinnedToNameSpaceTree
System.IsRead
System.IsSearchOnlyItem
System.IsSendToTarget
System.IsShared
System.ItemAuthors
System.ItemClassType
System.ItemDate
System.ItemFolderNameDisplay
System.ItemFolderPathDisplay
System.ItemFolderPathDisplayNarrow
System.ItemName
System.ItemNameDisplay
System.ItemNamePrefix
System.ItemParticipants
System.ItemPathDisplay
System.ItemPathDisplayNarrow
System.ItemType
System.ItemTypeText
System.ItemUrl
System.Keywords
System.Kind
System.KindText
System.Language
System.LayoutPattern.ContentViewModeForBrowse
System.LayoutPattern.ContentViewModeForSearch
System.LibraryLocationsCount
System.MileageInformation
System.MIMEType
System.Null
System.OfflineAvailability
System.OfflineStatus
System.OriginalFileName
System.OwnerSID
System.ParentalRating
System.ParentalRatingReason
System.ParentalRatingsOrganization
System.ParsingBindContext
System.ParsingName
System.ParsingPath
System.PerceivedType
System.PercentFull
System.Priority
System.PriorityText
System.Project
System.ProviderItemID
System.Rating
System.RatingText
System.Sensitivity
System.SensitivityText
System.SFGAOFlags
System.SharedWith
System.ShareUserRating
System.SharingStatus
System.Shell.OmitFromView
System.SimpleRating
System.Size
System.SoftwareUsed
System.SourceItem
System.StartDate
System.Status
System.StatusBarSelectedItemCount
System.StatusBarViewItemCount
System.Subject
System.Thumbnail
System.ThumbnailCacheId
System.ThumbnailStream
System.Title
System.TotalFileSize
System.Trademarks
----------------------------------------------------------------------
*/
