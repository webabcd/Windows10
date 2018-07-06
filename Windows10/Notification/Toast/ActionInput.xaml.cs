/*
 * 本例用于演示如何弹出带输入的 toast（文本输入框，下拉选择框）
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，foreground 代表前台方式激活
 * launch - 通过点击 toast 激活 app 时，传递给 app 的参数（通过按钮激活时，此参数无效）
 * template - 在 uwp 中就 ToastGeneric 一种模板
 * text - 每一个新的 text 会另起一行，一行显示不下会自动换行，第一个 text 会高亮显示，最多显示 5 行文本
 * input - 输入框，最多显示 5 个输入框
 *     type - text 文本输入框；selection 下拉选择框（最多 5 条选项）
 *     id - 标识
 *     title - 输入框上显示的标题
 *     defaultInput - 输入框内显示的默认内容
 *     placeHolderContent - 输入框内显示的 placeHolder
 * action - 按钮
 *     hint-inputId - 用于快速应答场景，指定快速应答的 input 的 id
 *         说明：当指定的 input 无内容时则按钮不可点击，当指定的 input 有内容时则按钮可点击（也可以通过快捷键 ctrl + enter 发送）
 *     
 *     
 * 注：只有通过 toast 中的按钮激活 app 时，input 中的内容才会被传递（通过点击 toast 激活 app 时，input 中的内容不会被传递）
 */

using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class ActionInput : Page
    {
        public ActionInput()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（文本输入框）
        private void buttonShowToast1_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-ActionInput-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                    <actions>
                        <input type='text' id='message1' title='title1' />
                        <input type='text' id='message2' title='title2' defaultInput='defaultInput'/>
                        <input type='text' id='message3' title='title3' placeHolderContent='placeHolderContent'/>
                        <action content='确认' activationType='foreground' arguments='Notification-Toast-ActionInput-Arguments 1 confirm'/>
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 toast 通知（文本输入框与快速应答）
        private void buttonShowToast2_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-ActionInput-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                    <actions>
                        <input id='message' type='text' />
                        <action content='确认' activationType='foreground' arguments='Notification-Toast-ActionInput-Arguments 2 confirm' 
                            hint-inputId='message' />
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 toast 通知（下拉选择框）
        private void buttonShowToast3_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-ActionInput-Arguments 3'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                        </binding>
                    </visual>
                    <actions>
                        <input id ='select' type='selection' defaultInput='2'>
                            <selection id='1' content='1天' />
                            <selection id='2' content='2天' />
                            <selection id='3' content='3天' />
                            <selection id='4' content='4天' />
                            <selection id='5' content='5天' />
                        </input> 
                        <action content='确认' activationType='foreground' arguments='Notification-Toast-ActionInput-Arguments 3 confirm'/>
                    </actions>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
