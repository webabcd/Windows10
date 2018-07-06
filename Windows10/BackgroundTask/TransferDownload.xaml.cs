/*
 * 演示 uwp 的后台下载任务
 * 
 * BackgroundDownloader - 后台下载任务管理器
 *     CreateDownload(Uri uri, IStorageFile resultFile) - 创建一个下载任务，返回 DownloadOperation 对象
 *     SetRequestHeader(string headerName, string headerValue) - 设置 http 请求头
 *     Method - 用于下载的 http method（默认 get）
 *     static GetCurrentDownloadsAsync() - 获取当前 app 的未与组关联的所有下载任务
 *     CostPolicy - 下载的成本策略，BackgroundTransferCostPolicy 枚举
 *         Default - 允许在高成本（比如 4G）网络上传输（默认值）
 *         UnrestrictedOnly - 不允许在高成本（比如 4G）网络上传输
 *         Always - 无论如何均可传输，即使在漫游时
 *     ServerCredential - 与服务端通信时的凭据
 *     ProxyCredential - 使用代理时的身份凭据
 *     SuccessToastNotification, SuccessTileNotification, FailureToastNotification, FailureTileNotification - 下载任务成功或失败后的 toast 或 tile 通知
 *     
 * DownloadOperation - 下载任务对象
 *     Guid - 获取此下载任务的标识
 *     CostPolicy - 下载的成本策略，BackgroundTransferCostPolicy 枚举
 *     RequestedUri - 下载的源 URI
 *     ResultFile - 下载的目标文件
 *     Method - 获取用于下载的 http method（get, post 之类的）
 *     GetResponseInformation() - 下载完成后获取到的服务端响应信息，返回 ResponseInformation 对象
 *         ActualUri - 下载源的真实 URI
 *         Headers - 服务端响应的 HTTP 头
 *         StatusCode - 服务端响应的状态码
 *     Pause() - 暂停此下载任务
 *     Resume() - 继续此下载任务
 *     StartAsync() - 新增一个下载任务，返回 IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> 对象
 *     AttachAsync() - 监视已存在的下载任务，返回 IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> 对象
 *     Progress - 获取下载进度，返回 BackgroundDownloadProgress 对象
 *     Priority - 下载的优先级，BackgroundTransferPriority 枚举
 *         Default 或 High
 *     
 * BackgroundDownloadProgress - 后台下载任务的下载进度对象
 *     BytesReceived - 已下载的字节数
 *     TotalBytesToReceive - 总共需要下载的字节数，未知则为 0 
 *     Status - 下载状态，BackgroundTransferStatus 枚举
 *         Idle, Running, PausedByApplication, PausedCostedNetwork, PausedNoNetwork, Completed, Canceled, Error
 *     HasResponseChanged - 服务端响应了则为 true
 *     HasRestarted - 当下载连接断掉后，系统会通过 http range 头向服务端请求断点续传，如果服务端不支持断点续传则需要重新下载，此种情况则为 true
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
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Windows10.BackgroundTask
{
    public sealed partial class TransferDownload : Page
    {
        // 下载任务的集合
        private ObservableCollection<TransferModel> _transfers = new ObservableCollection<TransferModel>();

        // 所有下载任务的关联的 CancellationTokenSource 对象
        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public TransferDownload()
        {
            this.InitializeComponent();

            Init();
        }

        private async void Init()
        {
            listView.ItemsSource = _transfers;

            // 加载存在的下载任务
            await LoadDownloadAsync();
        }

        // 加载存在的下载任务
        private async Task LoadDownloadAsync()
        {
            IReadOnlyList<DownloadOperation> downloads = null;
            try
            {
                // 获取存在的下载任务
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
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

        // 新增一个下载任务
        private async void btnAddDownload_Click(object sender, RoutedEventArgs e)
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
            
            BackgroundDownloader backgroundDownloader = new BackgroundDownloader();
            // 任务成功后弹出指定的 toast 通知（类似的还有 SuccessTileNotification, FailureToastNotification, FailureTileNotification）
            backgroundDownloader.SuccessToastNotification = GetToastNotification(sourceUri);
            // 创建一个后台下载任务
            DownloadOperation download = backgroundDownloader.CreateDownload(sourceUri, destinationFile);

            // 处理并监视指定的后台下载任务
            await HandleDownloadAsync(download, true);
        }

        /// <summary>
        /// 处理并监视指定的后台下载任务
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

        // 暂停全部后台下载任务
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            WriteLine("All Paused");
            foreach (TransferModel transfer in _transfers)
            {
                transfer.DownloadOperation.Pause();
            }
        }

        // 继续全部后台下载任务
        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            WriteLine("All Resumed");
            foreach (TransferModel transfer in _transfers)
            {
                transfer.DownloadOperation.Resume();
            }
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
        
        // 构造指定的 toast 通知
        private ToastNotification GetToastNotification(Uri sourceUri)
        {
            string toastXml = $@"
                <toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>下载任务成功完成</text>
                            <text>{sourceUri}</text>
                        </binding>
                    </visual>
                </toast>";
            
            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            return new ToastNotification(toastDoc);
        }
    }
}
