/*
 * 演示如何获取文件的缩略图
 * 
 * StorageFile - 文件操作类。与获取文件缩略图相关的接口如下
 *     public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode);
 *     public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode, uint requestedSize);
 *     public IAsyncOperation<StorageItemThumbnail> GetThumbnailAsync(ThumbnailMode mode, uint requestedSize, ThumbnailOptions options);
 *         ThumbnailMode mode - 用于描述缩略图的目的，以使系统确定缩略图图像的调整方式（就用 SingleItem 即可）
 *         uint requestedSize - 期望尺寸的最长边长的大小
 *         ThumbnailOptions options - 检索和调整缩略图的行为（默认值：UseCurrentScale）
 *         
 * StorageItemThumbnail - 缩略图（实现了 IRandomAccessStream 接口，可以直接转换为 BitmapImage 对象）
 *     OriginalWidth - 缩略图的宽
 *     OriginalHeight - 缩略图的高
 *     Size - 缩略图的大小（单位：字节）
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem
{
    public sealed partial class FileThumbnail : Page
    {
        public FileThumbnail()
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

            // 显示文件的缩略图
            await ShowThumbnail(storageFile);
        }

        private async Task ShowThumbnail(StorageFile storageFile)
        {
            // 如果要获取文件的缩略图，就指定为 ThumbnailMode.SingleItem 即可
            ThumbnailMode thumbnailMode = ThumbnailMode.SingleItem;
            uint requestedSize = 200;
            ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;

            using (StorageItemThumbnail thumbnail = await storageFile.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions))
            {
                if (thumbnail != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(thumbnail);
                    imageThumbnail.Source = bitmapImage;

                    lblMsg.Text = $"thumbnail1 requestedSize:{requestedSize}, returnedSize:{thumbnail.OriginalWidth}x{thumbnail.OriginalHeight}, size:{thumbnail.Size}";
                }
            }
        }
    }
}
