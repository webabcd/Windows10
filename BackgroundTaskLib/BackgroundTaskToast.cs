/*
 * 后台任务，用于演示如何通过 toast 激活后台任务 
 * 
 * ToastNotificationActionTriggerDetail - toast 触发器信息
 *     Argument - 由 toast 传递过来的参数
 *     UserInput - 由 toast 传递过来的输入框数据
 */

using System;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTaskLib
{
    public sealed class BackgroundTaskToast : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            try
            {
                // 获取 ToastNotificationActionTriggerDetail 对象
                ToastNotificationActionTriggerDetail toastDetail = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
                if (toastDetail != null)
                {
                    string result = "";

                    result = "argument: " + toastDetail.Argument;
                    result += Environment.NewLine;

                    foreach (string key in toastDetail.UserInput.Keys)
                    {
                        result += $"key:{key}, value:{toastDetail.UserInput[key]}";
                        result += Environment.NewLine;
                    }

                    // 将获取到的 toast 信息保存为文件
                    StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(@"webabcdBackgroundTask\toast.txt", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, result);
                }
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
