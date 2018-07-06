/*
 * Image - 图片控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 */

using System;
using System.IO;
using System.Reflection;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows10.Common;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class ImageDemo2 : Page
    {
        public ImageDemo2()
        {
            this.InitializeComponent();
        }

        private async void image4_Loaded(object sender, RoutedEventArgs e)
        {
            // 将程序包内的 png 文件复制到 ApplicationData 中的 LocalFolder
            StorageFolder localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("webabcdTest", CreationCollisionOption.OpenIfExists);
            StorageFile packageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
            await packageFile.CopyAsync(localFolder, "StoreLogo.png", NameCollisionOption.ReplaceExisting);

            // 通过 ms-appdata:/// 协议加载 Application 内图片
            string url = "ms-appdata:///local/webabcdTest/StoreLogo.png";
            image4.Source = new BitmapImage(new Uri(url, UriKind.Absolute));
        }

        private async void image5_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取 Package 内嵌入式资源数据
            Assembly assembly = typeof(ImageDemo2).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Windows10.Controls.MediaControl.EmbeddedResource.png");

            IRandomAccessStream imageStream = await ConverterHelper.Stream2RandomAccessStream(stream);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(imageStream);
            image5.Source = bitmapImage;
        }
    }
}
