/*
 * 本例用于演示如何弹出图文 toast
 * 单击 toast 激活 app 后（前台方式激活），如何获取相关信息请参见 Demo.xaml.cs 中的代码
 * 
 * 
 * 本例 xml 说明：
 * activationType - 通过点击 toast 激活 app 时的激活方式，foreground 代表前台方式激活
 * launch - 通过点击 toast 激活 app 时，传递给 app 的参数（通过按钮激活时，此参数无效）
 * template - 在 uwp 中就 ToastGeneric 一种模板
 * text - 每一个新的 text 会另起一行，一行显示不下会自动换行，第一个 text 会高亮显示，最多显示 5 行文本
 * image - 图片，最多显示一张图片
 *     hint-crop
 *         none - 图片不剪裁（默认值）
 *         circle - 图片剪裁成圆形
 *     placement
 *         inline - 图片显示在 toast 的文本的下面（默认值）
 *         appLogoOverride - 图片显示在 toast 的左上角
 *     addImageQuery
 *         是否将当前的部分环境信息以 url 参数的方式附加到图片地址中（一个 bool 值，默认值为 false）
 *         除了在 image 节点设置外，也可以在 visual 节点或 binding 节点设置
 *         
 *         
 * 注：图片不能大于 1024x1024 像素，不能大于 200 KB，类型必须为 .png .jpg .jpeg .gif
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Toast
{
    public sealed partial class TextAndImage : Page
    {
        public TextAndImage()
        {
            this.InitializeComponent();
        }

        // 弹出 toast 通知（图片来自 AppPackage）
        private void buttonShowToastImageFromAppPackage_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-TextAndImage-Arguments 1'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                            <image src='ms-appx:///Assets/hololens.jpg' />
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 toast 通知（图片来自 AppData，image 的 hint-crop 为 circle）
        private async void buttonShowToastImageFromAppData_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            StorageFolder localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("webabcdTest", CreationCollisionOption.OpenIfExists);
            StorageFile packageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/hololens.jpg"));
            await packageFile.CopyAsync(localFolder, "hololens.jpg", NameCollisionOption.ReplaceExisting);

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-TextAndImage-Arguments 2'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                            <image src='ms-appdata:///local/webabcdTest/hololens.jpg' hint-crop='circle' />
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastDoc = new XmlDocument();
            toastDoc.LoadXml(toastXml);

            ToastNotification toast = new ToastNotification(toastDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        // 弹出 toast 通知（图片来自 http，image 的 placement 为 appLogoOverride，image 的 addImageQuery 为 true）
        private void buttonShowToastImageFromHttp_Click(object sender, RoutedEventArgs e)
        {
            // 清除本 app 的之前的全部 toast 通知
            // ToastNotificationManager.History.Clear();

            /*
             * 本例中的 addImageQuery 被指定为 true
             * 所以以本例来说图片的实际请求地址为 http://images.cnblogs.com/mvpteam.gif?ms-scale=100&ms-contrast=standard 
             * 如果不指定 addImageQuery 或者将其设置为 false 则本例的图片的实际请求地址与 src 设置的一致，就是 http://images.cnblogs.com/mvpteam.gif
             */

            string toastXml = @"
                <toast activationType='foreground' launch='Notification-Toast-TextAndImage-Arguments 3'>
                    <visual addImageQuery='true'>
                        <binding template='ToastGeneric'>
                            <text>toast - title</text>
                            <text>""Hololens""引领技术革命浪潮传统的人机交互，主要是通过键盘和触摸，包括并不能被精确识别的语音等。""Hololens""的出现，则给新一代体验更好的人机交互指明道路，并现实了设备的小型化和便携化。</text>
                            <image src='http://images.cnblogs.com/mvpteam.gif3' placement='appLogoOverride' />
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
