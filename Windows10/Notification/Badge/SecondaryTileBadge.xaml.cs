/*
 * 本例用于演示 secondary tile 的 badge 通知
 *     secondary tile 的 badge 通知会显示在 secondary tile 上和 app 的任务栏的图标上
 * 
 * BadgeNotification - Badge 通知
 *     Content - Badge 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     ExpirationTime - Badge 通知的过期时间，超过这个时间就会清除这个 Badge
 *     
 * BadgeUpdateManager - Badge 更新管理器
 *     CreateBadgeUpdaterForSecondaryTile(string tileId) - 为指定的 secondary tile 创建一个 Badge 更新器，返回 BadgeUpdater 类型的数据
 *     
 * BadgeUpdater - Badge 更新器
 *     Update() - 更新指定的 BadgeNotification
 *     Clear() - 清除 BadgeNotification
 *     
 *     
 * 注：本例是通过 xml 来构造 badge 的，另外也可以通过 NuGet 的 Microsoft.Toolkit.Uwp.Notifications 来构造 badge（其用 c# 对 xml 做了封装）
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Badge
{
    public sealed partial class SecondaryTileBadge : Page
    {
        private const string TILEID = "tile_badge";

        public SecondaryTileBadge()
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

        // 以数字的方式更新指定的 Secondary Tile 的 Badge 通知
        private void btnUpdateBadgeWidthNumber_Click(object sender, RoutedEventArgs e)
        {
            // 用于描述 badge 通知的 xml 字符串（数字在 1 - 99 之间，如果大于 99 则会显示 99+ ，如果是 0 则会移除 badge，如果小于 0 则无效）
            string badgeXml = "<badge value='6'/>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument badgeDoc = new XmlDocument();
            badgeDoc.LoadXml(badgeXml);
            // 获取此 badge 的 xml
            // lblMsg.Text = badgeXml.GetXml();

            // 实例化 BadgeNotification 对象
            BadgeNotification badgeNotification = new BadgeNotification(badgeDoc);
            DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
            badgeNotification.ExpirationTime = expirationTime; // 30 秒后清除这个 badge

            // 将指定的 BadgeNotification 对象更新到指定的 secondary tile 磁贴
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TILEID);
            badgeUpdater.Update(badgeNotification);
        }

        // 以图标的方式更新指定的 Secondary Tile 的 Badge 通知
        private void btnUpdateBadgeWidthIcon_Click(object sender, RoutedEventArgs e)
        {
            // 用于描述 badge 通知的 xml 字符串
            string badgeXml = $"<badge value='{((ComboBoxItem)cmbBadgeValue.SelectedItem).Content}'/>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument badgeDoc = new XmlDocument();
            badgeDoc.LoadXml(badgeXml);
            // 获取此 badge 的 xml
            // lblMsg.Text = badgeXml.GetXml();

            // 实例化 BadgeNotification 对象
            BadgeNotification badgeNotification = new BadgeNotification(badgeDoc);
            DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
            badgeNotification.ExpirationTime = expirationTime; // 30 秒后清除这个 badge

            // 将指定的 BadgeNotification 对象更新到指定的 secondary tile 磁贴
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TILEID);
            badgeUpdater.Update(badgeNotification);
        }

        // 清除指定的 Secondary Tile 的 Badge 通知
        private void btnClearBadge_Click(object sender, RoutedEventArgs e)
        {
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TILEID);
            badgeUpdater.Clear();
        }
    }
}
