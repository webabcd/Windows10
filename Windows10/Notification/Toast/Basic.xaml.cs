/*
 * 本例用于演示 toast 的基础
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * ToastNotification - toast 通知
 *     Content - 一个 Windows.Data.Xml.Dom.XmlDocument 类型的对象（在构造函数中需要传递此对象），用于描述 toast 的 xml
 *     SuppressPopup - 默认值为 false
 *         false - 弹出 toast 通知，并放置于操作中心
 *         true - 不弹出 toast 通知，仅放置于操作中心
 *     ExpirationTime - 过期时间，超过这个时间就会从操作中心中移除
 *     ToastNotificationPriority - 优先级（Default 或 High）
 *     Group, Tag - 用于标识 toast 对象（前 16 个字符相同则认为相同）
 *         同 group 且同 tag 则视为同一 toast，即新 toast 会更新旧 toast
 *         系统会将所用未指定 group 的 toast 视为同 group
 *         系统会将所用未指定 tag 的 toast 视为不同 tag
 *     Activated - 通过 toast 激活 app 时触发的事件
 *     Dismissed - 弹出的 toast 通知 UI 消失时触发的事件
 *     Failed - 弹出 toast 通知时失败
 *     
 * ToastNotificationManager - toast 通知管理器
 *     CreateToastNotifier() - 创建 ToastNotifier 对象
 *     History - 获取 ToastNotificationHistory 对象
 *     
 * ToastNotifier - toast 通知器
 *     Show(ToastNotification notification) - 弹出指定的 toast 通知
 *     Hide(ToastNotification notification) - 移除指定的 toast 通知
 *     Setting - 获取系统的通知设置
 *         Enabled - 通知可被显示
 *         DisabledForApplication - 用户禁用了此应用程序的通知
 *         DisabledForUser - 用户禁用了此计算机此账户的所有通知
 *         DisabledByGroupPolicy - 管理员通过组策略禁止了此计算机上的所有通知
 *         DisabledByManifest - 应用程序未在 Package.appxmanifest 中设置“应用图标”（其实你要是不设置的话编译都不会通过）
 *         
 * ToastNotificationHistory - 本 app 的 toast 通知历史
 *     Clear() - 全部清除（从操作中心中移除）
 *     RemoveGroup() - 清除指定 group 的通知（从操作中心中移除）
 *     Remove() - 清除指定 tag 的通知或清除指定 tag 和 group 的通知（从操作中心中移除）
 *     GetHistory() - 获取历史数据，一个 ToastNotification 类型对象的集合（已被从操作中心中移除的是拿不到的）
 *     
 *     
 *     
 * 注：本例是通过 xml 来构造 toast 的，另外也可以通过 NuGet 的 Microsoft.Toolkit.Uwp.Notifications 来构造 toast（其用 c# 对 xml 做了封装）
 */

using System;
using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class Basic : Page
    {
        public Basic()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            // 用于描述 toast 通知的 xml 字符串
            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Basic-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 1 {DateTime.Now.ToString("mm:ss")}</text>
                        </binding>
                    </visual>
                </toast>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            // 实例化 ToastNotification 对象
            ToastNotification toast = new ToastNotification(toastDoc);
            // 系统会将所用未指定 group 的 toast 视为同 group
            // 同 group 且同 tag 则视为同一 toast，即新 toast 会更新旧 toast
            toast.Tag = "1";

            toast.Activated += Toast_Activated;
            toast.Dismissed += Toast_Dismissed;
            toast.Failed += Toast_Failed;

            // 弹出 toast 通知
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }

        // 弹出 toast 通知（设置 toast 的过期时间）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Basic-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 2 {DateTime.Now.ToString("mm:ss")}</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
            toast.ExpirationTime = expirationTime; // 30 秒后 toast 通知将从操作中心中移除

            // 系统会将所用未指定 group 的 toast 视为同 group
            // 系统会将所用未指定 tag 的 toast 视为不同 tag（此时虽然获取 tag 时其为空字符串，但是系统会认为其是一个随机值）
            // 每次弹出此 toast 时都会被认为是一个全新的 toast
            // toast.Tag = "2";

            toast.Activated += Toast_Activated;
            toast.Dismissed += Toast_Dismissed;
            toast.Failed += Toast_Failed;

            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }

        // 弹出 toast 通知（无 toast 通知 UI，仅放置于操作中心）
        private void buttonShowToast3_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Basic-Arguments 3'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 3 {DateTime.Now.ToString("mm:ss")}</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            toast.SuppressPopup = true; // 不会弹出 toast 通知 UI，但是会放置于操作中心
            toast.Tag = "3";

            toast.Activated += Toast_Activated;
            toast.Dismissed += Toast_Dismissed;
            toast.Failed += Toast_Failed;

            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }


        private void Toast_Activated(ToastNotification sender, object args)
        {
            Debug.WriteLine(sender.Tag + " Toast_Activated");
        }

        private void Toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            Debug.WriteLine(sender.Tag + " Toast_Dismissed");
        }

        private void Toast_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            Debug.WriteLine(sender.Tag + " Toast_Failed");
        }
    }
}
