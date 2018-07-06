/*
 * 演示如何轮询服务端以更新 badge 通知
 *     
 * BadgeUpdater - Badge 更新器
 *     StartPeriodicUpdate(Uri badgeContent, PeriodicUpdateRecurrence requestedInterval) - 启动一个“轮询服务端以更新 badge 通知”的任务
 *     StartPeriodicUpdate(Uri badgeContent, DateTimeOffset startTime, PeriodicUpdateRecurrence requestedInterval) - 启动一个“轮询服务端以更新 badge 通知”的任务
 *         badgeContent - Badge 通知的内容（xml 格式数据）的 uri 地址（指定多个则会循环显示）
 *         startTime - 可以指定启动此任务的时间
 *             指定此值时的逻辑为：首先会立刻请求服务端获取数据，然后在到达 startTime 指定的时间点后再次获取数据，最后再每次按 requestedInterval 指定的间隔轮询服务端
 *         requestedInterval - 轮询服务端的周期（PeriodicUpdateRecurrence 枚举）
 *             HalfHour, Hour, SixHours, TwelveHours, Daily
 *     StopPeriodicUpdate() - 停止“轮询服务端以更新 badge 通知”的任务
 *     
 *     
 * 注：服务端代码请参见 WebApi/Controllers/BadgeContentController.cs（有指定 X-WNS-Expires 标头和 X-WNS-Tag 标头的示例）
 */

using System;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Badge
{
    public sealed partial class Periodic : Page
    {
        private const string TILEID = "tile_badge_periodic";

        public Periodic()
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
        }

        // 启动一个“轮询服务端以更新 badge 通知”的任务
        private void btnStartPeriodicUpdate_Click(object sender, RoutedEventArgs e)
        {
            // 启动一个循环更新 Badge 通知的任务，并指定 Badge 通知的数据源和轮询周期
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TILEID);

            // 马上请求服务端获取数据，然后 45 分钟之后再次获取数据，最后再每半个小时获取一次数据
            badgeUpdater.StartPeriodicUpdate(new Uri("http://localhost:44914/api/BadgeContent", UriKind.Absolute), DateTimeOffset.UtcNow.AddMinutes(45), PeriodicUpdateRecurrence.HalfHour);

            // Badge 通知的数据源示例请参见 WebApi/Controllers/BadgeContentController.cs
        }
    }
}
