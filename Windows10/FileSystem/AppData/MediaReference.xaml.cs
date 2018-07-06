/*
 * 演示如何通过 uri 的方式引用 Application Data（应用程序数据存储）中的媒体（图片、视频或音频）文件
 * 
 * ApplicationData 中的 LocalFolder 对应 ms-appdata:///local/
 * ApplicationData 中的 RoamingFolder 对应 ms-appdata:///roaming/
 * ApplicationData 中的 TemporaryFolder 对应 ms-appdata:///temp/
 * 
 * StorageFile - 文件操作类
 *     public static IAsyncOperation<StorageFile> GetFileFromApplicationUriAsync(Uri uri) - 获取本地指定 uri 的文件
 */

using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem.AppData
{
    public sealed partial class MediaReference : Page
    {
        public MediaReference()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData appData = ApplicationData.Current;

            try
            {
                // 将程序包内的文件保存到 ApplicationData 中的 TemporaryFolder
                StorageFile imgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Logo.png"));
                await imgFile.CopyAsync(appData.TemporaryFolder, imgFile.Name, NameCollisionOption.ReplaceExisting);
            }
            catch { }

            // 引用 Application Data 内的图片文件并显示
            imgAppdata.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appdata:///temp/Logo.png"));
            // 引用程序包内的图片文件并显示
            imgAppx.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/Logo.png"));

            // 也可以在 xaml 这样写
            // <img src="ms-appdata:///temp/Logo.png" />

            base.OnNavigatedTo(e);
        }
    }
}
