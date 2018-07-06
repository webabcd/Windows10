/*
 * 演示如何按计划显示 tile 通知（在指定的时间显示指定的 tile 通知，此特性在 application tile 和 secondary tile 中均支持）
 * 
 * ScheduledTileNotification - 按计划显示的 Tile 通知
 *     Content - Tile 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     DeliveryTime - 显示 Tile 通知的时间，只读，其需要在构造函数中指定
 *     ExpirationTime - Tile 通知的过期时间，超过这个时间就会清除这个 Tile
 *     Id - ScheduledTileNotification 的标识
 *     Tag - 在启用 tile 的队列功能时，如果 tile 的 Tag 相同则新的内容会更新旧的内容（Tag 值的前 16 个字符相同则认为是相同的 Tag）
 *         不指定的 Tag 的话则认为 Tag 都是不同的
 *     
 * TileUpdater - 磁贴的 Tile 更新器
 *     AddToSchedule() - 将指定的 ScheduledTileNotification 对象添加到计划列表
 *     RemoveFromSchedule() - 从计划列表中移除指定的 ScheduledTileNotification 对象
 *     GetScheduledTileNotifications() - 获取全部 ScheduledTileNotification 对象列表
 */

using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Tile
{
    public sealed partial class Schedule : Page
    {
        private const string TILEID = "tile_schedule";

        public Schedule()
        {
            this.InitializeComponent();
        }

        // 在开始屏幕固定一个 secondary tile 磁贴
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Uri square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
            Uri wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
            Uri square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");
            SecondaryTile secondaryTile = new SecondaryTile(TILEID, "name", "arguments", square150x150Logo, TileSize.Wide310x150);
            secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
            secondaryTile.VisualElements.Square310x310Logo = square310x310Logo;

            try
            {
                bool isPinned = await secondaryTile.RequestCreateAsync();
                lblMsg.Text = isPinned ? "固定成功" : "固定失败";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "固定失败: " + ex.ToString();
            }

            ShowScheduledTiles();
        }

        // 添加指定的 ScheduledTileNotification 到指定的 secondary tile 的计划列表中
        private void btnAddScheduledTile_Click(object sender, RoutedEventArgs e)
        {
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

            XmlDocument tileDoc = new XmlDocument();
            tileDoc.LoadXml(tileXml);

            // 实例化 ScheduledTileNotification 对象（15 秒后显示此 Tile 通知）
            DateTime dt = DateTime.Now.AddSeconds(15);
            ScheduledTileNotification tileNotification = new ScheduledTileNotification(tileDoc, dt);

            tileNotification.Id = new Random().Next(100000, 1000000).ToString(); ;
            tileNotification.Tag = $"在 {dt.ToString("HH:mm:ss")} 时显示此 tile 通知";

            // 将指定的 ScheduledTileNotification 对象添加进指定的 secondary tile 的计划列表
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TILEID);
            tileUpdater.AddToSchedule(tileNotification);
            tileUpdater.EnableNotificationQueue(true); // 启用 tile 的队列功能（最多可容纳 5 个 tile）

            ShowScheduledTiles();
        }

        // 删除指定 secondary tile 的指定 ScheduledTileNotification 对象
        private void btnRemoveScheduledTile_Click(object sender, RoutedEventArgs e)
        {
            string notificationId = (string)(sender as FrameworkElement).Tag;

            // 获取指定 secondary tile 的全部 ScheduledTileNotification 对象列表
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TILEID);
            IReadOnlyList<ScheduledTileNotification> notifications = tileUpdater.GetScheduledTileNotifications();

            int notificationCount = notifications.Count;
            for (int i = 0; i < notificationCount; i++)
            {
                if (notifications[i].Id == notificationId)
                {
                    // 从计划列表中移除指定的 ScheduledTileNotification 对象
                    tileUpdater.RemoveFromSchedule(notifications[i]);
                    break;
                }
            }

            ShowScheduledTiles();
        }

        // 显示指定 secondary tile 的全部 ScheduledTileNotification 列表
        private void ShowScheduledTiles()
        {
            // 获取指定 secondary tile 的全部 ScheduledTileNotification 对象列表
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TILEID);
            IReadOnlyList<ScheduledTileNotification> notifications = tileUpdater.GetScheduledTileNotifications();

            listBox.ItemsSource = notifications;
        }
    }
}
