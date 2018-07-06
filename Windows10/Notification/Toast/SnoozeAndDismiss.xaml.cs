/*
 * 本例用于演示如何通过 toast 选择在指定的时间之后延迟提醒或者取消延迟提醒
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * hint-systemCommands - 当此值为 SnoozeAndDismiss 时，则由系统设置下拉框和按钮，并由系统处理相关行为
 * action - 按钮（以下说明以 activationType='system' 为例）
 *     activationType - 单击此按钮激活 app 时的激活方式，system 代表由系统处理相关行为
 *     content - 按钮上显示的文本，不指定的话则由系统设置
 *     arguments - snooze 代表延迟按钮；dismiss 代表取消按钮
 *     hint-inputId - 用户选择延迟时间的下拉框的 id
 * 
 * 
 * 注：
 * 所谓的 snooze and dismiss 指的是：snooze - 在指定的时间之后延迟提醒，dismiss - 取消延迟提醒
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class SnoozeAndDismiss : Page
    {
        public SnoozeAndDismiss()
        {
            this.InitializeComponent();
        }

        // 弹出 snooze and dismiss toast 通知（由系统设置下拉框和按钮）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-SnoozeAndDismiss-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>snooze and dismiss</text>
                            <text>单击按钮后的行为由系统处理</text>
                        </binding>
                    </visual>
                    <actions hint-systemCommands='SnoozeAndDismiss' />
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 snooze and dismiss toast 通知（自定义下拉框，由系统设置按钮文字）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-SnoozeAndDismiss-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>snooze and dismiss</text>
                            <text>单击按钮后的行为由系统处理</text>
                        </binding>
                    </visual>
                    <actions>
                        <input id='snoozeTime' type='selection' defaultInput='1'>
                            <selection id='1' content='1 分钟'/>
                            <selection id='2' content='2 分钟'/>
                            <selection id='5' content='5 分钟'/>
                        </input>
                        <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='' />
                        <action activationType='system' arguments='dismiss' content='' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 snooze and dismiss toast 通知（自定义下拉框，自定义按钮文字）
        private void buttonShowToast3_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-SnoozeAndDismiss-Arguments 3'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>snooze and dismiss</text>
                            <text>单击按钮后的行为由系统处理</text>
                        </binding>
                    </visual>
                    <actions>
                        <input id='snoozeTime' type='selection' defaultInput='1'>
                            <selection id='1' content='1 分钟'/>
                            <selection id='2' content='2 分钟'/>
                            <selection id='5' content='5 分钟'/>
                        </input>
                        <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='延迟' />
                        <action activationType='system' arguments='dismiss' content='取消' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
