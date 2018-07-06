/*
 * 演示如何获取 Package 中的文件
 * 1、可以通过 Package.Current.InstalledLocation.GetFileAsync() 访问
 * 2、可以通过 ms-appx:/// 访问
 * 
 * 
 * StorageFile - 文件操作类
 *     public static IAsyncOperation<StorageFile> GetFileFromApplicationUriAsync(Uri uri) - 获取本地指定 uri 的文件
 *     
 *     
 * 注：需要访问的 Package 中的文件的属性的“生成操作”应该设置为“内容”
 */

using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem.PackageData
{
    public sealed partial class Demo : Page
    {
        public Demo()
        {
            this.InitializeComponent();
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            // 写（无法向程序包中写数据，会报错）
            // StorageFile fileWrite = await Package.Current.InstalledLocation.CreateFileAsync(@"FileSystem\PackageData\readWriteDemo.txt", CreationCollisionOption.ReplaceExisting);
            // await FileIO.WriteTextAsync(fileWrite, "I am webabcd: " + DateTime.Now.ToString());

            // 读
            // StorageFile fileRead = await Package.Current.InstalledLocation.GetFileAsync(@"FileSystem\PackageData\readWriteDemo.txt");
            StorageFile fileRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileSystem/PackageData/readWriteDemo.txt", UriKind.Absolute));
            string textContent = await FileIO.ReadTextAsync(fileRead);
            lblMsg.Text = textContent;


            // 引用程序包内的图片文件并显示
            img.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/hololens.jpg"));
        }
    }
}
