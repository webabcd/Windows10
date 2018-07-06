/*
 * 本例用于演示如何弹出纯文本 toast，以及如何控制 toast 的显示时长（短时和长时两种）
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，foreground 代表前台方式激活
 * launch - 通过点击 toast 激活 app 时，传递给 app 的参数（通过按钮激活时，此参数无效）
 * duration - short 短时通知（默认值）；long 长时通知
 * template - 在 uwp 中就 ToastGeneric 一种模板
 * text - 每一个新的 text 会另起一行，一行显示不下会自动换行，第一个 text 会高亮显示，最多显示 5 行文本
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class Text : Page
    {
        public Text()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（短时通知）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-Text-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>toast - content</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 toast 通知（长时通知）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' duration='long' launch='Notification-Toast-Text-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
