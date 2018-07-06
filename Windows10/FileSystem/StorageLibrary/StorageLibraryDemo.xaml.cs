/*
 * 演示如何 添加/删除 “库”所包含的文件夹
 * 
 * StorageLibrary - 用于“库”管理
 *     GetLibraryForUserAsync(User user, KnownLibraryId libraryId) - 获取指定用户的指定“库”，返回 StorageLibrary 类型的对象
 *         user - 指定用户，传 null 则为当前用户（关于 User 相关请参见 /UserAndAccount/UserInfo.xaml）
 *         libraryId - 指定库目录，一个 KnownLibraryId 类型的枚举值（Music, Pictures, Videos, Documents）
 *     Folders - 当前库所包含的文件夹们
 *     SaveFolder - 当前库的默认文件夹
 *     RequestAddFolderAsync() - 添加文件夹到当前库
 *     RequestRemoveFolderAsync() - 从当前库移除指定的文件夹
 *     DefinitionChanged -  当前库所包含的文件夹发生变化时触发的事件
 *     
 *     
 * 注：根据需要请在 Package.appxmanifest 中配置  <Capability Name="picturesLibrary" />, <Capability Name="videosLibrary" />,  <Capability Name="musicLibrary" />, <Capability Name="documentsLibrary" />
 */

using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.FileSystem.StorageLibrary
{
    public sealed partial class StorageLibraryDemo : Page
    {
        // 临时保存添加进图片库的文件夹
        private List<StorageFolder> _addedFloders = new List<StorageFolder>();

        public StorageLibraryDemo()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 注意：要想访问图片库，别忘了增加 <Capability Name="picturesLibrary" />

            // 获取图片库的 StorageLibrary 对象
            Windows.Storage.StorageLibrary picturesLibrary = await Windows.Storage.StorageLibrary.GetLibraryForUserAsync(null, KnownLibraryId.Pictures);

            // 当前库所包含的文件夹增多或减少时
            picturesLibrary.DefinitionChanged += async (Windows.Storage.StorageLibrary innerSender, object innerEvent) =>
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    lblMsg.Text = "图片库所包含的文件夹如下：";
                    foreach (StorageFolder folder in picturesLibrary.Folders) // 当前库所包含的全部文件夹
                    {
                        lblMsg.Text += Environment.NewLine;
                        lblMsg.Text += folder.Path;
                    }
                });
            };

            base.OnNavigatedTo(e);
        }

        // 增加一个文件夹引用到图片库
        private async void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageLibrary picturesLibrary = await Windows.Storage.StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);

            // 弹出文件夹选择器，以选择需要添加到图片库的文件夹
            StorageFolder addedFolder = await picturesLibrary.RequestAddFolderAsync();
            if (addedFolder != null)
            {
                // 添加成功
                _addedFloders.Add(addedFolder);
            }
            else
            {

            }
        }

        // 从图片库移除之前添加的全部文件夹引用
        private async void btnRemoveFolder_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageLibrary picturesLibrary = await Windows.Storage.StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);

            foreach (StorageFolder folder in _addedFloders)
            {
                // 从图片库移除指定的文件夹引用
                if (await picturesLibrary.RequestRemoveFolderAsync(folder))
                {
                    // 移除成功
                }
                else
                {

                }
            }
        }
    }
}
