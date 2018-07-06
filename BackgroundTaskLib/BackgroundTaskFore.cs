/*
 * 后台任务，用于演示如何在前台程序通过 api 激活后台任务
 */

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BackgroundTaskLib
{
    // 实现 IBackgroundTask 接口，其只有一个方法，即 Run()
    public sealed class BackgroundTaskFore : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 后台任务在执行中被终止执行时所触发的事件
            taskInstance.Canceled += taskInstance_Canceled;

            // 异步操作
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // 指定后台任务的进度
                taskInstance.Progress = 0;
                // taskInstance.InstanceId - 后台任务实例的唯一标识，由系统生成，与前台的 IBackgroundTaskRegistration.TaskId 一致

                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdBackgroundTask\fore.txt", CreationCollisionOption.ReplaceExisting);
                for (uint progress = 10; progress <= 100; progress += 10)
                {
                    await Task.Delay(1000);

                    // 更新后台任务的进度（会通知给前台）
                    taskInstance.Progress = progress;

                    // 写入相关数据到指定的文件
                    await FileIO.AppendTextAsync(file, "progress: " + progress.ToString() + ", currentTime: " + DateTime.Now.ToString() + Environment.NewLine);
                }
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }

        void taskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            /*
             * BackgroundTaskCancellationReason - 后台任务在执行中被终止执行的原因
             *     Abort - 前台 app 调用了 IBackgroundTaskRegistration.Unregister(true)
             *     Terminating - 因为系统策略，而被终止
             *     LoggingOff - 因为用户注销系统而被取消
             *     ServicingUpdate - 因为 app 更新而被取消
             *     ... - 还有好多，参见文档吧
             */
        }
    }
}
