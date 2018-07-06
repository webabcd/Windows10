/*
 * Image - 图片控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 */

using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Controls.MediaControl
{
    public sealed partial class ImageDemo1 : Page
    {
        public ImageDemo1()
        {
            this.InitializeComponent();

            this.Loaded += ImageDemo_Loaded;
        }
        
        private async void ImageDemo_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 将 Image 控件的图片源设置为 ms-appx:///Assets/StoreLogo.png
            image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png", UriKind.Absolute));

            // 将图片文件流转换为 ImageSource 对象（BitmapImage 继承自 BitmapSource, BitmapSource 继承自 ImageSource）
            RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/StoreLogo.png", UriKind.Absolute));
            IRandomAccessStream imageStream = await imageStreamRef.OpenReadAsync();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(imageStream);
            image4.Source = bitmapImage;

            // 通过下面这种方式也可以拿到文件的 IRandomAccessStream 流
            // StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
            // IRandomAccessStream stream = await storageFile.OpenReadAsync();
        }

        private void remoteImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            // 图片加载完成后，获取 Image 控件的真实的宽和高
            lblMsg.Text = $"remoteImage_ImageOpened, remoteImage.ActualWidth:{remoteImage.ActualWidth}, remoteImage.ActualHeight:{remoteImage.ActualHeight}";
            lblMsg.Text += Environment.NewLine;

            // 图片加载完成后，获取图片的真实的宽和高
            BitmapSource bs = remoteImage.Source as BitmapSource;
            lblMsg.Text += $"remoteImage_ImageOpened, PixelWidth:{bs.PixelWidth}, PixelHeight:{bs.PixelHeight}";
        }

        private void remoteImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            lblMsg.Text = "remoteImage_ImageFailed";
        }
    }
}