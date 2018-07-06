/*
 * 本例用于演示如何通过 toast 打开指定的协议
 * 
 * 
 * 本例 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，protocol 代表打开指定的协议
 * launch - 协议地址
 *
 * 
 * 注：通过 toast 中的按钮打开指定协议也是类似的，示例如下
 * <action content='打开' activationType='protocol' arguments='http://webabcd.cnblogs.com/' />
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class LaunchProtocol : Page
    {
        public LaunchProtocol()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（打开 http 协议）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='protocol' launch='http://webabcd.cnblogs.com/'>
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

        // 弹出 toast 通知（打开 webabcd 协议）
        // 关于 webabcd 协议的支持，请参见 /AssociationLaunching/ProtocolAssociation.xaml.cs
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='protocol' launch='webabcd:data'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content 2</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }
    }
}
