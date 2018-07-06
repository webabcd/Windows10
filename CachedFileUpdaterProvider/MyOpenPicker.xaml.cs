/*
 * 打开一个文件，并关联到 CachedFileUpdater
 * 
 * 1、在 Package.appxmanifest 中新增一个“文件打开选取器”声明，并做相关配置
 * 2、在 App.xaml.cs 中 override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)，如果 app 是由文件打开选取器激活的，则可以在此获取其相关信息
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers.Provider;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CachedFileUpdaterProvider
{
    public sealed partial class MyOpenPicker : Page
    {
        private FileOpenPickerUI _fileOpenPickerUI;

        public MyOpenPicker()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 FileOpenPickerUI 对象（从 App.xaml.cs 传来的）
            var args = (FileOpenPickerActivatedEventArgs)e.Parameter;
            _fileOpenPickerUI = args.FileOpenPickerUI;

            _fileOpenPickerUI.Title = "自定义文件打开选取器";

            base.OnNavigatedTo(e);
        }

        // 本 CachedFile 用于从 Local 更新（由 CachedFileUpdater 更新 CachedFile）
        private async void btnPickCachedFileLocal_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdCachedFileUpdaterLocal.txt", CreationCollisionOption.ReplaceExisting);
            string textContent = "I am webabcd";

            await FileIO.WriteTextAsync(file, textContent);

            /*
             * 设置 CachedFile，即将文件关联到 CachedFileUpdater
             * SetUpdateInformation(IStorageFile file, string contentId, ReadActivationMode readMode, WriteActivationMode writeMode, CachedFileOptions options);
             *     file - 与 CachedFileUpdater 关联的文件
             *     contentId - 与 CachedFileUpdater 关联的文件标识
             */
            CachedFileUpdater.SetUpdateInformation(file, "cachedFileLocal", ReadActivationMode.BeforeAccess, WriteActivationMode.NotNeeded, CachedFileOptions.RequireUpdateOnAccess);

            lblMsg.Text = "选择的文件: " + file.Name;
            AddFileResult result = _fileOpenPickerUI.AddFile("myFile", file);
        }

        // 本 CachedFile 用于从 Remote 更新（由 app 更新 CachedFile）
        private async void btnPickCachedFileRemote_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdCachedFileUpdaterRemote.txt", CreationCollisionOption.ReplaceExisting);
            string textContent = "I am webabcd";

            await FileIO.WriteTextAsync(file, textContent);

            /*
             * 设置 CachedFile，即将文件关联到 CachedFileUpdater
             * SetUpdateInformation(IStorageFile file, string contentId, ReadActivationMode readMode, WriteActivationMode writeMode, CachedFileOptions options);
             *     file - 与 CachedFileUpdater 关联的文件
             *     contentId - 与 CachedFileUpdater 关联的文件标识
             */
            CachedFileUpdater.SetUpdateInformation(file, "cachedFileRemote", ReadActivationMode.NotNeeded, WriteActivationMode.AfterWrite, CachedFileOptions.RequireUpdateOnAccess);

            lblMsg.Text = "选择的文件: " + file.Name;
            AddFileResult result = _fileOpenPickerUI.AddFile("myFile", file);
        }
    }
}
