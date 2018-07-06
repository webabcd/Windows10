/*
 * 本例用于演示 secondary tile 的基础
 * 
 * 
 * SecondaryTile - secondary tile
 *     TileId - secondary tile 的唯一标识
 *     SecondaryTile.Exists(tileId) - 指定 TileId 的 secondary tile 是否存在（静态方法）
 *     SecondaryTile secondaryTile = new SecondaryTile(tileId)
 *         如果指定的 tileId 不存在，则创建 SecondaryTile 对象
 *         如果指定的 tileId 存在，则返回此 SecondaryTile 对象
 *     SecondaryTile secondaryTile = SecondaryTile(string tileId, string displayName, string arguments, Uri square150x150Logo, TileSize desiredSize)
 *         tileId - secondary tile 的唯一标识（指定的 tileId 不存在，则创建 SecondaryTile 对象；指定的 tileId 存在，则返回此 SecondaryTile 对象）
 *         displayName - 磁贴左下角的显示名称（磁贴的 ToolTip 用的也是此值）
 *         arguments - 启动参数
 *         square150x150Logo - 中磁贴图标
 *         desiredSize - 固定到开始屏幕时的期望的初始规格（Square150x150, Wide310x150）
 *     DisplayName - 磁贴左下角的显示名称（磁贴的 ToolTip 用的也是此值）
 *     Arguments - 启动参数（通过磁贴启动 app 时可以获取此参数，只可以在 OnLaunched 中获取，在 OnActivated 之类的中是获取不到的））
 *     VisualElements - 返回一个 SecondaryTileVisualElements 对象，用于设置磁贴的部分显示属性
 *     RequestCreateAsync() - 请求固定此 SecondaryTile（会弹出确认框）
 *     RequestDeleteAsync() - 请求取消固定此 SecondaryTile
 *     UpdateAsync() - 更新此 SecondaryTile（这里不是更新 Tile 通知，而只是更新 SecondaryTile 对象的相关信息）
 *     FindAllAsync() - 获取此 app 固定到开始屏幕的所有 SecondaryTile 集合（静态方法）
 *     
 * SecondaryTileVisualElements - 磁贴的部分显示属性
 *     BackgroundColor - 背景颜色
 *     ForegroundText - 前景色类别（Dark 或 Light）
 *     Square150x150Logo - 中磁贴的图标的地址
 *     Wide310x150Logo - 宽磁贴的图标的地址
 *     Square310x310Logo - 大磁贴的图标的地址
 *     ShowNameOnSquare150x150Logo - 是否在中磁贴的左下角显示名称
 *     ShowNameOnWide310x150Logo - 是否在宽磁贴的左下角显示名称
 *     ShowNameOnSquare310x310Logo - 是否在大磁贴的左下角显示名称
 * 
 * TileNotification - Tile 通知
 *     Content - Tile 的内容，XmlDocument 类型的数据，只读，其需要在构造函数中指定
 *     ExpirationTime - Tile 通知的过期时间，超过这个时间就会清除这个 Tile
 *     
 * TileUpdateManager - Tile 更新管理器
 *     CreateTileUpdaterForSecondaryTile(string tileId) - 为指定的 SecondaryTile 创建一个 Tile 更新器，返回 TileUpdater 类型的数据
 *     
 * TileUpdater - 磁贴的 Tile 更新器
 *     Update(TileNotification notification) - 将指定的 TileNotification 对象更新到指定的 secondary tile
 *     Clear() - 清除 secondary tile 的数据
 *     EnableNotificationQueue(bool enable) - 是否启用 tile 的队列功能（即 tile 循环显示），队列最多可容纳 5 个 tile
 *     EnableNotificationQueueForSquare150x150(bool enable) - 是否启用中磁贴的 tile 队列功能
 *     EnableNotificationQueueForWide310x150(bool enable) - 是否启用宽磁贴的 tile 队列功能
 *     EnableNotificationQueueForSquare310x310(bool enable) - 是否启用大磁贴的 tile 队列功能
 *     Setting - 获取通知设置（NotificationSetting 枚举）
 *         Enabled - 通知可被显示
 *         DisabledForApplication, DisabledForUser, DisabledByGroupPolicy, DisabledByManifest - 因相应的原因通知被禁止显示
 * 
 * 
 * 注：
 * 1、磁贴规格分为小，中，宽，大
 * 2、小规格的磁贴无法在其左下角显示名称，但是可以显示 tile 通知（但是不支持 tile 通知的队列功能）
 * 3、对于 secondary tile 来说，每个磁贴都可以为其设置一个启动参数，此启动参数与磁贴当前显示的是什么通知无关，其只与磁贴有关
 * 4、单击 secondary tile 启动 app 时，可以在 App.xaml.cs 中通过如下方式获取启动参数（本例的相关代码在 /UI/MySplashScreen.xaml.cs 中）
 * protected async override void OnLaunched(LaunchActivatedEventArgs args)
 * {
 *     string arguments = args.Arguments;
 * }
 * 
 * 
 * 注：本例是通过 xml 来构造 tile 的，另外也可以通过 NuGet 的 Microsoft.Toolkit.Uwp.Notifications 来构造 tile（其用 c# 对 xml 做了封装）
 */

