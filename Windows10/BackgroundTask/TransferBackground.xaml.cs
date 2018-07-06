/*
 * 演示后台下载任务的分组，以及组任务全部完成后如何触发后台任务
 * 
 * BackgroundTransferCompletionGroup - 分组对象（用于实现“组任务全部完成后触发后台任务”）
 *     Enable() - 启用“组任务全部完成后触发后台任务”的功能
 *     IsEnabled - 是否启用了“组任务全部完成后触发后台任务”的功能（只读）
 *     Trigger - “组任务全部完成后触发后台任务”的触发器
 *     
 * BackgroundDownloader - 后台下载任务管理器
 *     BackgroundDownloader(BackgroundTransferCompletionGroup completionGroup) - 通过指定的 BackgroundTransferCompletionGroup 对象实例化 BackgroundDownloader 对象
 *     CompletionGroup - 获取关联的 BackgroundTransferCompletionGroup 对象
 *     
 *     
 * 注：需要引用后台任务项目，相关代码参见 BackgroundTaskLib/BackgroundTaskTransfer.cs
 */

using BackgroundTaskLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Web;

namespace Windows10.BackgroundTask
{
    public sealed partial class TransferBackground : Page
    {
        // 后台任务是否已在系统中注册
        private bool _taskRegistered = false;

        // 下载任务的集合
        private ObservableCollection<TransferModel> _transfers = new ObservableCollection<TransferModel>();

        // 所有下载任务的关联的 CancellationTokenSource 对象
        private CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public TransferBackground()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 遍历所有已注册的后台任务
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTaskTransfer.TaskName)
                {
                    _taskRegistered = true;
                    break;
                }
            }

            UpdateUI();
        }

        // 新增一组（3 个）下载任务，并注册指定的后台任务
        private async void btnAddDownloadAndRegister_Click(object sender, RoutedEventArgs e)
        {
            // 注册指定的后台任务，并返回与此后台任务相关联的 BackgroundDownloader 对象
            BackgroundDownloader downloader = BackgroundTaskTransfer.RegisterBackgroundTaskAndReturnBackgrounDownloader();
            _taskRegistered = true;
            UpdateUI();

            List<DownloadOperation> downloads = new List<DownloadOperation>();
            for (int i = 0; i < 3; i++)
            {
                Uri uri = new Uri("http://files.cnblogs.com/webabcd/Windows10.rar");

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
                DownloadOperation download = downloader.CreateDownload(uri, destinationFile);

                downloads.Add(download);
            }

            // 启用“组任务全部完成后触发后台任务”的功能
            downloader.CompletionGroup.Enable();

            WriteLine("用于完成后触发后台任务的一组下载任务创建完成了，相关的后台任务也注册了");

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

        // 向 lblMsg 中追加一行文本
        private void WriteLine(string message)
        {
            var ignore = lblMsg.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text += message;
                lblMsg.Text += Environment.NewLine;

                scrollViewer.ChangeView(0, scrollViewer.ScrollableHeight, 1f);
            });
        }

        // 取消下载任务并注销指定的后台任务
        private void btnRemoveDownloadAndUnregister_Click(object sender, RoutedEventArgs e)
        {
            // 取消下载任务
            _cancelToken.Cancel();
            _cancelToken.Dispose();
            _cancelToken = new CancellationTokenSource();

            // 注销指定的后台任务
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BackgroundTaskTransfer.TaskName)
                {
                    // 从系统中注销指定的后台任务。唯一一个参数代表如果当前后台任务正在运行中，是否需要将其取消
                    task.Value.Unregister(true);
                    break;
                }
            }
            _taskRegistered = false;
            UpdateUI();
        }

        private async void UpdateUI()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                btnAddDownloadAndRegister.IsEnabled = !_taskRegistered;
                btnRemoveDownloadAndUnregister.IsEnabled = _taskRegistered;
            });
        }
    }
}
