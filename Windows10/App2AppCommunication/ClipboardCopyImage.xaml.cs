/*
 * 1、演示如何复制图片内容剪切板，以及如何从剪切板中获取图片内容
 * 2、演示如何通过 SetDataProvider() 延迟数据的复制，即在“粘贴”操作触发后由数据提供器生成相关数据
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows10.App2AppCommunication
{
    public sealed partial class ClipboardCopyImage : Page
    {
        public ClipboardCopyImage()
        {
            this.InitializeComponent();
        }

        // 复制图片内容到剪切板
        private void btnCopyImage_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/StoreLogo.png", UriKind.Absolute)));

            try
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                lblMsg.Text = "已将内容复制到剪切板";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 延迟复制
        private async void btnCopyImageWithDeferral_Click(object sender, RoutedEventArgs e)
        {
            StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png", UriKind.Absolute));

            DataPackage dataPackage = new DataPackage();
            dataPackage.SetDataProvider(StandardDataFormats.Bitmap, async (request) =>
            {
                /*
                 * 当从剪切板中获取 StandardDataFormats.Bitmap 数据时，会执行到此处以提供相关数据
                 */

                if (imageFile != null)
                {
                    // 开始异步处理
                    var deferral = request.GetDeferral();

                    try
                    {
                        using (var imageStream = await imageFile.OpenAsync(FileAccessMode.Read))
                        {
                            // 将图片缩小一倍
                            BitmapDecoder imageDecoder = await BitmapDecoder.CreateAsync(imageStream);
                            var inMemoryStream = new InMemoryRandomAccessStream();
                            var imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
                            imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * 0.5);
                            imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * 0.5);
                            await imageEncoder.FlushAsync();

                            // 指定需要复制到剪切板的数据
                            request.SetData(RandomAccessStreamReference.CreateFromStream(inMemoryStream));

                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                lblMsg.Text = "数据已生成";
                            });
                        }
                    }
                    finally
                    {
                        // 通知系统已完成异步操作
                        deferral.Complete();
                    }
                }
            });

            try
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                lblMsg.Text = "已将数据提供器复制到剪切板，在“粘贴”操作时才会生成数据";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }

        // 显示剪切板中的图片内容
        private async void btnPasteImage_Click(object sender, RoutedEventArgs e)
        {
            DataPackageView dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                try
                {
                    IRandomAccessStreamReference randomStream = await dataPackageView.GetBitmapAsync();
                    if (randomStream != null)
                    {
                        using (IRandomAccessStreamWithContentType imageStream = await randomStream.OpenReadAsync())
                        {
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(imageStream);
                            imgBitmap.Source = bitmapImage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
            else
            {
                lblMsg.Text = "剪切板中无 bitmap 内容";
            }
        }
    }
}
