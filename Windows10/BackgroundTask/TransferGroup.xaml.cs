/*
 * 演示后台下载任务的分组，以及如何设置组内任务是并行执行还是串行执行，以及组任务全部完成后如何 toast 或 tile 通知）
 * 
 * BackgroundTransferGroup - 后台下载任务的分组对象
 *     static BackgroundTransferGroup CreateGroup(string name) - 创建指定分组标识的 BackgroundTransferGroup 对象
 *     Name - 分组标识（只读）
 *     TransferBehavior - 组内下载任务的执行方式，BackgroundTransferBehavior 枚举
 *         Parallel - 并行
 *         Serialized - 串行
 * 
 * BackgroundDownloader - 后台下载任务管理器
 *     TransferGroup - 设置或获取分组对象（BackgroundTransferGroup 类型）
 *     static GetCurrentDownloadsForTransferGroupAsync(BackgroundTransferGroup group) - 获取指定组的所有下载任务
 *     
 * DownloadOperation - 下载任务对象
 *     TransferGroup - 获取此下载任务的分组对象（BackgroundTransferGroup 类型）
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Windows10.BackgroundTask
{
    public sealed partial class TransferGroup : Page
    {
        // 用于后台任务的分组（通过组名标识后台任务）
        private BackgroundTransferGroup _group = BackgroundTransferGroup.CreateGroup("my_group");

        // 下载任务的集合
        private ObservableCollection<TransferModel> _transfers = new ObservableCollection<TransferModel>();

        // 所有下载任务的关联的 CancellationTokenSource 对象
        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public TransferGroup()
        {
            this.InitializeComponent();

            Init();
        }

        private async void Init()
        {
            // 指定组内任务并行执行
            _group.TransferBehavior = BackgroundTransferBehavior.Parallel;

            listView.ItemsSource = _transfers;

            // 加载指定组的下载任务
            await LoadDownloadAsync();
        }

        // 加载指定组的下载任务
        private async Task LoadDownloadAsync()
        {
            IReadOnlyList<DownloadOperation> downloads = null;
            try
            {
                // 获取指定组的下载任务
                downloads = await BackgroundDownloader.GetCurrentDownloadsForTransferGroupAsync(_group);
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                return;
            }

            if (downloads.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (DownloadOperation download in downloads)
                {
                    // 监视指定的后台下载任务
                    tasks.Add(HandleDownloadAsync(download, false));
                }

                await Task.WhenAll(tasks);
            }
        }

        // 新增一组（3 个）下载任务
        private async void btnAddDownload_Click(object sender, RoutedEventArgs e)
        {
            BackgroundDownloader backgroundDownloader = new BackgroundDownloader();
            // 指定分组
            backgroundDownloader.TransferGroup = _group;
            // 组任务全部成功后弹出指定的 toast 通知（类似的还有 SuccessTileNotification, FailureToastNotification, FailureTileNotification）
            backgroundDownloader.SuccessToastNotification = GetToastNotification(_group.Name);

            List<DownloadOperation> downloads = new List<DownloadOperation>();
            for (int i = 0; i < 3; i++)
            {
                Uri sourceUri = new Uri("http://files.cnblogs.com/webabcd/Windows10.rar", UriKind.Absolute);

                StorageFile destinationFile;
                try
                {
                    // 保存的目标地址（别忘了在 Package.appxmanifest 中配置好 <Capability Name="documentsLibrary" /> 和 .rar 类型文件的关联）
                    StorageFolder storageFolder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.DocumentsLibrary);
                    destinationFile = await storageFolder.CreateFileAsync("Windows10.rar", CreationCollisionOption.GenerateUniqueName);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.ToString());
                    return;
                }
                // 创建一个后台下载任务
                DownloadOperation download = backgroundDownloader.CreateDownload(sourceUri, destinationFile);

                downloads.Add(download);
            }

            // 处理并监视组内的后台下载任务
            Task[] tasks = new Task[downloads.Count];
            for (int i = 0; i < downloads.Count; i++)
            {
                tasks[i] = HandleDownloadAsync(downloads[i], true);
            }

            await Task.WhenAll(tasks);

        }

        /// <summary>
        /// 处理并监视组内的后台下载任务
        /// </summary>
        /// <param name="download">后台下载任务</param>
        /// <param name="isNew">是否是新增的任务</param>
        private async Task HandleDownloadAsync(DownloadOperation download, bool isNew)
        {
            try
            {
                // 构造显示用的相关数据
                TransferModel transfer = new TransferModel();
                transfer.DownloadOperation = download;
                transfer.Source = download.RequestedUri.ToString();
                transfer.Destination = download.ResultFile.Path;
                transfer.Progress = download.Progress.Status.ToString() + ": 0 / 0";

                _transfers.Add(transfer);

                WriteLine("Task Count: " + _transfers.Count.ToString());

                // 当下载进度发生变化时的回调函数
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);

                if (isNew)
                    await download.StartAsync().AsTask(_cancelToken.Token, progressCallback); // 启动一个后台下载任务
                else
                    await download.AttachAsync().AsTask(_cancelToken.Token, progressCallback); // 监视已存在的后台下载任务

                // 下载完成后获取服务端的响应信息
                ResponseInformation response = download.GetResponseInformation();
                WriteLine("Completed: " + response.ActualUri + ", HttpStatusCode: " + response.StatusCode.ToString());
            }
            catch (TaskCanceledException) // 调用 CancellationTokenSource.Cancel() 后会抛出此异常
            {
                WriteLine("Canceled: " + download.Guid);
            }
            catch (Exception ex)
            {
                // 将异常转换为 WebErrorStatus 枚举，如果获取到的是 WebErrorStatus.Unknown 则说明此次异常不是涉及 web 的异常
                WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);

                WriteLine(ex.ToString());
            }
            finally
            {
                _transfers.Remove(_transfers.First(p => p.DownloadOperation == download));
            }
        }

        // 进度发生变化时，更新 TransferModel 的 Progress
        private void DownloadProgress(DownloadOperation download)
        {
            TransferModel transfer = _transfers.First(p => p.DownloadOperation == download);
            transfer.Progress = download.Progress.Status.ToString() + ": " + download.Progress.BytesReceived.ToString("#,0") + " / " + download.Progress.TotalBytesToReceive.ToString("#,0");
        }

        // 取消全部后台下载任务
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelToken.Cancel();
            _cancelToken.Dispose();

            _cancelToken = new CancellationTokenSource();
        }

        // 向 lblMsg 中追加一行文本
        private void WriteLine(string message)
        {
            lblMsg.Text += message;
            lblMsg.Text += Environment.NewLine;

            scrollViewer.ChangeView(0, scrollViewer.ScrollableHeight, 1f);
        }

        private ToastNotification GetToastNotification(string groupName)
        {
            string toastXml = $@"
                <toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>组 {groupName} 中的下载任务全部完成了</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            return new ToastNotification(toastDoc);
        }
    }
}
