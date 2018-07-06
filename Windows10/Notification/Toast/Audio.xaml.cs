/*
 * 本例用于演示 toast 的提示音
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * audio - 提示音
 *     src - 播放的提示音的地址。支持系统提示音，ms-appx 地址的音频文件，ms-appdata 地址的音频文件
 *     loop - 是否循环，默认值为 false
 *     silent - 是否静音，默认值为 false
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class Audio : Page
    {
        public Audio()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（系统提示音）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Audio-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>audio</text>
                            <text>演示 toast 的提示音</text>
                        </binding>
                    </visual>
                    <audio src='{cmbSrc.SelectionBoxItem}' loop='{cmbLoop.SelectionBoxItem}' silent='{cmbSilent.SelectionBoxItem}' />
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);
            
            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }

        // 弹出 toast 通知（ms-appx 或 ms-appdata 地址的音频文件）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = $@"
                <toast activationType='foreground' launch='Notification-Toast-Audio-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>audio</text>
                            <text>演示 toast 的提示音</text>
                        </binding>
                    </visual>
                    <audio src='ms-appx:///Assets/audio.aac' />
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }
    }
}
