/*
 * 演示如何调用 CachedFileUpdater（缓存文件更新程序）
 * 
 * 流程：
 * 1、单击“打开 Provider 提供的 CachedFile”按钮，以弹出打开文件对话框
 * 2、在弹出的对话框中选择 CachedFileUpdaterProvider，以打开 CachedFileUpdaterProvider 项目中的 MyOpenPicker.xaml
 * 3、在 provider 中单击“提供一个 CachedFile”按钮，以打开一个文件，同时将此文件关联到 CachedFileUpdater
 * 4、如果在 provider 选择了“提供一个由 Local 更新的 CachedFile”则转到（5）；如果在 provider 选择了“提供一个由 Remote 更新的 CachedFile”则转到（6）
 * 
 * 5、单击“由 CachedFileUpdater 更新文件”按钮，激活 CachedFileUpdater，获取由 CachedFileUpdater 修改后的文件（CachedFileUpdater 的 Local 方式）
 * 6、单击“由 app 更新文件”按钮，会在 app 端更指定的 CachedFile，需要的话可以激活 CachedFileUpdater 做一些别的处理（CachedFileUpdater 的 Remote 方式）
 * 
 * 
 * 注：本例中 app 代表调用方，provider 代表缓存文件提供方，CachedFileUpdater 代表缓存文件更新程序
 */

using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Picker
{
    public sealed partial class CachedFileUpdaterDemo : Page
    {
        private string _cachedFileToken;

        public CachedFileUpdaterDemo()
        {
            this.InitializeComponent();
        }

        private async void btnGetCachedFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".txt");

            // 弹出打开文件对话框后，选择 CachedFileUpdaterProvider，以获取 CachedFile
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                _cachedFileToken = StorageApplicationPermissions.FutureAccessList.Add(file);
                string fileContent = await FileIO.ReadTextAsync(file);

                lblMsg.Text = "文件名: " + file.Name;
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += "文件内容: " + fileContent;
            }
            else
            {
                lblMsg.Text = "取消了";
            }
        }

        // 由 CachedFileUpdater 更新文件（CachedFileUpdater 的 Local 方式）
        private async void btnReadCachedFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_cachedFileToken))
            {
                try
                {
                    StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_cachedFileToken);
                    // 获取的文件是 CachedFile，且是 CachedFileUpdater 的 Local 方式
                    // 如此就会激活 CachedFileUpdater，由 CachedFileUpdater 更新文件并返回
                    string fileContent = await FileIO.ReadTextAsync(file);

                    lblMsg.Text = "文件名: " + file.Name;
                    lblMsg.Text += Environment.NewLine;
                    lblMsg.Text += "文件内容: " + fileContent;
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }

        // 由 app 更新文件（CachedFileUpdater 的 Remote 方式）
        private async void btnWriteCachedFile_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_cachedFileToken))
            {
                try
                {
                    StorageFile file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(_cachedFileToken);

                    // 开始异步更新操作（不需要激活 CachedFileUpdater 的话可以不走这一步）
                    CachedFileManager.DeferUpdates(file);

                    // 更新文件
                    await FileIO.AppendTextAsync(file, Environment.NewLine + "由 app 更新：" + DateTime.Now.ToString());

                    // 通知系统已完成异步操作（之前激活的 CachedFileUpdater 会返回一个 FileUpdateStatus）
                    // 更新的文件是 CachedFile，且是 CachedFileUpdater 的 Remote 方式，即 app 更新
                    // 更新后会激活 CachedFileUpdater，在 CachedFileUpdater 中可以拿到更新后的文件
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);

                    lblMsg.Text = status.ToString();
                    lblMsg.Text += Environment.NewLine;

                    if (status == FileUpdateStatus.Complete)
                    {
                        string fileContent = await FileIO.ReadTextAsync(file);

                        lblMsg.Text += "文件名: " + file.Name;
                        lblMsg.Text += Environment.NewLine;
                        lblMsg.Text += "文件内容: " + fileContent;
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.ToString();
                }
            }
        }
    }
}
