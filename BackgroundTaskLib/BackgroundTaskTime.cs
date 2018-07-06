/*
 * 后台任务，用于演示如何定时激活后台任务
 */

using System;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BackgroundTaskLib
{
    // 实现 IBackgroundTask 接口，其只有一个方法，即 Run()
    public sealed class BackgroundTaskTime : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // 异步操作
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // 写入相关数据到文件
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdBackgroundTask\time.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.AppendTextAsync(file, "background task timeTrigger or maintenanceTrigger: " + DateTime.Now.ToString() + Environment.NewLine);

            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }
    }
}
