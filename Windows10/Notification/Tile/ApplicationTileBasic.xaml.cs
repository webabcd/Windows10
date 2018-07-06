/*
 * 本例用于演示 application tile 的基础
 * 
 * 
 * TileNotification - Tile 通知
 *     Content - Tile 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     ExpirationTime - Tile 通知的过期时间，超过这个时间就会清除这个 Tile
 *     
 * TileUpdateManager - Tile 更新管理器
 *     CreateTileUpdaterForApplication() - 创建一个 application tile 更新器
 *     
 * TileUpdater - 磁贴的 Tile 更新器
 *     Update(TileNotification notification) - 将指定的 TileNotification 对象更新到 application tile
 *     Clear() - 清除 application tile 的数据
 *     EnableNotificationQueue(bool enable) - 是否启用 tile 的队列功能（即 tile 循环显示），队列最多可容纳 5 个 tile
 *     EnableNotificationQueueForSquare150x150(bool enable) - 是否启用中磁贴的 tile 队列功能
 *     EnableNotificationQueueForWide310x150(bool enable) - 是否启用宽磁贴的 tile 队列功能
 *     EnableNotificationQueueForSquare310x310(bool enable) - 是否启用大磁贴的 tile 队列功能
 *     Setting - 获取通知设置（NotificationSetting 枚举）
 *         Enabled - 通知可被显示
 *         DisabledForApplication, DisabledForUser, DisabledByGroupPolicy, DisabledByManifest - 因相应的原因通知被禁止显示
 * 
 * 
 * 注：
 * 1、磁贴规格分为小，中，宽，大，对于 application tile 来说支持哪种规格的磁贴需要在 Package.appxmanifest 中做相应的配置
 * 2、对于 application tile 来说，其左下角是否显示名称可以在 Package.appxmanifest 中配置，可以使其显示指定的短名称，不指定短名称的话则显示的是“显示名称”
 * 3、小规格的磁贴无法在其左下角显示名称，但是可以显示 tile 通知（但是不支持 tile 通知的队列功能）
 * 4、无法为 application tile 设置启动参数，可以为 secondary tile 设置启动参数
 * 
 * 
 * 注：本例是通过 xml 来构造 tile 的，另外也可以通过 NuGet 的 Microsoft.Toolkit.Uwp.Notifications 来构造 tile（其用 c# 对 xml 做了封装）
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Tile
{
    public sealed partial class ApplicationTileBasic : Page
    {
        public ApplicationTileBasic()
        {
            this.InitializeComponent();
        }

        // 更新 application tile 的数据
        private void btnUpdateTile_Click(object sender, RoutedEventArgs e)
        {
            // 用于描述 tile 通知的 xml 字符串
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileSmall'>
                            <text>Small（小）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileMedium'>
                            <text>Medium（中）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileWide'>
                            <text>Wide（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge'>
                            <text>Large（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument tileDoc = new XmlDocument();
            tileDoc.LoadXml(tileXml);
            // 获取此 tile 的 xml
            // lblMsg.Text = tileDoc.GetXml();

            // 实例化 TileNotification 对象
            TileNotification tileNotification = new TileNotification(tileDoc);
            DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
            tileNotification.ExpirationTime = expirationTime; // 30 秒后清除这个 tile

            // 将指定的 TileNotification 对象更新到 application tile
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.EnableNotificationQueue(true); // 启用 tile 的队列功能（最多可容纳 5 个 tile）
            tileUpdater.Update(tileNotification);
        }

        // 清除 application tile 的数据
        private void btnClearTile_Click(object sender, RoutedEventArgs e)
        {
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Clear();
        }
    }
}
