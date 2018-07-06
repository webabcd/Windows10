/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     CapturePreviewToStreamAsync() - 对 WebView 当前显示的内容截图，并将图片写入指定的流
 *     
 *     
 * 本例用于演示如何对 WebView 中的内容截图
 */

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo5 : Page
    {
        public WebViewDemo5()
        {
            this.InitializeComponent();
        }

        private async void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            // 对 WebView 中的内容截图，并将原始图像数据放入内存流
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            await webView.CapturePreviewToStreamAsync(ms);

            // 显示原始截图
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(ms);
            imageOriginal.Source = bitmapImage;



            // 定义缩略图的大小（最长边定义为 180）
            int longlength = 180, width = 0, height = 0;
            double srcwidth = webView.ActualWidth, srcheight = webView.ActualHeight;
            double factor = srcwidth / srcheight;
            if (factor < 1)
            {
                height = longlength;
                width = (int)(longlength * factor);
            }
            else
            {
                width = longlength;
                height = (int)(longlength / factor);
            }

            // 显示原始截图的缩略图
            BitmapSource thumbnail = await resize(width, height, ms);
            imageThumbnail.Source = thumbnail;
        }

        // 将指定的图片修改为指定的大小，并返回修改后的图片
        private async Task<BitmapSource> resize(int width, int height, IRandomAccessStream source)
        {
            WriteableBitmap thumbnail = new WriteableBitmap(width, height);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(source);
            BitmapTransform transform = new BitmapTransform();
            transform.ScaledHeight = (uint)height;
            transform.ScaledWidth = (uint)width;
            PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);
            pixelData.DetachPixelData().CopyTo(thumbnail.PixelBuffer);

            return thumbnail;
        }
    }
}
