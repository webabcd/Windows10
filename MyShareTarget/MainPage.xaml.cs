/*
 * 本例用于演示如何开发一个分享的分享目标（分享源的示例参见 /Windows10/App2AppCommunication/ShareSource.xaml.cs）
 * 1、在 Package.appxmanifest 中新增一个“共享目标”声明，并做相关配置（支持的文件类型，支持的分享数据的格式），类似如下
 * <Extensions>
 *   <uap:Extension Category="windows.shareTarget">
 *     <uap:ShareTarget>
 *       <uap:SupportedFileTypes>
 *         <uap:SupportsAnyFileType />
 *       </uap:SupportedFileTypes>
 *       <uap:DataFormat>Text</uap:DataFormat>
 *       <uap:DataFormat>WebLink</uap:DataFormat>
 *       <uap:DataFormat>ApplicationLink</uap:DataFormat>
 *       <uap:DataFormat>Html</uap:DataFormat>
 *       <uap:DataFormat>Bitmap</uap:DataFormat>
 *       <uap:DataFormat>http://webabcd/sharedemo</uap:DataFormat>
 *     </uap:ShareTarget>
 *   </uap:Extension>
 * </Extensions>
 * 2、在 App.xaml.cs 中 override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)，以获取相关的分享信息
 * 
 * 
 * ShareTargetActivatedEventArgs - 当 app 由分享激活时的事件参数
 *     ShareOperation - 返回一个 ShareOperation 类型的对象
 *     PreviousExecutionState - 此 app 被分享激活前的执行状态（ApplicationExecutionState 枚举：NotRunning, Running, Suspended, Terminated, ClosedByUser）
 *     SplashScreen - 启动画面对象
 * 
 * ShareOperation - 分享操作的相关信息
 *     Data - 返回 DataPackageView 对象，其相当于 DataPackage 的一个只读副本
 *     ReportCompleted(), ReportStarted(), ReportDataRetrieved(), ReportError(), ReportSubmittedBackgroundTask() - 顾名思义的一些方法，用不用皆可，它们的作用是可以帮助系统优化资源的使用
 * 
 * DataPackageView - DataPackage 对象的只读版本，从剪切板获取数据或者分享目标接收数据均通过此对象来获取 DataPackage 对象的数据
 *     AvailableFormats - DataPackage 中数据所包含的所有格式
 *     Contains() - 是否包含指定格式的数据
 *     Properties - 返回 DataPackagePropertySetView 对象，就是由分享源所设置的一些信息
 *     Properties.Title - 分享数据的标题
 *         Properties.Description - 分享数据的描述
 *         Properties.Thumbnail - 分享数据的缩略图
 *         ApplicationName - 分享源的 app 的名称
 *         PackageFamilyName - 分享源的 app 的包名
 *         ApplicationListingUri - 分享源的 app 在商店中的地址
 *         ContentSourceApplicationLink - 内容源的 ApplicationLink
 *         ContentSourceWebLink - 内容源的 WebLink
 *         Square30x30Logo - 分享源的 app 的 30 x 30 的 logo
 *         LogoBackgroundColor - 分享源的 app 的 logo 的背景色
 *         FileTypes - 获取分享数据中包含的文件类型
 *     GetTextAsync(), GetWebLinkAsync(), GetApplicationLinkAsync(), GetHtmlFormatAsync(), GetBitmapAsync(), GetStorageItemsAsync(), GetDataAsync(), GetResourceMapAsync() - 获取分享过来的各种格式的数据，详细用法见下面的相关 demo（注：一个 DataPackage 可以有多种不同格式的数据）
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MyShareTarget
{
    public sealed partial class MainPage : Page
    {
        private ShareOperation _shareOperation;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取分享激活 app 时的事件参数 ShareTargetActivatedEventArgs，可在 App.xaml.cs 的 override void OnShareTargetActivated(ShareTargetActivatedEventArgs args) 中拿到
            ShareTargetActivatedEventArgs shareTargetActivated = e.Parameter as ShareTargetActivatedEventArgs;
            if (shareTargetActivated == null)
            {
                lblMsg.Text = "为了演示分享目标，请从分享源激活本程序";
                return;
            }
            // 获取分享的 ShareOperation 对象
            _shareOperation = shareTargetActivated.ShareOperation;

            // 异步获取分享数据
            await Task.Factory.StartNew(async () =>
            {
                // 显示分享数据的相关信息
                OutputMessage("Title: " + _shareOperation.Data.Properties.Title);
                OutputMessage("Description: " + _shareOperation.Data.Properties.Description);
                OutputMessage("ApplicationName: " + _shareOperation.Data.Properties.ApplicationName);
                OutputMessage("PackageFamilyName: " + _shareOperation.Data.Properties.PackageFamilyName);
                OutputMessage("ApplicationListingUri: " + _shareOperation.Data.Properties.ApplicationListingUri);
                OutputMessage("ContentSourceApplicationLink: " + _shareOperation.Data.Properties.ContentSourceApplicationLink);
                OutputMessage("ContentSourceWebLink: " + _shareOperation.Data.Properties.ContentSourceWebLink);
                OutputMessage("LogoBackgroundColor: " + _shareOperation.Data.Properties.LogoBackgroundColor);
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    IRandomAccessStreamWithContentType logoStream = await _shareOperation.Data.Properties.Square30x30Logo.OpenReadAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(logoStream);
                    imgLogo.Source = bitmapImage;
                });
                OutputMessage("");

                

                // 如果分享数据中包含 Text 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.Text))
                {
                    try
                    {
                        var text = await _shareOperation.Data.GetTextAsync();
                        OutputMessage("Text: " + text);
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含 WebLink 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.WebLink))
                {
                    try
                    {
                        var uri = await _shareOperation.Data.GetWebLinkAsync();
                        OutputMessage("WebLink: " + uri.AbsoluteUri);
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含 ApplicationLink 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.ApplicationLink))
                {
                    try
                    {
                        var uri = await _shareOperation.Data.GetApplicationLinkAsync();
                        OutputMessage("ApplicationLink: " + uri.AbsoluteUri);
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含 Bitmap 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.Bitmap))
                {
                    try
                    {
                        IRandomAccessStreamReference stream = await _shareOperation.Data.GetBitmapAsync();
                        ShowBitmap(stream);

                        ShowThumbnail(_shareOperation.Data.Properties.Thumbnail);
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含 Html 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.Html))
                {
                    try
                    {
                        // 获取 html 数据
                        var html = await _shareOperation.Data.GetHtmlFormatAsync();
                        OutputMessage("Html: " + html);

                        // 获取 html 中包含的本地资源的引用（如引用的本地图片等），其数据在分享源的 DataPackage.ResourceMap 中设置
                        IReadOnlyDictionary<string, RandomAccessStreamReference> sharedResourceMap = await _shareOperation.Data.GetResourceMapAsync();
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含 StorageItems 格式数据，则显示之
                if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
                {
                    try
                    {
                        var storageItems = await _shareOperation.Data.GetStorageItemsAsync();
                        foreach (var storageItem in storageItems)
                        {
                            OutputMessage("storageItem: " + storageItem.Path);
                        }
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }

                // 如果分享数据中包含“http://webabcd/sharedemo”格式数据，则显示之
                if (_shareOperation.Data.Contains("http://webabcd/sharedemo"))
                {
                    try
                    {
                        var customData = await _shareOperation.Data.GetTextAsync("http://webabcd/sharedemo");
                        OutputMessage("Custom Data: " + customData);
                    }
                    catch (Exception ex)
                    {
                        OutputMessage(ex.ToString());
                    }
                }
            });
        }

        // 在 UI 上输出指定的信息
        async private void OutputMessage(string message)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lblMsg.Text += message;
                lblMsg.Text += Environment.NewLine;
            });
        }

        // 显示图片
        async private void ShowThumbnail(IRandomAccessStreamReference thumbnail)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (thumbnail != null)
                {
                    IRandomAccessStreamWithContentType thumbnailStream = await thumbnail.OpenReadAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(thumbnailStream);

                    imgThumbnail.Source = bitmapImage;
                }
            });
        }

        // 显示图片
        async private void ShowBitmap(IRandomAccessStreamReference bitmap)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (bitmap != null)
                {
                    IRandomAccessStreamWithContentType bitmapStream = await bitmap.OpenReadAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(bitmapStream);

                    imgBitmap.Source = bitmapImage;
                }
            });
        }



        /*
         * 关于 QuickLink 的相关说明如下
         * 注：经测试，在我的 windows 10 环境中，此部分内容无效（应该是 windows 10 已经不再支持 QuickLink 了）
         * 
         * ShareOperation - 分享操作的相关信息
         *     QuickLinkId - 如果是 QuickLink 激活的，此属性可获取此 QuickLink 的 Id
         *     RemoveThisQuickLink() - 如果是 QuickLink 激活的，此方法可删除此 QuickLink
         *     ReportCompleted(QuickLink quicklink) - 指定一个需要增加的 QuickLink 对象，然后通知系统分享操作已经完成（会自动关闭分享面板）
         *     
         * QuickLink - 预定义了一些数据，指向相应分享目标的一个快捷方式，其会出现在分享面板的上部
         *     Id - 预定义数据
         *     Title - QuickLink 出现在分享面板上的时候所显示的名称
         *     Thumbnail - QuickLink 出现在分享面板上的时候所显示的图标
         *     SupportedDataFormats - 分享数据中所包含的数据格式与此定义相匹配（有一个匹配即可），QuickLink 才会出现在分享面板上
         *     SupportedFileTypes - 分享数据中所包含的文件的扩展名与此定义的相匹配（有一个匹配即可），QuickLink 才会出现在分享面板上
         *     
         * QuickLink 的适用场景举例
         * 1、当用户总是分享信息到某一个邮件地址时，便可以在分享目标中以此邮件地址为 QuickLinkId 生成一个 QuickLink
         * 2、下回用户再分享时，此 QuickLink 就会出现在分享面板上，用户在分享面板上可以点击此 QuickLink 激活分享目标
         * 3、分享目标被 QuickLink 激活后便可以通过 ShareOperation 对象获取到 QuickLink，然后就可以拿到用户需要分享到的邮件地址（就是 QuickLink 的 Id），从而避免用户输入此邮件地址，从而方便用户的分享操作
         */

        // 演示如何增加一个 QuickLink
        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            OutputMessage("QuickLinkId: " + _shareOperation.QuickLinkId);
            
            QuickLink quickLink = new QuickLink
            {
                Id = "wanglei@email.com",
                Title = "分享到邮件: wanglei@email.com", // QuickLink 在分享面板上显示的内容

                SupportedFileTypes = { "*" },
                SupportedDataFormats =
                {
                    StandardDataFormats.Text,
                    StandardDataFormats.WebLink,
                    StandardDataFormats.ApplicationLink,
                    StandardDataFormats.Bitmap,
                    StandardDataFormats.StorageItems,
                    StandardDataFormats.Html,
                    "http://webabcd/sharedemo"
                }
            };

            try
            {
                // 设置 QuickLink 在分享面板上显示的图标
                StorageFile iconFile = await Package.Current.InstalledLocation.CreateFileAsync("Assets\\StoreLogo.png", CreationCollisionOption.OpenIfExists);
                quickLink.Thumbnail = RandomAccessStreamReference.CreateFromFile(iconFile);

                // 完成分享操作，同时在分享面板上增加一个指定的 QuickLink
                _shareOperation.ReportCompleted(quickLink);
            }
            catch (Exception ex)
            {
                OutputMessage(ex.ToString());
            }
        }
    }
}
