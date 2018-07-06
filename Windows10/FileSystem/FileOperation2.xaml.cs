/*
 * 演示如何打开文件，获取指定的本地 uri 的文件，通过 StreamedFileDataRequest 或远程 uri 创建文件或替换文件
 * 
 * StorageFile - 文件操作类
 *     public IAsyncOperation<IRandomAccessStream> OpenAsync(FileAccessMode accessMode) - 打开文件，可以指定是只读方式还是读写方式，返回 IRandomAccessStream 流
 *     public IAsyncOperation<IRandomAccessStreamWithContentType> OpenReadAsync() - 以只读方式打开文件，返回 IRandomAccessStreamWithContentType 流
 *     public IAsyncOperation<IInputStream> OpenSequentialReadAsync() - 以只读方式打开文件，返回 IInputStream 流
 *     
 *     public static IAsyncOperation<StorageFile> GetFileFromApplicationUriAsync(Uri uri);
 *     public static IAsyncOperation<StorageFile> CreateStreamedFileAsync(string displayNameWithExtension, StreamedFileDataRequestedHandler dataRequested, IRandomAccessStreamReference thumbnail);
 *     public static IAsyncOperation<StorageFile> ReplaceWithStreamedFileAsync(IStorageFile fileToReplace, StreamedFileDataRequestedHandler dataRequested, IRandomAccessStreamReference thumbnail);
 *     public static IAsyncOperation<StorageFile> CreateStreamedFileFromUriAsync(string displayNameWithExtension, Uri uri, IRandomAccessStreamReference thumbnail);
 *     public static IAsyncOperation<StorageFile> ReplaceWithStreamedFileFromUriAsync(IStorageFile fileToReplace, Uri uri, IRandomAccessStreamReference thumbnail);
 *     
 *     
 * 注：以上接口不再一一说明，看看下面的示例代码就基本都明白了
 */

using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.FileSystem
{
    public sealed partial class FileOperation2 : Page
    {
        public FileOperation2()
        {
            this.InitializeComponent();
        }
        
        private async void btnGetFile_Click(object sender, RoutedEventArgs e)
        {
            // 获取指定的本地 uri 的文件
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/hololens.jpg"));
            // 只读方式打开文件，返回 IRandomAccessStream 流
            IRandomAccessStream stream = await storageFile.OpenReadAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            image1.Source = bitmapImage;
        }

        private async void btnCreateFile1_Click(object sender, RoutedEventArgs e)
        {
            // 通过 StreamedFileDataRequest 创建文件
            StorageFile storageFile = await StorageFile.CreateStreamedFileAsync("GetFileFromApplicationUriAsync.jpg", StreamHandler, null);
            // 只读方式打开文件，返回 IRandomAccessStream 流
            IRandomAccessStream stream = await storageFile.OpenReadAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            image2.Source = bitmapImage;
        }

        private async void btnCreateFile2_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("http://images.cnblogs.com/mvpteam.gif", UriKind.Absolute);
            // 通过远程 uri 创建文件
            StorageFile storageFile = await StorageFile.CreateStreamedFileFromUriAsync("CreateStreamedFileFromUriAsync.gif", uri, null);
            // 只读方式打开文件，返回 IRandomAccessStream 流
            IRandomAccessStream stream = await storageFile.OpenReadAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            image3.Source = bitmapImage;
        }
       
        private async void btnReplaceFile1_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            // 需要被替换的文件
            StorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdTest\GetFolderForUserAsync.jpg", CreationCollisionOption.ReplaceExisting);

            // 通过 StreamedFileDataRequest 替换指定的文件，然后通过返回的 newFile 对象操作替换后的文件
            StorageFile newFile = await StorageFile.ReplaceWithStreamedFileAsync(storageFile, StreamHandler, null);
            // 只读方式打开文件，返回 IRandomAccessStream 流
            IRandomAccessStream stream = await newFile.OpenReadAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            image4.Source = bitmapImage;
        }
        
        private async void btnReplaceFile2_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder picturesFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.PicturesLibrary);
            // 需要被替换的文件
            StorageFile storageFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdTest\CreateStreamedFileFromUriAsync.jpg", CreationCollisionOption.ReplaceExisting);

            Uri uri = new Uri("http://images.cnblogs.com/mvpteam.gif", UriKind.Absolute);
            // 通过远程 uri 替换指定的文件，然后通过返回的 newFile 对象操作替换后的文件
            StorageFile newFile = await StorageFile.ReplaceWithStreamedFileFromUriAsync(storageFile, uri, null);
            // 只读方式打开文件，返回 IRandomAccessStream 流
            IRandomAccessStream stream = await newFile.OpenReadAsync();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            image5.Source = bitmapImage;
        }


        // 一个 StreamedFileDataRequestedHandler
        private async void StreamHandler(StreamedFileDataRequest stream)
        {
            StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/hololens.jpg"));
            IInputStream inputStream = await storageFile.OpenSequentialReadAsync();
            await RandomAccessStream.CopyAndCloseAsync(inputStream, stream);
        }
    }
}
