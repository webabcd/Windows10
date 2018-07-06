/*
 * 演示如何读写文本数据
 *     
 * FileIO - 用于读写 IStorageFile 对象的帮助类
 *     WriteTextAsync() - 将指定的文本数据写入到指定的文件
 *     AppendTextAsync() - 将指定的文本数据追加到指定的文件
 *     WriteLinesAsync() - 将指定的多段文本数据写入到指定的文件
 *     AppendLinesAsync() - 将指定的多段文本数据追加到指定的文件
 *     ReadTextAsync() - 获取指定的文件中的文本数据
 *     ReadLinesAsync() - 获取指定的文件中的文本数据，返回的是一行一行的数据
 */

using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class ReadWriteText : Page
    {
        public ReadWriteText()
        {
            this.InitializeComponent();
        }

        private async void btnWriteText_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下创建指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.CreateFileAsync("webabcdText.txt", CreationCollisionOption.ReplaceExisting);

            // 在指定的文件中写入指定的文本
            string textContent = "I am webabcd";
            await FileIO.WriteTextAsync(storageFile, textContent, Windows.Storage.Streams.UnicodeEncoding.Utf8); // 编码为 UnicodeEncoding.Utf8

            lblMsg.Text = "写入成功";
        }

        private async void btnReadText_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下获取指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.GetFileAsync("webabcdText.txt");

            if (storageFile != null)
            {
                // 获取指定的文件中的文本内容
                string textContent = await FileIO.ReadTextAsync(storageFile, Windows.Storage.Streams.UnicodeEncoding.Utf8); // 编码为 UnicodeEncoding.Utf8
                lblMsg.Text = "读取结果：" + textContent;
            }
        }
    }
}
