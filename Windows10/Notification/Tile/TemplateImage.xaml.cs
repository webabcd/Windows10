/*
 * 本例用于演示 tile 显示模板的图片相关的知识点
 * 
 * 
 * tile -  磁贴元素
 * visual - 可视元素
 *     addImageQuery
 *         是否将当前的部分环境信息以 url 参数的方式附加到图片地址中（一个 bool 值，默认值为 false）
 * binding - 绑定元素
 *     addImageQuery
 *         是否将当前的部分环境信息以 url 参数的方式附加到图片地址中（一个 bool 值，默认值为 false）
 *     hint-presentation - 内容的呈现方式
 *         none - 默认值
 *         photos - 让最多 12 张图片以幻灯片的方式循环显示，图片间切换时会有一些过渡效果（仅支持图片，不支持文本）
 *         people - 类似 win10 的“人脉”应用，即将多张图片圆形剪裁并堆在磁贴上，再增加一些动画效果（仅支持图片，不支持文本）
 * image - 图片元素
 *     src - 图片地址（ms-appx:/// 或 ms-appdata:///local/ 或 http:// 或 https://）
 *     hint-removeMargin - 是否移除图片的 margin（默认值为 false）
 *         显示图片时，图片自己默认会带着 8 个像素的 margin，此属性用于指定是否将这 8 个像素的 margin 去掉
 *         注：磁贴自带的 padding 为 8 个像素，所以即使设置了 hint-removeMargin='true' 图片和磁贴边缘还是有距离的
 *     placement - 图片显示方式
 *         inline - 图片显示在磁贴内部（默认值）
 *         background - 图片作为磁贴的背景
 *         peek - 图片与文本轮流显示
 *     hint-overlay - 在图片上覆盖一层黑色的遮罩，并设置遮罩的不透明度（0 - 100）
 *         此属性只对 background 图片和 peek 图片有效
 *         在 binding 节点也可以设置此属性
 *     hint-align - 图片对齐方式
 *         stretch - 拉伸（默认值）
 *         left - 居左（图片以原始分辨率显示）
 *         center - 居中（图片以原始分辨率显示）
 *         right - 居右（图片以原始分辨率显示）
 *     hint-crop
 *         none - 图片不剪裁（默认值）
 *         circle - 图片剪裁成圆形
 *     addImageQuery
 *         是否将当前的部分环境信息以 url 参数的方式附加到图片地址中（一个 bool 值，默认值为 false）
 *         
 *         
 * 注：图片不能大于 1024x1024 像素，不能大于 200 KB，类型必须为 .png .jpg .jpeg .gif
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
    public sealed partial class TemplateImage : Page
    {
        private const string TILEID = "tile_template_image";

        public TemplateImage()
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

        // hint-removeMargin
        private void btnSample1_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='ms-appx:///Assets/hololens.jpg' hint-removeMargin='{((FrameworkElement)cmbRemoveMargin.SelectedItem).Tag}' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // placement
        private void btnSample2_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                 <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='ms-appx:///Assets/hololens.jpg' placement='{((FrameworkElement)cmbPlacement.SelectedItem).Tag}' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // hint-align
        private void btnSample3_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                 <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='ms-appx:///Assets/hololens.small.jpg' hint-align='{((FrameworkElement)cmbAlign.SelectedItem).Tag}' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // hint-crop='circle' addImageQuery='true'
        private void btnSample4_Click(object sender, RoutedEventArgs e)
        {
            /*
             * 本例中的 addImageQuery 被指定为 true
             * 所以以本例来说图片的实际请求地址为 http://images.cnblogs.com/mvpteam.gif?ms-scale=100&ms-contrast=standard 
             * 如果不指定 addImageQuery 或者将其设置为 false 则本例的图片的实际请求地址与 src 设置的一致，就是 http://images.cnblogs.com/mvpteam.gif
             */

            string tileXml = $@"
                 <tile>
                    <visual addImageQuery='true'>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='http://images.cnblogs.com/mvpteam.gif' hint-crop='circle' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // peek 图片和 background 图片相结合
        // “peek 图片”和“background 图片 + 文本”轮流显示
        private void btnSample5_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                 <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='ms-appx:///Assets/hololens.jpg' placement='peek' />
                            <image src='ms-appx:///Assets/StoreLogo.png' placement='background' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // peek 图片和 background 图片相结合，并指定其 hint-overlay
        private void btnSample6_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                 <tile>
                    <visual>
                        <binding template='TileWide'>
                            <text hint-style='caption'>title</text>
                            <image src='ms-appx:///Assets/hololens.jpg' placement='peek' hint-overlay='50' />
                            <image src='ms-appx:///Assets/StoreLogo.png' placement='background' hint-overlay='80' />
                        </binding>
                    </visual>
                </tile>";

            UpdateTileNotification(tileXml);
        }

        // hint-presentation
        private void btnSample7_Click(object sender, RoutedEventArgs e)
        {
            string tileXml = $@"
                 <tile>
                    <visual>
                        <binding template='TileWide' hint-presentation='{((FrameworkElement)cmbPresentation.SelectedItem).Tag}'>
                            <image src='ms-appx:///Assets/hololens.jpg' />
                            <image src='ms-appx:///Assets/StoreLogo.png' />
                            <image src='ms-appx:///Assets/hololens.jpg' />
                            <image src='ms-appx:///Assets/StoreLogo.png' />
                            <image src='ms-appx:///Assets/hololens.jpg' />
                            <image src='ms-appx:///Assets/StoreLogo.png' />
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
