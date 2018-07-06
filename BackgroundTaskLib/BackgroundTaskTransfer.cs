/*
 * 后台任务，用于演示指定的一组后台下载任务全部完成后如何触发此后台任务
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
 * 注：需要在 Package.appxmanifest 添加“后台任务”声明，支持的任务类型选择“系统事件”，并指定 EntryPoint（后台任务的类全名），类似如下：
 * <Extension Category="windows.backgroundTasks" EntryPoint="BackgroundTaskLib.BackgroundTaskTransfer">
 *   <BackgroundTasks>
 *     <Task Type="systemEvent" />
 *   </BackgroundTasks>
 * </Extension>
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Notifications;

namespace BackgroundTaskLib
{
    public sealed class BackgroundTaskTransfer : IBackgroundTask
    {
        // 所注册的后台任务的名称
        public static string TaskName { get; set; } = "Transfer";
        // 所注册的后台任务的 EntryPoint，即后台任务的类全名
        public static string TaskEntryPoint { get; set; } = "BackgroundTaskLib.BackgroundTaskTransfer";

        // 实现 IBackgroundTask 接口，其只有一个方法，即 Run()
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // 获取 BackgroundTransferCompletionGroupTriggerDetails 对象，如果不是 null 则说明是 BackgroundTransferCompletionGroup.Trigger 触发的
            BackgroundTransferCompletionGroupTriggerDetails details = taskInstance.TriggerDetails as BackgroundTransferCompletionGroupTriggerDetails;
            if (details == null)
            {
                return;
            }

            // 获取下载任务列表
            List<DownloadOperation> failedDownloads = new List<DownloadOperation>();
            int successTotal = 0;
            foreach (DownloadOperation download in details.Downloads)
            {
                if (IsFailed(download))
                {
                    // 保存失败的下载任务列表，稍后会重试
                    failedDownloads.Add(download);
                }
                else
                {
                    successTotal++;
                }
            }

            if (failedDownloads.Count > 0)
            {
                // 重新下载失败的任务
                RetryDownloads(failedDownloads);
            }

            // 此后台任务执行完毕，弹出指定的 toast 通知
            ShowToast(successTotal, failedDownloads.Count);
        }

        // 判断指定的下载任务是否失败了
        private bool IsFailed(DownloadOperation download)
        {
            BackgroundTransferStatus status = download.Progress.Status;
            if (status == BackgroundTransferStatus.Error || status == BackgroundTransferStatus.Canceled)
            {
                return true;
            }

            ResponseInformation response = download.GetResponseInformation();
            if (response.StatusCode != 200)
            {
                return true;
            }

            return false;
        }

        // 重新下载指定的任务
        private void RetryDownloads(IEnumerable<DownloadOperation> downloads)
        {
            // 注册指定的后台任务，并返回与此后台任务相关联的 BackgroundDownloader 对象
            BackgroundDownloader downloader = BackgroundTaskTransfer.RegisterBackgroundTaskAndReturnBackgrounDownloader();

            foreach (DownloadOperation download in downloads)
            {
                // 创建并启动后台任务
                DownloadOperation downloadNew = downloader.CreateDownload(download.RequestedUri, download.ResultFile);
                Task<DownloadOperation> startTask = downloadNew.StartAsync().AsTask();
            }

            // 启用“组任务全部完成后触发后台任务”的功能
            downloader.CompletionGroup.Enable();
        }

        // 注册指定的后台任务，并返回与此后台任务相关联的 BackgroundDownloader 对象
        // 后台任务的注册一般是在前台代码中写的，但是由于本例还有一个后台任务执行完成后重新下载失败任务的功能，所以这部分代码写在后台代码中就很合理了
        public static BackgroundDownloader RegisterBackgroundTaskAndReturnBackgrounDownloader()
        {
            // 根据指定 BackgroundTransferCompletionGroup 对象创建一个 BackgroundDownloader 对象
            BackgroundTransferCompletionGroup completionGroup = new BackgroundTransferCompletionGroup();
            BackgroundDownloader downloader = new BackgroundDownloader(completionGroup);

            // 注册一个后台任务，并指定触发器为 BackgroundTransferCompletionGroup.Trigger
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = BackgroundTaskTransfer.TaskName;
            builder.TaskEntryPoint = BackgroundTaskTransfer.TaskEntryPoint;
            builder.SetTrigger(completionGroup.Trigger);
            BackgroundTaskRegistration task = builder.Register();

            return downloader;
        }

        // 后台任务执行完成后弹出 toast 通知
        private void ShowToast(int successTotal, int failureTotal)
        {
            string toastXml = $@"
                <toast activationType='foreground'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>下载任务成功数: {successTotal}</text>
                            <text>下载任务失败数: {failureTotal}</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastDoc));
        }
    }
}
