/*
 * 演示如何读写流数据
 * 
 * IBuffer - 字节数组
 * 
 * IInputStream - 支持读取的流
 * IOutputStream - 支持写入的流
 * IRandomAccessStream - 支持读取和写入的流，其继承自 IInputStream 和 IOutputStream
 * 
 * DataReader - 数据读取器，用于从数据流中读取数据
 *     LoadAsync() - 从数据流中加载指定长度的数据到缓冲区
 *     ReadInt32(), ReadByte(), ReadString() 等 - 从缓冲区中读取数据
 * DataWriter - 数据写入器，用于将数据写入数据流
 *     WriteInt32(), WriteByte(), WriteString() 等 - 将数据写入缓冲区
 *     StoreAsync() - 将缓冲区中的数据保存到数据流
 * 
 * StorageStreamTransaction - 用于写数据流到文件的类（它写文件的方式是：先写临时文件，然后临时文件重命名）
 *     Stream - 数据流（只读）
 *     CommitAsync - 重命名临时文件   
 *     
 * StorageFile - 文件操作类
 *     public IAsyncOperation<IRandomAccessStream> OpenAsync(FileAccessMode accessMode) - 打开文件，返回 IRandomAccessStream 对象
 *     public IAsyncOperation<IRandomAccessStream> OpenAsync(FileAccessMode accessMode, StorageOpenOptions options) - 打开文件，返回 IRandomAccessStream 对象
 *         FileAccessMode.Read - 返回的流可读，不可写
 *         FileAccessMode.ReadWrite - 返回的流可读，可写
 *         StorageOpenOptions.AllowOnlyReaders - 其他调用者可读不可写
 *         StorageOpenOptions.AllowReadersAndWriters - 其他调用者可读可写
 *     public IAsyncOperation<StorageStreamTransaction> OpenTransactedWriteAsync() - 打开文件，返回 StorageStreamTransaction 对象
 *     public IAsyncOperation<StorageStreamTransaction> OpenTransactedWriteAsync(StorageOpenOptions options) - 打开文件，返回 StorageStreamTransaction 对象 
 */

using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.FileSystem
{
    public sealed partial class ReadWriteStream : Page
    {
        public ReadWriteStream()
        {
            this.InitializeComponent();
        }

        // Write Stream（通过 IRandomAccessStream 写）
        private async void btnWriteStream1_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下创建指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.CreateFileAsync("webabcdStream.txt", CreationCollisionOption.ReplaceExisting);

            string textContent = "I am webabcd(IRandomAccessStream)";
            if (storageFile != null)
            {
                using (IRandomAccessStream randomStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (DataWriter dataWriter = new DataWriter(randomStream))
                    {
                        // 将字符串写入数据流
                        dataWriter.WriteString(textContent);

                        // 将数据流写入文件
                        await dataWriter.StoreAsync();

                        lblMsg.Text = "写入成功";
                    }
                }
            }
        }

        // Write Stream（通过 StorageStreamTransaction 写）
        private async void btnWriteStream2_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下创建指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.CreateFileAsync("webabcdStream.txt", CreationCollisionOption.ReplaceExisting);

            string textContent = "I am webabcd(StorageStreamTransaction)";

            using (StorageStreamTransaction transaction = await storageFile.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                {
                    // 将字符串写入数据流
                    dataWriter.WriteString(textContent);

                    // 使用 StorageStreamTransaction 方式的话，调用 DataWriter 的 StoreAsync() 方法的作用是把数据写入临时文件
                    // 以本例为例，这个临时文件就是同目录下名为 webabcdStream.txt.~tmp 的文件。只要不调用 CommitAsync() 方法的话就会看到
                    // 返回值为写入数据的大小，需要通过此值更新 StorageStreamTransaction 中的 Stream 的大小
                    transaction.Stream.Size = await dataWriter.StoreAsync();

                    // 重命名临时文件
                    await transaction.CommitAsync();

                    lblMsg.Text = "写入成功";
                }
            }
        }

        private async void btnReadStream_Click(object sender, RoutedEventArgs e)
        {
            // 在指定的目录下获取指定的文件
            StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
            StorageFile storageFile = await storageFolder.GetFileAsync("webabcdStream.txt");

            if (storageFile != null)
            {
                using (IRandomAccessStream randomStream = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    using (DataReader dataReader = new DataReader(randomStream))
                    {
                        ulong size = randomStream.Size;
                        if (size <= uint.MaxValue)
                        {
                            // 从数据流中读取数据
                            uint numBytesLoaded = await dataReader.LoadAsync((uint)size);

                            // 将读取到的数据转换为字符串
                            string fileContent = dataReader.ReadString(numBytesLoaded);

                            lblMsg.Text = "读取结果：" + fileContent;
                        }
                    }
                }
            }
        }
    }
}
