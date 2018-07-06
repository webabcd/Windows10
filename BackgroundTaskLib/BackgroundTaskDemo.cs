/*
 * 后台任务
 * 
 * 注：
 * 后台任务项目的输出类型需要设置为“Windows 运行时组件”，其会生成 .winmd 文件，winmd - Windows Metadata
 * 
 * 另：
 * 在用户设置的免打扰时间内，所有后台任务均暂停（来电和闹钟例外），免打扰时间过后，后台任务将随机在不同时间点启动
 */

using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace BackgroundTaskLib
{
    // 实现 IBackgroundTask 接口，其只有一个方法，即 Run()
    public sealed class BackgroundTaskDemo : IBackgroundTask
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
                // taskInstance.SuspendedCount - 由资源管理政策导致后台任务挂起的次数

                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdBackgroundTask\demo.txt", CreationCollisionOption.ReplaceExisting);
                for (uint progress = 10; progress <= 100; progress += 10)
                {
                    await Task.Delay(1000);

                    // 更新后台任务的进度（会通知给前台）
                    taskInstance.Progress = progress;

                    // 获取当前后台任务的开销量（Low, Medium, High）
                    // BackgroundWorkCostValue bwcv = Windows.ApplicationModel.Background.BackgroundWorkCost.CurrentBackgroundWorkCost;

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
            // 如果这里 5 秒内没有完成，则系统会终止该应用，并生成错误报告上传至 windows 商店的开发人员账户

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
