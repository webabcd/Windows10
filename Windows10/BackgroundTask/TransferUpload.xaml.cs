/*
 * 演示 uwp 的后台上传任务
 * 
 * BackgroundUploader - 后台上传任务管理器
 *     CreateUpload(Uri uri, IStorageFile sourceFile) - 创建一个上传任务，返回 UploadOperation 对象
 *     CreateUploadFromStreamAsync(Uri uri, IInputStream sourceStream) - 以流的方式创建一个上传任务
 *     CreateUploadAsync(Uri uri, IEnumerable<BackgroundTransferContentPart> parts) - 创建一个包含多个上传文件的上传任务
 *     SetRequestHeader(string headerName, string headerValue) - 设置 http 请求头
 *     Method - 用于上传的 http method（默认 post）
 *     static GetCurrentUploadsAsync() - 获取当前 app 的未与组关联的所有上传任务
 *     CostPolicy - 上传的成本策略，BackgroundTransferCostPolicy 枚举
 *         Default - 不允许在高成本（比如 4G）网络上传输
 *         UnrestrictedOnly - 允许在高成本（比如 4G）网络上传输
 *         Always - 无论如何均可传输，即使在漫游时
 *     ServerCredential - 与服务端通信时的凭据
 *     ProxyCredential - 使用代理时的身份凭据
 *     SuccessToastNotification, SuccessTileNotification, FailureToastNotification, FailureTileNotification - 上传任务成功或失败后的 toast 或 tile 通知
 *     static GetCurrentUploadsForTransferGroupAsync(BackgroundTransferGroup group) - 获取指定组的所有上传任务
 *     TransferGroup - 设置或获取分组对象（BackgroundTransferGroup 类型）
 *     BackgroundUploader(BackgroundTransferCompletionGroup completionGroup) - 通过指定的 BackgroundTransferCompletionGroup 对象实例化 BackgroundUploader 对象
 *     CompletionGroup - 获取关联的 BackgroundTransferCompletionGroup 对象
 *     
 * UploadOperation - 上传任务对象
 *     Guid - 获取此上传任务的标识
 *     CostPolicy - 上传的成本策略，BackgroundTransferCostPolicy 枚举
 *     RequestedUri - 上传任务所请求的服务端地址
 *     SourceFile - 需要上传的文件，如果是一次上传多个文件则此属性为 null
 *     Method - 获取用于上传的 http method（get, post 之类的）
 *     GetResponseInformation() - 上传完成后获取到的服务端响应信息，返回 ResponseInformation 对象
 *         ActualUri - 上传服务的真实 URI
 *         Headers - 服务端响应的 HTTP 头
 *         StatusCode - 服务端响应的状态码
 *     StartAsync() - 新增一个上传任务，返回 IAsyncOperationWithProgress<UploadOperation, UploadOperation> 对象
 *     AttachAsync() - 监视已存在的上传任务，返回 IAsyncOperationWithProgress<UploadOperation, UploadOperation> 对象
 *     Progress - 获取上传进度，返回 BackgroundUploadProgress 对象
 *     Priority - 上传的优先级，BackgroundTransferPriority 枚举
 *         Default 或 High
 *     TransferGroup - 获取此上传任务的分组对象（BackgroundTransferGroup 类型）
 *     
 * BackgroundUploadProgress - 后台上传任务的上传进度对象
 *     BytesSent - 已上传的字节数
 *     TotalBytesToSend - 总共需要上传的字节数
 *     BytesReceived - 已下载的字节数
 *     TotalBytesToReceive - 总共需要下载的字节数，未知则为 0 
 *     Status - 上传状态，BackgroundTransferStatus 枚举
 *         Idle, Running, PausedByApplication, PausedCostedNetwork, PausedNoNetwork, Completed, Canceled, Error
 *     HasResponseChanged - 服务端响应了则为 true
 *     HasRestarted - 当上传连接断掉后，系统会重新上传，此种情况则为 true
 *     
 * BackgroundTransferGroup - 后台上传任务的分组对象
 *     static BackgroundTransferGroup CreateGroup(string name) - 创建指定分组标识的 BackgroundTransferGroup 对象
 *     Name - 分组标识（只读）
 *     TransferBehavior - 组内上传任务的执行方式，BackgroundTransferBehavior 枚举
 *         Parallel - 并行
 *         Serialized - 串行
 * 
 * BackgroundTransferCompletionGroup - 分组对象（用于实现“组任务全部完成后触发后台任务”）
 *     Enable() - 启用“组任务全部完成后触发后台任务”的功能
 *     IsEnabled - 是否启用了“组任务全部完成后触发后台任务”的功能（只读）
 *     Trigger - “组任务全部完成后触发后台任务”的触发器
 *     
 * BackgroundTransferContentPart - 当一次上传多个文件时，将每个需要上传的文件构造成一个 BackgroundTransferContentPart 对象
 *     BackgroundTransferContentPart(string name, string fileName) - 通过一个标识和文件名称实例化 BackgroundTransferContentPart 对象
 *     SetFile(IStorageFile value) - 指定需要上传的文件
 *     
 * 
 * 注：关于上传任务的“任务分组，并行或串行执行，组完成后通知”和“任务分组，组完成后触发后台任务”的实现方式与下载任务是一样的，请参见下载任务的相关演示示例（TransferGroup.xaml.cs 和 TransferBackground.xaml.cs）
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web;

namespace Windows10.BackgroundTask
{
    public sealed partial class TransferUpload : Page
    {
        // 上传任务的集合
        private ObservableCollection<TransferModel> _transfers = new ObservableCollection<TransferModel>();

        // 所有上传任务的关联的 CancellationTokenSource 对象
        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public TransferUpload()
        {
            this.InitializeComponent();

            Init();
        }

        private async void Init()
        {
            listView.ItemsSource = _transfers;

            // 加载全部上传任务
            await LoadUploadAsync();
        }

        // 加载全部上传任务
        private async Task LoadUploadAsync()
        {
            IReadOnlyList<UploadOperation> uploads = null;
            try
            {
                // 获取所有后台上传任务
                uploads = await BackgroundUploader.GetCurrentUploadsAsync();
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                return;
            }

            if (uploads.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (UploadOperation upload in uploads)
                {
                    // 监视指定的后台上传任务
                    tasks.Add(HandleUploadAsync(upload, false));
                }

                await Task.WhenAll(tasks);
            }
        }

        // 新增一个上传任务（一次请求上传一个文件）
        private async void btnAddUpload_Click(object sender, RoutedEventArgs e)
        {
            // 上传服务的地址
            Uri serverUri = new Uri("http://localhost:44914/api/Upload", UriKind.Absolute);

            StorageFile sourceFile;
            try
            {
                // 需要上传的文件
                sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute));
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                return;
            }

            // 实例化 BackgroundUploader，并设置 http header
            BackgroundUploader backgroundUploader = new BackgroundUploader();
            backgroundUploader.SetRequestHeader("Filename", "hololens.jpg");

            // 任务成功后弹出指定的 toast 通知（类似的还有 SuccessTileNotification, FailureToastNotification, FailureTileNotification）
            backgroundUploader.SuccessToastNotification = GetToastNotification();

            // 创建一个后台上传任务，此任务包含一个上传文件
            UploadOperation upload = backgroundUploader.CreateUpload(serverUri, sourceFile);

            // 以流的方式创建一个后台上传任务
            // await backgroundUploader.CreateUploadFromStreamAsync(Uri uri, IInputStream sourceStream);

            // 处理并监视指定的后台上传任务
            await HandleUploadAsync(upload, true);
        }

        // 新增一个上传任务（一次请求上传多个文件）
        private async void btnAddMultiUpload_Click(object sender, RoutedEventArgs e)
        {
            // 上传服务的地址
            Uri serverUri = new Uri("http://localhost:44914/api/Upload", UriKind.Absolute);

            // 需要上传的文件源集合
            List<StorageFile> sourceFiles = new List<StorageFile>();
            for (int i = 0; i < 3; i++)
            {
                StorageFile sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute));
                sourceFiles.Add(sourceFile);
            }

            // 构造需要上传 BackgroundTransferContentPart 集合
            List<BackgroundTransferContentPart> contentParts = new List<BackgroundTransferContentPart>();
            for (int i = 0; i < sourceFiles.Count; i++)
            {
                BackgroundTransferContentPart contentPart = new BackgroundTransferContentPart("File" + i, sourceFiles[i].Name);
                contentPart.SetFile(sourceFiles[i]);
                contentParts.Add(contentPart);
            }
            
            BackgroundUploader backgroundUploader = new BackgroundUploader();

            // 任务成功后弹出指定的 toast 通知（类似的还有 SuccessTileNotification, FailureToastNotification, FailureTileNotification）
            backgroundUploader.SuccessToastNotification = GetToastNotification();

            // 创建一个后台上传任务，此任务包含多个上传文件
            UploadOperation upload = await backgroundUploader.CreateUploadAsync(serverUri, contentParts);

            // 处理并监视指定的后台上传任务
            await HandleUploadAsync(upload, true);
        }

        /// <summary>
        /// 处理并监视指定的后台上传任务
        /// </summary>
        /// <param name="upload">后台上传任务</param>
        /// <param name="isNew">是否是新增的任务</param>
        private async Task HandleUploadAsync(UploadOperation upload, bool isNew)
        {
            try
            {
                // 将 UploadOperation 附加到 TransferModel，以便上传进度可通知
                TransferModel transfer = new TransferModel();
                transfer.UploadOperation = upload;
                transfer.Source = "多个文件";
                transfer.Destination = upload.RequestedUri.ToString();
                transfer.Progress = upload.Progress.Status.ToString() + "0 / 0";

                _transfers.Add(transfer);

                WriteLine("Task Count: " + _transfers.Count.ToString());

                // 当上传进度发生变化时的回调函数
                Progress<UploadOperation> progressCallback = new Progress<UploadOperation>(UploadProgress);

                if (isNew)
                    await upload.StartAsync().AsTask(_cancelToken.Token, progressCallback); // 启动一个后台上传任务
                else
                    await upload.AttachAsync().AsTask(_cancelToken.Token, progressCallback); // 监视已存在的后台上传任务

                // 上传完成后获取服务端的响应信息
                ResponseInformation response = upload.GetResponseInformation();
                WriteLine("Completed: " + response.ActualUri + ", HttpStatusCode: " + response.StatusCode.ToString());
            }
            catch (TaskCanceledException) // 调用 CancellationTokenSource.Cancel() 后会抛出此异常
            {
                WriteLine("Canceled: " + upload.Guid);
            }
            catch (Exception ex)
            {
                // 将异常转换为 WebErrorStatus 枚举，如果获取到的是 WebErrorStatus.Unknown 则说明此次异常不是涉及 web 的异常
                WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);

                WriteLine(ex.ToString());
            }
            finally
            {
                _transfers.Remove(_transfers.First(p => p.UploadOperation == upload));
            }
        }

        // 进度发生变化时，更新 TransferModel 的 Progress
        private void UploadProgress(UploadOperation upload)
        {
            TransferModel transfer = _transfers.First(p => p.UploadOperation == upload);
            transfer.Progress = upload.Progress.Status.ToString() + ": " + upload.Progress.BytesSent.ToString("#,0") + " / " + upload.Progress.TotalBytesToSend.ToString("#,0");
        }

        // 取消全部后台上传任务
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
        private ToastNotification GetToastNotification()
        {
            string toastXml = $@"
                <toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>上传任务成功完成</text>
                        </binding>
                    </visual>
                </toast>";
            
            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            return new ToastNotification(toastDoc);
        }
    }
}
