/*
 * 本例用于演示 tile 显示模板的分组相关的知识点
 * 
 * 
 * tile -  磁贴元素
 * visual - 可视元素
 * binding - 绑定元素
 * group/subgroup - 组元素/子组元素
 *     一个 binding 内可以包含多个 group，一个 group 内可以包含多个 subgroup 且 group 的子节点只能是 subgroup
 *     不同的 group 在垂直方向上排列，不同的 subgroup 在水平方向上排列
 *     在磁贴上显示多 group 数据时，如果后面的 group 的内容无法显示完全的话则可能不会被显示
 * subgroup - 子组元素
 *     hint-weight - 多 subgroup 水平排列时，每列 subgroup 所占用空间的权重
 *     hint-textStacking - 垂直对齐方式
 *         top - 顶部对齐（默认值）
 *         center - 垂直居中
 *         bottom - 底部对齐
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
    public sealed partial class TemplateGroup : Page
    {
        private const string TILEID = "tile_template_group";

        public TemplateGroup()
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

        // 垂直分组（在大磁贴中演示）
        private void btnSample1_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileLarge'>
                            <group>
                                <subgroup>
                                    <text hint-style='caption'>caption 1</text>
                                    <text hint-style='captionSubtle'>captionSubtle 1</text>
                                </subgroup>
                            </group>
                            <text />
                            <group>
                                <subgroup>
                                    <text hint-style='caption'>caption 2</text>
                                    <text hint-style='captionSubtle'>captionSubtle 2</text>
                                </subgroup>
                            </group>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // 水平分组（可以指定每列的空间权重）
        private void btnSample2_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide'>
                            <group>
                                <subgroup hint-weight='1'>
                                    <text hint-style='caption'>caption 1</text>
                                    <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='true'/>
                                    <text hint-style='captionSubtle'>captionSubtle 1</text>
                                </subgroup>
                                <subgroup hint-weight='1'>
                                    <text hint-style='caption'>caption 2</text>
                                    <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='true'/>
                                    <text hint-style='captionSubtle'>captionSubtle 2</text>
                                </subgroup>
                            </group>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // 水平分组（可以指定每列的空间权重，以及每列的垂直对齐方式）
        private void btnSample3_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide'>
                            <group>
                                <subgroup hint-weight='2'>
                                    <text hint-style='caption'>caption 1</text>
                                    <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='true'/>
                                    <text hint-style='captionSubtle'>captionSubtle 1</text>
                                </subgroup>
                                <subgroup hint-weight='1' hint-textStacking='center'>
                                    <text hint-style='caption'>caption 2</text>
                                    <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='true'/>
                                    <text hint-style='captionSubtle'>captionSubtle 2</text>
                                </subgroup>
                                <subgroup hint-weight='1' hint-textStacking='bottom'>
                                    <text hint-style='caption'>caption 3</text>
                                    <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='true'/>
                                    <text hint-style='captionSubtle'>captionSubtle 3</text>
                                </subgroup>
                            </group>
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
