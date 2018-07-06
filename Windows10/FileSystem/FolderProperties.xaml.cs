/*
 * 演示如何获取文件夹的属性
 * 
 * StorageFolder - 文件夹操作类
 *     直接通过调用 Name, Path, DisplayName, DisplayType, FolderRelativeId, Provider, DateCreated, Attributes 获取相关属性，详见文档
 *     GetBasicPropertiesAsync() - 返回一个 BasicProperties 类型的对象 
 *     Properties - 返回一个 StorageItemContentProperties 类型的对象
 *
 * BasicProperties
 *     可以获取的数据有 Size, DateModified, ItemDate
 * 
 * StorageItemContentProperties
 *     通过调用 RetrievePropertiesAsync() 方法来获取指定的属性，详见下面的示例
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem
{
    public sealed partial class FolderProperties : Page
    {
        public FolderProperties()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取“图片库”目录下的所有根文件夹
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            IReadOnlyList<StorageFolder> folderList = await picturesFolder.GetFoldersAsync();
            listBox.ItemsSource = folderList.Select(p => p.Name).ToList();

            base.OnNavigatedTo(e);
        }

        private async void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 用户选中的文件夹
            string folderName = (string)listBox.SelectedItem;
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            StorageFolder storageFolder = await picturesFolder.GetFolderAsync(folderName);

            // 显示文件夹的各种属性
            ShowProperties1(storageFolder);
            await ShowProperties2(storageFolder);
            await ShowProperties3(storageFolder);
        }

        // 通过 StorageFolder 获取文件夹的属性
        private void ShowProperties1(StorageFolder storageFolder)
        {
            lblMsg.Text = "Name：" + storageFolder.Name;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Path：" + storageFolder.Path;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DisplayName：" + storageFolder.DisplayName;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DisplayType：" + storageFolder.DisplayType;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "FolderRelativeId：" + storageFolder.FolderRelativeId;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Provider：" + storageFolder.Provider;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DateCreated：" + storageFolder.DateCreated;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "Attributes：" + storageFolder.Attributes; // 返回一个 FileAttributes 类型的枚举（FlagsAttribute），可以从中获知文件夹是否是 ReadOnly 之类的信息
            lblMsg.Text += Environment.NewLine;
        }

        // 通过 StorageFolder.GetBasicPropertiesAsync() 获取文件夹的属性
        private async Task ShowProperties2(StorageFolder storageFolder)
        {
            BasicProperties basicProperties = await storageFolder.GetBasicPropertiesAsync();
            lblMsg.Text += "Size：" + basicProperties.Size;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "DateModified：" + basicProperties.DateModified;
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "ItemDate：" + basicProperties.ItemDate;
            lblMsg.Text += Environment.NewLine;
        }

        // 通过 StorageFolder.Properties 的 RetrievePropertiesAsync() 方法获取文件夹的属性
        private async Task ShowProperties3(StorageFolder storageFolder)
        {
            /*
             * 获取文件夹的其它各种属性
             * 详细的属性列表请参见结尾处的“附录一: 属性列表”或者参见：http://msdn.microsoft.com/en-us/library/windows/desktop/ff521735(v=vs.85).aspx
             */
            List<string> propertiesName = new List<string>();
            propertiesName.Add("System.DateAccessed");
            propertiesName.Add("System.DateCreated");
            propertiesName.Add("System.FileOwner");

            StorageItemContentProperties storageItemContentProperties = storageFolder.Properties;
            IDictionary<string, object> extraProperties = await storageItemContentProperties.RetrievePropertiesAsync(propertiesName);

            lblMsg.Text += "System.DateAccessed：" + extraProperties["System.DateAccessed"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "System.DateCreated：" + extraProperties["System.DateCreated"];
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += "System.FileOwner：" + extraProperties["System.FileOwner"];
            lblMsg.Text += Environment.NewLine;
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
