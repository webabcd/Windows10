/*
 * 本例用于演示如何弹出带按钮的 toast
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，foreground 代表前台方式激活
 * launch - 通过点击 toast 激活 app 时，传递给 app 的参数（通过按钮激活时，此参数无效）
 * template - 在 uwp 中就 ToastGeneric 一种模板
 * text - 每一个新的 text 会另起一行，一行显示不下会自动换行，第一个 text 会高亮显示，最多显示 5 行文本
 * action - 按钮，最多显示 5 个按钮
 *     content - 按钮上显示的文本
 *     activationType - 单击此按钮激活 app 时的激活方式，foreground 代表前台方式激活
 *     arguments - 单击此按钮激活 app 时，传递给 app 的参数
 *     imageUri - 图文按钮上显示的图标
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class ActionButton : Page
    {
        public ActionButton()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（文本按钮）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-ActionButton-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                    <actions>
                        <action content='确认' activationType='foreground' arguments='Notification-Toast-ActionButton-Arguments 1 confirm'/>
                        <action content='取消' activationType='foreground' arguments='Notification-Toast-ActionButton-Arguments 1 cancel' imageUri='Assets/StoreLogo.png' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toastNotification = new ToastNotification(toastDoc);
            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toastNotification);
        }

        // 弹出 toast 通知（图文按钮）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-ActionButton-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                    <actions>
                        <action content='确认' activationType='foreground' arguments='Notification-Toast-ActionButton-Arguments 2 confirm' imageUri='Assets/StoreLogo.png' />
                        <action content='取消' activationType='foreground' arguments='Notification-Toast-ActionButton-Arguments 2 cancel' imageUri='Assets/StoreLogo.png' />
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
