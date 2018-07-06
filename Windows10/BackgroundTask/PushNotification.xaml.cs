/*
 * 演示如何接收推送通知
 * 由于本例没有上商店，所以本例是无法演示的，需要看演示效果的话运行一下自己写的“打字通”的 /TypingGame/PushNotification/Sample.xaml，然后用其生成的 channel 地址在 /WebApi/Controllers/PushNotificationController.cs 推送通知
 * 
 * 
 * 注：
 * 1、在商店后台的 dashboard 中找到你的 app 的“包名”和“发布者”并替换你的 Package.appxmanifest 中的相关节点，类似如下
 *    <Identity Name="10437webabcd.**********E91" Publisher="CN=27514DEC-****-****-****-F956384483D0" Version="1.0.0.0" />
 *    也可以直接访问 https://apps.dev.microsoft.com/ 来查找这些信息
 *    最简单也是推荐的做法是：“选中项目”->“右键”->“应用商店”->“将应用程序与应用商店关联”，按提示操作后会自动将商店信息同步到你的项目中
 * 2、需要在 Package.appxmanifest 中增加后台任务声明，并勾选“推送通知”（经测试发现不做这一步也可以，但是为了保险还是加上吧）
 * 3、每次新建的 channel 有效期为 30 天
 * 
 * 
 * 另：
 * WNS - Windows Push Notification Service
 * 推送通知的服务端参见：/WebApi/Controllers/PushNotificationController.cs
 */

using System;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.BackgroundTask
{
    public sealed partial class PushNotification : Page
    {
        public PushNotification()
        {
            this.InitializeComponent();
        }

        private async void btnCreateChannel_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个推送通知信道，每个新建的 channel 有效期为 30 天，所以建议每次进入 app 后都重新建一个 channel（如果两次创建的间隔时间较短的话，则会复用之前的 channel 地址）
            PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            // 接收到通知后所触发的事件
            channel.PushNotificationReceived += channel_PushNotificationReceived;

            // channel.Close(); // 关闭 channel
            // channel.ExpirationTime; // channel 的过期时间，此时间过后 channel 则失效

            // channel 的 uri 地址，服务端通过此 uri 向此 app 推送通知
            txtUri.Text = channel.Uri.ToString();
        }

        void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge: // badge 通知
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(args.BadgeNotification);
                    break;
                case PushNotificationType.Raw: // raw 通知
                    // 当收到推送的 raw 通知时，如果 app 在锁屏，则可以触发后台任务以执行相关的逻辑（PushNotificationTrigger）
                    string msg = args.RawNotification.Content;
                    break;
                case PushNotificationType.Tile: // tile 通知
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(args.TileNotification);
                    break;
                case PushNotificationType.Toast: // toast 通知
                    ToastNotificationManager.CreateToastNotifier().Show(args.ToastNotification);
                    break;
                default:
                    break;
            }

            args.Cancel = true;
        }
    }
}
