/*
 * 本例用于演示 application 的 badge 通知
 *     application 的 badge 通知会显示在 application tile 上和 app 的任务栏的图标上
 * 
 * BadgeNotification - Badge 通知
 *     Content - Badge 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     ExpirationTime - Badge 通知的过期时间，超过这个时间就会清除这个 Badge（application tile 的 badge 会被清除，而 app 的任务栏的图标上 的 badge 不会被清除）
 *     
 * BadgeUpdateManager - Badge 更新管理器
 *     CreateBadgeUpdaterForApplication() - 创建一个 Application 的 Badge 更新器，返回 BadgeUpdater 类型的数据
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Badge
{
    public sealed partial class ApplicationBadge : Page
    {
        public ApplicationBadge()
        {
            this.InitializeComponent();
        }

        // 以数字的方式更新 Application Badge 通知
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

            // 将指定的 BadgeNotification 对象更新到 application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Update(badgeNotification);
        }

        // 以图标的方式更新 Application Badge 通知
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

            // 将指定的 BadgeNotification 对象更新到 application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Update(badgeNotification);
        }

        // 清除 Application Badge 通知
        private void btnClearBadge_Click(object sender, RoutedEventArgs e)
        {
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Clear();
        }
    }
}
