/*
 * 本例用于演示 toast 的不同特定场景
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * scenario - 场景类别，不指定则为默认场景
 *     default - 默认场景
 *     reminder - 弹出的 toast 框不会消失
 *     alarm - 弹出的 toast 框不会消失，且使用闹铃提示音
 *     incomingCall - 弹出的 toast 框不会消失，且按钮样式会与其他场景不同（在 mobile app 中会全屏显示）
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class Scenario : Page
    {
        public Scenario()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（默认场景）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-Scenario-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 1</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }

        // 弹出 toast 通知（reminder 场景）
        // 经测试，没有按钮的话则无法实现 reminder 场景的特性
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' scenario='reminder' launch='Notification-Toast-Scenario-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 2</text>
                        </binding>
                    </visual>
                    <actions>
                        <action content='确认' arguments='confirm' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }

        // 弹出 toast 通知（alarm 场景）
        // 经测试，没有按钮的话则无法实现 alarm 场景的特性
        private void buttonShowToast3_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' scenario='alarm' launch='Notification-Toast-Scenario-Arguments 3'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 3</text>
                        </binding>
                    </visual>
                    <actions>
                        <action content='确认' arguments='confirm' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }

        // 弹出 toast 通知（incomingCall 场景）
        private void buttonShowToast4_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' scenario='incomingCall' launch='Notification-Toast-Scenario-Arguments 4'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 4</text>
                        </binding>
                    </visual>
                    <actions>
                        <action content='确认' arguments='confirm' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }
    }
}
