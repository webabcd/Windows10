/*
 * 本例用于演示如何开发一个分享的分享源（分享目标的示例参见 /MyShareTarget/MainPage.xaml.cs）
 *
 * 分享源 - 提供分享数据的 app
 * 分享目标 - 接收并处理分享数据的 app
 * 分享面板 - 点击“分享”后出来的，包含了一堆分享目标的面板
 *
 *
 * DataTransferManager - 分享数据管理器
 *     GetForCurrentView() - 返回当前窗口关联的 DataTransferManager 对象
 *     ShowShareUI() -  弹出分享面板，以开始分享操作
 *     DataRequested - 分享操作开始时（即弹出分享面板后）所触发的事件，事件参数为 DataTransferManager 和 DataRequestedEventArgs
 *     TargetApplicationChosen - 选中了分享面板上的某个分享目标时所触发的事件，事件参数为 DataTransferManager 和 TargetApplicationChosenEventArgs
 * 
 * TargetApplicationChosenEventArgs - TargetApplicationChosen 的事件参数
 *     ApplicationName - 选中的分享目标的名称
 *     
 * DataRequestedEventArgs - DataRequested 的事件参数
 *     Request - 返回 DataRequest 类型的数据
 *     
 * DataRequest - 一个对象，其包括了分享的内容和错误提示
 *     FailWithDisplayText() - 当需要分享的数据不合法时，需要在分享面板上显示的提示信息
 *     Data - 需要分享的内容，返回 DataPackage 对象
 *         
 * DataPackage - 分享内容（注：复制到剪切板的内容也是通过此对象来封装）
 *     Properties - 返回 DataPackagePropertySetView 对象
 *         Properties.Title - 分享数据的标题
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
 *     SetText(), SetWebLink(), SetApplicationLink(), SetHtmlFormat(), SetBitmap(), SetStorageItems(), SetData(), SetDataProvider() - 设置需要分享的各种格式的数据，详细用法见下面的相关 demo（注：一个 DataPackage 可以有多种不同格式的数据）
 *     ResourceMap - IDictionary<string, RandomAccessStreamReference> 类型，分享 html 时如果其中包含了本地资源的引用（如引用了本地图片），则需要通过 ResourceMap 传递
 *     GetView() - 返回 DataPackageView 对象，其相当于 DataPackage 的一个只读副本，详细说明见 /MyShareTarget/MainPage.xaml.cs
 *     
 *     
 * 异步分享：
 * 1、DataPackage 通过 SetDataProvider() 传递数据，其对应的委托的参数为一个 DataProviderRequest 类型的数据
 * 2、DataProviderRequest.GetDeferral() 用于获取 DataProviderDeferral 对象以开始异步处理，然后通过 DataProviderDeferral.Complete() 完成异步操作
 * 
 * 
 * 注：
 * 一个 DataPackage 可以包含多种不同格式的数据（本例为了演示的清晰，每次只会分享一种格式的数据）
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.Graphics.Imaging;
using Windows.UI;

namespace Windows10.App2AppCommunication
{
    public sealed partial class ShareSource : Page
    {
        // 当前需要分享的内容的类型
        private string _shareType = "Share Text";
        // 需要分享的文件集合
        private IReadOnlyList<StorageFile> _selectedFiles;


        public ShareSource()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 初始化 DataTransferManager
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;
            dataTransferManager.TargetApplicationChosen += dataTransferManager_TargetApplicationChosen;
        }

        // 分享操作开始时（即弹出分享面板后）
        void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // 根据需要分享的内容的类型执行指定的方法
            switch (_shareType)
            {
                case "Share Text":
                    ShareText(sender, args);
                    break;
                case "Share Web Link":
                    ShareWebLink(sender, args);
                    break;
                case "Share Application Link":
                    ShareApplicationLink(sender, args);
                    break;
                case "Share Image":
                    ShareImage(sender, args);
                    break;
                case "Share File":
                    ShareFile(sender, args);
                    break;
                case "Share With Deferral":
                    ShareWithDeferral(sender, args);
                    break;
                case "Share Html":
                    ShareHtml(sender, args);
                    break;
                case "Share Custom Protocol":
                    ShareCustomProtocol(sender, args);
                    break;
                case "Share Failed":
                    ShareFailed(sender, args);
                    break;
                default:
                    break;
            }
        }

        // 选中了分享面板上的某个分享目标时
        void dataTransferManager_TargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            // 显示用户需要与其分享内容的应用程序的名称
            lblMsg.Text = "分享给：" + args.ApplicationName;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            _shareType = (sender as Button).Content.ToString();

            // 如果需要分享文件，则提示用户选择文件
            if (_shareType == "Share Image" || _shareType == "Share File" || _shareType == "Share With Deferral")
            {
                FileOpenPicker filePicker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                    FileTypeFilter = { "*" }
                    // FileTypeFilter = { ".jpg", ".png", ".bmp", ".gif", ".tif" }
                };

                _selectedFiles = await filePicker.PickMultipleFilesAsync();
                if (_selectedFiles.Count > 0)
                {
                    // 弹出分享面板，以开始分享操作
                    DataTransferManager.ShowShareUI();
                }
            }
            else
            {
                // 弹出分享面板，以开始分享操作
                DataTransferManager.ShowShareUI();
            }
        }

        // 分享文本的 Demo
        private void ShareText(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);
            dataPackage.SetText("需要分享的详细内容");
        }

        // 分享 WebLink 的 Demo
        private void ShareWebLink(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);
            dataPackage.SetWebLink(new Uri("http://webabcd.cnblogs.com"));
        }

        // 分享 ApplicationLink 的 Demo
        private void ShareApplicationLink(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);
            dataPackage.SetApplicationLink(new Uri("webabcd:data"));
        }

        // 分享图片的 Demo（关于如何为分享的图片减肥请参见本例的“异步分享的 Demo”）
        private void ShareImage(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);

            // 分享选中的所有文件中的第一个文件（假设其是图片）
            RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(_selectedFiles.First());
            dataPackage.Properties.Thumbnail = imageStreamRef;
            dataPackage.SetBitmap(imageStreamRef);
        }

        // 分享文件的 Demo
        private void ShareFile(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);
            dataPackage.SetStorageItems(_selectedFiles);
        }

        // 分享 html 的 Demo
        private void ShareHtml(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            string localImage = "ms-appx:///Assets/StoreLogo.png";
            string htmlExample = "<p><b>webabcd</b><img src=\"" + localImage + "\" /></p>";
            // 为 html 添加分享所需的必要的标头，以保证可以正常进行 html 的分享操作
            string htmlFormat = HtmlFormatHelper.CreateHtmlFormat(htmlExample);

            DataPackage dataPackage = GetDataPackage(args);
            dataPackage.SetHtmlFormat(htmlFormat);

            // 设置本地图像数据（如果需要分享的 html 包含本地图像，则只能通过这种方法分享之）
            RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(new Uri(localImage));
            dataPackage.ResourceMap[localImage] = streamRef;

            /*
             * 以下演示如何分享 WebView 中的被用户选中的 html
             * 具体可参见：Controls/WebViewDemo/WebViewDemo6.xaml
             * 
            DataPackage dataPackage = WebView.DataTransferPackage;
            DataPackageView dataPackageView = dataPackage.GetView();

            if ((dataPackageView != null) && (dataPackageView.AvailableFormats.Count > 0))
            {
                dataPackage.Properties.Title = "Title";
                dataPackage.Properties.Description = "Description";
            
                args.Request.Data = dataPackage;
            }
            */
        }

        // 分享自定义协议的 Demo
        private void ShareCustomProtocol(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);

            // 指定需要分享的自定义协议和自定义数据，第一个参数是自定义数据的格式 id，也就是自定义协议（要使用标准格式 id 的话，就是 Windows.ApplicationModel.DataTransfer.StandardDataFormats 枚举）
            // 需要在 SourceTarget 的 Package.appxmanifest 中的“共享目标”声明中对自定义格式 id 做相应的配置
            dataPackage.SetData("http://webabcd/sharedemo", "自定义数据");
        }

        // 异步分享的 Demo（在分享内容需要较长时间才能计算出来的场景下，应该使用异步分享）
        private void ShareWithDeferral(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            DataPackage dataPackage = GetDataPackage(args);

            dataPackage.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(_selectedFiles.First());
            // 通过委托来提供分享数据，当用户点击了分享目标后会调用此委托，即不马上提供分享数据，而是等到用户点击了分享目标后再异步准备数据
            dataPackage.SetDataProvider(StandardDataFormats.Bitmap, providerRequest => this.OnDeferredImageRequestedHandler(providerRequest, _selectedFiles.First()));
        }

        // 用户点击了分享目标后会调用此方法
        private async void OnDeferredImageRequestedHandler(DataProviderRequest providerRequest, StorageFile imageFile)
        {
            // 获取 DataProviderDeferral，以开始异步处理
            DataProviderDeferral deferral = providerRequest.GetDeferral();
            InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();

            try
            {
                // 将用户选中的图片缩小一倍，然后再分享
                IRandomAccessStream imageStream = await imageFile.OpenAsync(FileAccessMode.Read);
                BitmapDecoder imageDecoder = await BitmapDecoder.CreateAsync(imageStream);
                BitmapEncoder imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
                imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * 0.5);
                imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * 0.5);
                await imageEncoder.FlushAsync();

                // 停 3 秒，以模拟长时间任务
                await Task.Delay(3000);

                providerRequest.SetData(RandomAccessStreamReference.CreateFromStream(inMemoryStream));
            }
            finally
            {
                // 完成异步操作
                deferral.Complete();
            }
        }

        // 需要分享的数据不合法时如何处理的 Demo
        private void ShareFailed(DataTransferManager dtm, DataRequestedEventArgs args)
        {
            // 判断需要分享的数据是否合法，不合法的话就调用下面的方法，用以在分享面板上显示指定的提示信息
            if (true)
            {
                args.Request.FailWithDisplayText("需要分享的数据不合法，分享操作失败");
            }
        }



        // 用于设置分享的数据
        private DataPackage GetDataPackage(DataRequestedEventArgs args)
        {
            DataPackage dataPackage = args.Request.Data;

            dataPackage.Properties.Title = "Title";
            dataPackage.Properties.Description = "Description";
            dataPackage.Properties.ContentSourceApplicationLink = new Uri("webabcd:data");
            dataPackage.Properties.ContentSourceWebLink = new Uri("http://webabcd.cnblogs.com/");

            // 下面这些数据不指定的话，系统会自动为其设置好相应的值
            dataPackage.Properties.ApplicationListingUri = new Uri("https://www.microsoft.com/store/apps/9pd68q7017mw");
            dataPackage.Properties.ApplicationName = "ApplicationName";
            dataPackage.Properties.PackageFamilyName = "PackageFamilyName";
            dataPackage.Properties.Square30x30Logo = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/StoreLogo.png"));
            dataPackage.Properties.LogoBackgroundColor = Colors.Red;

            return dataPackage;
        }
    }
}
