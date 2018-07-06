/*
 * 演示如何在 Application Data（应用程序数据存储）中对文件进行操作
 * 
 * 
 * StorageFile - 文件操作类
 *     public static IAsyncOperation<StorageFile> GetFileFromApplicationUriAsync(Uri uri) - 获取本地指定 uri 的文件
 * 
 * 
 * ApplicationData - 操作 Application Data 中数据的类
 *     Current - 返回当前的 ApplicationData 对象
 *     LocalFolder - 返回 StorageFolder 对象。本地存储，永久保存
 *         保存路径：%USERPROFILE%\AppData\Local\Packages\{PackageId}\LocalState
 *         访问协议：ms-appdata:///local/
 *     RoamingFolder - 返回 StorageFolder 对象。漫游存储，同一微软账户同一应用程序在不同设备间会自动同步
 *         保存路径：%USERPROFILE%\AppData\Local\Packages\{PackageId}\RoamingState
 *         访问协议：ms-appdata:///roaming/
 *     TemporaryFolder - 返回 StorageFolder 对象。临时存储，系统在需要的时候可以将其删除
 *         保存路径：%USERPROFILE%\AppData\Local\Packages\{PackageId}\TempState
 *         访问协议：ms-appdata:///temp/
 *     RoamingStorageQuota - 从漫游存储同步到云端的数据的最大大小，只读
 *     ClearAsync() - 删除 Application Data 中的数据
 *     ClearAsync(ApplicationDataLocality locality) - 删除指定存储区的数据据
 *         ApplicationDataLocality.Local, ApplicationDataLocality.Roaming, ApplicationDataLocality.Temporary
 *    
 *     DataChanged - 从服务端向 app 同步漫游数据时所触发的事件
 *     SignalDataChanged() - 强行触发 DataChanged 事件
 */

using System;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem.AppData
{
    public sealed partial class FileDemo : Page
    {
        StorageFolder _localFolder = ApplicationData.Current.LocalFolder;

        public FileDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ApplicationData.Current.DataChanged += Current_DataChanged;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationData.Current.DataChanged -= Current_DataChanged;

            base.OnNavigatedFrom(e);
        }

        //  从服务端向 app 同步漫游数据时
        private async void Current_DataChanged(ApplicationData sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "DataChanged 事件被触发";
            });
        }

        private async void btnReadWrite_Click(object sender, RoutedEventArgs e)
        {
            // 写
            StorageFile fileWrite = await _localFolder.CreateFileAsync(@"webabcdTest\readWriteDemo.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(fileWrite, "I am webabcd: " + DateTime.Now.ToString());

            // 读
            // StorageFile fileRead = await _localFolder.GetFileAsync(@"webabcdTest\readWriteDemo.txt");
            StorageFile fileRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/webabcdTest/readWriteDemo.txt", UriKind.Absolute));
            string textContent = await FileIO.ReadTextAsync(fileRead);
            lblMsg.Text = textContent;

            // 强行触发 DataChanged 事件（仅为演示用，实际项目中不需要）
            ApplicationData.Current.SignalDataChanged();
        }
    }
}
