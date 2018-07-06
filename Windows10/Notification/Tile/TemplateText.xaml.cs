/*
 * 本例用于演示 tile 显示模板的文本相关的知识点
 * 
 * 
 * tile -  磁贴元素
 * visual - 可视元素
 * binding - 绑定元素
 *     hint-textStacking - 垂直对齐方式
 *         top - 顶部对齐（默认值）
 *         center - 垂直居中
 *         bottom - 底部对齐
 * text - 文本元素
 *     hint-style - 文本样式
 *     hint-align - 水平对齐方式
 *         left - 居左（默认值）
 *         center - 居中
 *         right - 居右
 *     hint-wrap - 是否可换行（默认值为 false）
 *     hint-minLines - 最小行数
 *     hint-maxLines - 最大行数
 *     
 *     
 * 
 * 
 * 
 * 关于 hint-style 的详细说明如下
 * 
 * 1、基本样式（epx - effective pixels 有效像素）
 * caption:   12epx 常规
 * body:      15epx 常规
 * base:      15epx 半粗
 * subtitle:  20epx 常规
 * title:     24epx 半细
 * subheader: 34epx 细体
 * header:    46epx 细体
 * 
 * 2、基本样式的 Numeral 变体（减小了行高）
 * titleNumeral, subheaderNumeral, headerNumeral
 * 
 * 3、“基本样式”和“基本样式的 Numeral 变体”的 Subtle 变体（透明度 60%）
 * captionSubtle, bodySubtle, baseSubtle, subtitleSubtle, titleSubtle, titleNumeralSubtle, subheaderSubtle, subheaderNumeralSubtle, headerSubtle, headerNumeralSubtle
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
    public sealed partial class TemplateText : Page
    {
        private const string TILEID = "tile_template_text";

        public TemplateText()
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

        // 文本的样式
        private void btnSample1_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='base'>base</text>
                            <text hint-style='baseSubtle'>baseSubtle</text>
                            <text hint-style='captionSubtle'>captionSubtle</text>
                            <text hint-style='titleNumeral'>titleNumeral</text>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // 水平对齐方式和文本换行
        private void btnSample2_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption' hint-align='center'>hint-align='center'</text>
                            <text hint-style='caption' hint-wrap='true' hint-minLines='1' hint-maxLines='10'>hint-wrap='true' hint-minLines='1' hint-maxLines='10'</text>
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // 垂直对齐方式
        private void btnSample3_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide' hint-textStacking='bottom'>
                            <text hint-style='caption'>caption 1</text>
                            <text hint-style='caption'>caption 2</text>
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
