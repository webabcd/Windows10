/*
 * 演示如何将 Application 的 Badge 通知和 Tile 通知发送到锁屏
 * 
 * 
 * 注：
 * 1、需要在 Package.appxmanifest 中配置好是否支持锁屏徽章和文本，以及要配置好徽章图标。类似如下：
 * <LockScreen Notification="badgeAndTileText" BadgeLogo="Assets\LockScreenLogo.png" />
 * 2、在 app 安装好后，去“设置”->“个性化”->“锁屏界面”中做如下选择
 * 在“选择要显示详细状态的应用”中选择你的 app，则对此 app 发送 tile 通知后，此 tile 通知的文本就会显示在锁屏
 * 在“选择要显示快速状态的应用”中选择你的 app，则对此 app 发送 badge 通知后，此 badge 通知的图标就会显示在锁屏
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.LockScreen
{
    public sealed partial class ApplicationNotification : Page
    {
        public ApplicationNotification()
        {
            this.InitializeComponent();
        }

        // 发送 Application 的 Badge 通知
        private void btnBadgeNotification_Click(object sender, RoutedEventArgs e)
        {
            // 用于描述 badge 通知的 xml 字符串（数字在 1 - 99 之间，如果大于 99 则会显示 99+ ，如果是 0 则会移除 badge，如果小于 0 则无效）
            string badgeXml = "<badge value='3'/>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument badgeDoc = new XmlDocument();
            badgeDoc.LoadXml(badgeXml);

            // 实例化 BadgeNotification 对象
            BadgeNotification badgeNotification = new BadgeNotification(badgeDoc);

            // 将指定的 BadgeNotification 对象更新到 application
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
            badgeUpdater.Update(badgeNotification);
        }

        // 发送 Application 的 Tile 通知
        private void btnTileNotification_Click(object sender, RoutedEventArgs e)
        {
            /*
             * 这里有几个要特别注意的地方：
             * 1、锁屏只能显示 tile 的文本，不能显示 tile 的图片之类的
             * 2、锁屏只会显示 tile 的 TileWide 模板的文本
             * 3、要想在锁屏显示文本，则 tile 的 text 必须要有 id 属性并指定一个整型值
             * 4、锁屏文本会按照 tile 的 text 的 id 的大小按顺序显示，如果 id 值一样则只显示后面的
             * 5、tile 上的文本显示规则与其 text 的 id 无关（只有锁屏文本才有关系）
             */

            // 用于描述 tile 通知的 xml 字符串
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileSmall'>
                            <text id='1'>Small 1（小）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileMedium'>
                            <text id='1'>Medium 1（中）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileWide'>
                            <text id='1'>Wide 1（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text id='3'>Wide 2（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text id='2'>Wide 3（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge'>
                            <text id='1'>Large 1（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            /*
             * 如果需要锁屏显示的 tile 内容与 TileWide 显示的 tile 内容不一样的话，可以像下面这样写
             * 设置 TileWide 所属 binding 的 hint-lockDetailedStatus1 属性和 hint-lockDetailedStatus2 属性和 hint-lockDetailedStatus3 属性
             */
            string tileXml2 = $@"
                <tile>
                    <visual>
                        <binding template='TileSmall'>
                            <text>Small 1（小）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileMedium'>
                            <text>Medium 1（中）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileWide'
                                 hint-lockDetailedStatus1='hint-lockDetailedStatus1' 
                                 hint-lockDetailedStatus2='hint-lockDetailedStatus2' 
                                 hint-lockDetailedStatus3='hint-lockDetailedStatus3'>
                            <text>Wide 1（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text>Wide 2（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text>Wide 3（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge'>
                            <text>Large 1（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument tileDoc = new XmlDocument();
            tileDoc.LoadXml(tileXml);

            // 实例化 TileNotification 对象
            TileNotification tileNotification = new TileNotification(tileDoc);

            // 将指定的 TileNotification 对象更新到 application tile
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Update(tileNotification);
        }
    }
}
