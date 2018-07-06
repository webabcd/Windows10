/*
 * 本例用于演示 tile 显示模板的基础
 * 
 * 
 * tile -  磁贴元素
 * visual - 可视元素
 *     branding - 如何显示磁贴左下角的名称和右下角的图标
 *         none - 不参与（默认值）
 *         logo - 显示右下角的图标
 *         name - 显示左下角的名称
 *         nameAndLogo - 显示左下角的名称和右下角的图标
 *     displayName - 指定显示在磁贴左下角的名称（不指定且设置为显示时，则显示的是 SecondaryTile 对象的 DisplayName）
 * binding - 绑定元素
 *     template - 指定 tile 的规格（小，中，宽，大）
 *     branding - 如何显示磁贴左下角的名称和右下角的图标
 *         none - 不参与（默认值）
 *         logo - 显示右下角的图标
 *         name - 显示左下角的名称
 *         nameAndLogo - 显示左下角的名称和右下角的图标
 *     displayName - 指定显示在磁贴左下角的名称（不指定且设置为显示时，则显示的是 SecondaryTile 对象的 DisplayName）
 */

using System;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Tile
{
    public sealed partial class TemplateBasic : Page
    {
        private const string TILEID = "tile_template_basic";

        public TemplateBasic()
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

        // 为不同规格的磁贴指定不同的显示内容
        private void btnSample1_Click(object sender, RoutedEventArgs e)
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

            UpdateTileNotification(tileXml);
        }

        // 在 binding 节点指定磁贴左下角的名称显示和右下角的图标显示
        private void btnSample2_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide' branding='nameAndLogo' displayName='name 2'>
                            <text>Wide（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge' branding='nameAndLogo' displayName='name 3'>
                            <text>Large（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // 在 visual 节点指定磁贴左下角的名称显示和右下角的图标显示
        private void btnSample3_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual branding='nameAndLogo' displayName='name 4'>
                        <binding template='TileWide'>
                            <text>Wide（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }


        private void UpdateTileNotification(string tileXml)
        {
            XmlDocument tileDoc = new XmlDocument();
            tileDoc.LoadXml(tileXml);

            TileNotification tileNotification = new TileNotification(tileDoc);

            TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(TILEID);
            tileUpdater.Update(tileNotification);
        }
    }
}