using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Notification.Tile
{
    public sealed partial class SecondaryTileBasic : Page
    {
        private Random _random = new Random();

        public SecondaryTileBasic()
        {
            this.InitializeComponent();
        }

        // 固定一个新的 SecondaryTile（可以固定多个）
        private async void btnPinSecondaryTile_Click(object sender, RoutedEventArgs e)
        {
            Uri square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
            Uri wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.png");
            Uri square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");

            string tileId = _random.Next(100000, 1000000).ToString();

            /*
            if (SecondaryTile.Exists(tileId)) // 指定 tileId 的 secondary tile 是否存在
            {
                SecondaryTile secondaryTile = new SecondaryTile(tileId); // 通过 tileId 获取指定的 SecondaryTile 对象
            }
            */

            // 创建一个 SecondaryTile 对象
            SecondaryTile secondaryTile = new SecondaryTile
            (
                tileId,
                "DisplayName",
                $"Arguments（TileId: {tileId}）{DateTime.Now.ToString("HH:mm:ss")}", // 不能设置为空，否则会报错
                square150x150Logo,
                TileSize.Wide310x150 // 我这里测试，只能设置为 Square150x150 或 Wide310x150
            );

            secondaryTile.VisualElements.Wide310x150Logo = wide310x150Logo;
            secondaryTile.VisualElements.Square310x310Logo = square310x310Logo;

            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

            secondaryTile.VisualElements.BackgroundColor = Colors.Orange;
            secondaryTile.VisualElements.ForegroundText = ForegroundText.Light;

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

        // 更新所有的 SecondaryTile 通知
        private async void btnUpdateSecondaryTile_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<SecondaryTile> secondaryTileList = await SecondaryTile.FindAllAsync();
            foreach (var secondaryTile in secondaryTileList)
            {
                // 用于描述 tile 通知的 xml 字符串
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

                // 将 xml 字符串转换为 Windows.Data.Xml.Dom.XmlDocument 对象
                XmlDocument tileDoc = new XmlDocument();
                tileDoc.LoadXml(tileXml);
                // 获取此 tile 的 xml
                // lblMsg.Text = tileDoc.GetXml();

                // 实例化 TileNotification 对象
                TileNotification tileNotification = new TileNotification(tileDoc);
                DateTimeOffset expirationTime = DateTimeOffset.UtcNow.AddSeconds(30);
                tileNotification.ExpirationTime = expirationTime; // 30 秒后清除这个 tile

                // 将指定的 TileNotification 对象更新到 secondary tile
                TileUpdater tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(secondaryTile.TileId);
                tileUpdater.EnableNotificationQueue(true); // 启用 tile 的队列功能（最多可容纳 5 个 tile）
                tileUpdater.Update(tileNotification);


                // 更新此 SecondaryTile 对象的相关信息
                secondaryTile.Arguments = $"Arguments（TileId: {secondaryTile.TileId}）{DateTime.Now.ToString("HH:mm:ss")}";            
                bool success = await secondaryTile.UpdateAsync();
            }
        }

        // 取消固定所有的 SecondaryTile
        private async void btnUnpinSecondaryTile_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<SecondaryTile> secondaryTileList = await SecondaryTile.FindAllAsync();
            foreach (SecondaryTile secondaryTile in secondaryTileList)
            {
                try
                {
                    bool isUnpinned = await secondaryTile.RequestDeleteAsync();
                    lblMsg.Text = isUnpinned ? "取消固定成功" : "取消固定失败";
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "取消固定失败: " + ex.ToString();
                }
            }
        }
    }
}
