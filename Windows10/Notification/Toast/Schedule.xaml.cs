/*
 * 演示如何按计划显示 toast 通知（在指定的时间显示指定的 toast 通知）
 * 
 * ScheduledToastNotification - 按计划显示 Toast 通知
 *     Content - Toast 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     DeliveryTime - 显示 Toast 通知的时间，只读，其需要在构造函数中指定
 *     SnoozeInterval - 循环显示 Toast 通知的间隔时长（60 秒 - 60 分之间），只读，其需要在构造函数中指定（经测试，此逻辑无效）
 *     MaximumSnoozeCount - 循环的最大次数（1 - 5 次），只读，其需要在构造函数中指定（经测试，此逻辑无效）
 *     Id - ScheduledToastNotification 的标识
 *     Group, Tag - 用于标识 toast 对象（前 16 个字符相同则认为相同）
 *         同 group 且同 tag 则视为同一 toast，即新 toast 会更新旧 toast
 *         系统会将所用未指定 group 的 toast 视为同 group
 *         系统会将所用未指定 tag 的 toast 视为不同 tag
 *     SuppressPopup - 默认值为 false
 *         false - 弹出 toast 通知，并放置于操作中心
 *         true - 不弹出 toast 通知，仅放置于操作中心
 *     
 * ToastNotifier - toast 通知器
 *     AddToSchedule() - 将指定的 ScheduledToastNotification 对象添加到计划列表
 *     RemoveFromSchedule() - 从计划列表中移除指定的 ScheduledToastNotification 对象
 *     GetScheduledToastNotifications() - 获取当前 app 的全部 ScheduledToastNotification 对象列表
 */

using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Toast
{
    public sealed partial class Schedule : Page
    {
        public Schedule()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowScheduledToasts();
        }

        // 添加指定的 ScheduledToastNotification 到计划列表中
        private void btnAddScheduledToast_Click(object sender, RoutedEventArgs e)
        {
            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Schedule-Arguments'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content {DateTime.Now.ToString("mm:ss")}</text>
                        </binding>
                    </visual>
                </toast>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            // 实例化 ScheduledToastNotification 对象（15 秒后显示此 Toast 通知，然后每隔 60 秒再显示一次 Toast，循环显示 5 次）
            // 但是经测试，只会显示一次，无法循环显示，不知道为什么
            // ScheduledToastNotification toastNotification = new ScheduledToastNotification(toastDoc, DateTime.Now.AddSeconds(15), TimeSpan.FromSeconds(60), 5);

            // 实例化 ScheduledToastNotification 对象（15 秒后显示此 Toast 通知）
            ScheduledToastNotification toastNotification = new ScheduledToastNotification(toastDoc, DateTime.Now.AddSeconds(15));

            toastNotification.Id = new Random().Next(100000, 1000000).ToString();
            toastNotification.Tag = toastDoc.GetElementsByTagName("text")[1].InnerText;

            // 将指定的 ScheduledToastNotification 添加进计划列表
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.AddToSchedule(toastNotification);

            ShowScheduledToasts();
        }
        
        // 删除指定的 ScheduledToastNotification 对象
        private void btnRemoveScheduledToast_Click(object sender, RoutedEventArgs e)
        {
            string notificationId = (string)(sender as FrameworkElement).Tag;

            // 获取当前 app 的全部 ScheduledToastNotification 对象列表
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> notifications = toastNotifier.GetScheduledToastNotifications();

            int notificationCount = notifications.Count;
            for (int i = 0; i < notificationCount; i++)
            {
                if (notifications[i].Id == notificationId)
                {
                    // 从计划列表中移除指定的 ScheduledToastNotification 对象
                    toastNotifier.RemoveFromSchedule(notifications[i]);
                    break;
                }
            }

            ShowScheduledToasts();
        }

        // 显示当前 app 的全部 ScheduledToastNotification 列表
        private void ShowScheduledToasts()
        {
            // 获取当前 app 的全部 ScheduledToastNotification 对象列表
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> notifications = toastNotifier.GetScheduledToastNotifications();

            listBox.ItemsSource = notifications;
        }
    }
}
