/*
 * 演示如何将本 app 沙盒内的文件共享给其他 app 使用
 * 
 * SharedStorageAccessManager.AddFile(IStorageFile file) - 指定文件设置为可共享，并返回 token
 * SharedStorageAccessManager.RedeemTokenForFileAsync(string token) - 在其他 app 中可以根据 token 获取 StorageFile 对象，并且可读可写
 * SharedStorageAccessManager.RemoveFile(string token) - 根据 token 将指定的可共享文件设置为不可共享
 * 
 * 注：
 * 1、一个 token 的有效期为 14 天
 * 2、一个 app 最多可以拥有 1000 个有效的 token
 * 3、一个 token 被访问一次后就会失效（文档是这么写的，但是实际测试发现并不会失效）
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.App2AppCommunication
{
    public sealed partial class SharedStorage : Page
    {
        private string _sharedToken;

        public SharedStorage()
        {
            this.InitializeComponent();
        }

        // 将沙盒内的指定文件设置为可共享，并生成 token
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdTest\sharedStorage.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, "I am webabcd: " + DateTime.Now.ToString());

            _sharedToken = SharedStorageAccessManager.AddFile(file);

            lblMsg.Text += $"文件 {file.Path} 已经被设置为可共享，其 token 值为 {_sharedToken}";
            lblMsg.Text += Environment.NewLine;
        }

        // 根据 token 获取文件
        // 为了演示方便，本例就在同一个 app 中演示了，实际上在其他 app 中也是可以根据此 token 获取文件的
        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile file = await SharedStorageAccessManager.RedeemTokenForFileAsync(_sharedToken);
                string textContent = await FileIO.ReadTextAsync(file);

                lblMsg.Text += $"token 值为 {_sharedToken} 的文件的内容为: {textContent}";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }

        // 根据 token 将沙盒内的可共享文件设置为不可共享
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SharedStorageAccessManager.RemoveFile(_sharedToken);

                lblMsg.Text += $"token 值为 {_sharedToken} 的文件取消共享了";
                lblMsg.Text += Environment.NewLine;
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }
        }
    }
}
