/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     CaptureSelectedContentToDataPackageAsync() - 将选中的内容转换为 DataPackage 对象
 *     DataRequested - 分享操作开始时触发的事件（事件参数 DataRequestedEventArgs）
 * 
 * DataRequestedEventArgs
 *     GetDeferral() - 获取异步操作对象，同时开始异步操作，之后通过 Complete() 通知完成异步操作
 * 
 * 
 * 本例用于演示如何通过 Share Contract 分享 WebView 中的被选中的内容（如果没有选中任何内容，则分享网页地址）
 */

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo6 : Page
    {
        private DataTransferManager _dataTransferManager;

        public WebViewDemo6()
        {
            this.InitializeComponent();
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += _dataTransferManager_DataRequested;

            DataTransferManager.ShowShareUI();
        }

        // 分享 WebView 中的被选中的内容
        async void _dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            DataRequestDeferral deferral = args.Request.GetDeferral();

            // 如果 dataPackage 是 null 的话，则说明用户没有选择任何内容
            DataPackage dataPackage = await webView.CaptureSelectedContentToDataPackageAsync();

            // 用于判断用户是否选中了分享内容
            bool hasSelection = false;
            try
            {
                hasSelection = (dataPackage != null) && (dataPackage.GetView().AvailableFormats.Count > 0);
            }
            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    // 无法为选中的内容生成 data package
                    case unchecked((int)0x80070490):
                        hasSelection = false;
                        break;
                    default:
                        throw;
                }
            }

            if (hasSelection)
            {
                dataPackage.Properties.Title = "Title（hasSelection）";
            }
            else
            {
                // 用户没有选择任何内容的话，则分享网页地址
                dataPackage = new DataPackage();
                dataPackage.SetWebLink(webView.Source);
                dataPackage.Properties.Title = "Title";
            }

            dataPackage.Properties.Description = "Description";
            request.Data = dataPackage;

            _dataTransferManager.DataRequested -= _dataTransferManager_DataRequested;

            deferral.Complete();
        }
    }
}
