/*
 * 演示如何将 secondary tile 的 Badge 通知和 Tile 通知发送到锁屏
 * 
 * 
 * SecondaryTile - secondary tile
 *     LockScreenDisplayBadgeAndTileText - 是否允许此 secondary tile 在锁屏上显示徽章和文本
 *     LockScreenBadgeLogo - 锁屏上徽章图标的地址
 * 
 * 注：
 * 在 app 安装好后，先要将 secondary tile 固定到开始屏幕，然后去“设置”->“个性化”->“锁屏界面”中做如下选择
 * 在“选择要显示详细状态的应用”中选择你的 secondary tile，则对此 secondary tile 发送 tile 通知后，此 tile 通知的文本就会显示在锁屏
 * 在“选择要显示快速状态的应用”中选择你的 secondary tile，则对此 secondary tile 发送 badge 通知后，此 badge 通知的图标就会显示在锁屏
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.LockScreen
{
    public sealed partial class SecondaryTileNotification : Page
    {
        private const string TILEID = "tile_lockscreen";

        public SecondaryTileNotification()
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
            SecondaryTile secondaryTile = new SecondaryTile(TILEID, "我的 secondary tile", "arguments", square150x150Logo, TileSize.Wide310x150);
            secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
            secondaryTile.VisualElements.Square310x310Logo = square310x310Logo;

            // 允许此 secondary tile 在锁屏上显示徽章和文本
            secondaryTile.LockScreenDisplayBadgeAndTileText = true;
            // 此 secondary tile 在锁屏上显示徽章时的图标
            secondaryTile.LockScreenBadgeLogo = new Uri("ms-appx:///Assets/LockScreenLogo.png");

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

        // 发送 secondary tile 的 Badge 通知
        private void btnBadgeNotification_Click(object sender, RoutedEventArgs e)
        {
            // 用于描述 badge 通知的 xml 字符串（数字在 1 - 99 之间，如果大于 99 则会显示 99+ ，如果是 0 则会移除 badge，如果小于 0 则无效）
            string badgeXml = "<badge value='3'/>";

            // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
            XmlDocument badgeDoc = new XmlDocument();
            badgeDoc.LoadXml(badgeXml);

            // 实例化 BadgeNotification 对象
            BadgeNotification badgeNotification = new BadgeNotification(badgeDoc);

            // 将指定的 BadgeNotification 对象更新到指定的 secondary tile 磁贴
            BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForSecondaryTile(TILEID);
            badgeUpdater.Update(badgeNotification);
        }

        // 发送 secondary tile 的 Tile 通知
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
                            <text>Small 1（小）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileMedium'>
                            <text>Medium 1（中）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileWide'>
                            <text id='1'>Wide 1（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text id='3'>Wide 2（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                            <text id='2'>Wide 3（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge'>
                            <text>Large 1（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
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

            // 将指定的 TileNotification 对象更新到 secondary tile
            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TILEID);
            tileUpdater.Update(tileNotification);
        }
    }
}
