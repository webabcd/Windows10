/*
 * 演示如何读写二进制数据
 * 
 * FileIO - 用于读写 IStorageFile 对象的帮助类
 *     WriteBufferAsync() - 将指定的二进制数据写入指定的文件
 *     ReadBufferAsync() - 获取指定的文件中的二进制数据
 *     
 * IBuffer - 字节数组
 */

using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class ReadWriteBinary : Page
    {
        public ReadWriteBinary()
        {
            this.InitializeComponent();
        }

        private async void btnWriteBinary_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下创建指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.CreateFileAsync("webabcdBinary.txt", CreationCollisionOption.ReplaceExisting);

            // 将字符串转换成二进制数据，并保存到指定文件
            string textContent = "I am webabcd";
            IBuffer buffer = ConverterHelper.String2Buffer(textContent);
            await FileIO.WriteBufferAsync(storageFile, buffer);

            lblMsg.Text = "写入成功";
        }

        private async void btnReadBinary_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下获取指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.GetFileAsync("webabcdBinary.txt");

            if (storageFile != null)
            {
                // 获取指定文件中的二进制数据，将其转换成字符串并显示
                IBuffer buffer = await FileIO.ReadBufferAsync(storageFile);
                string textContent = ConverterHelper.Buffer2String(buffer);

                lblMsg.Text = "读取结果：" + textContent;
            }
        }
    }
}
