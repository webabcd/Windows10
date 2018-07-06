/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     NewWindowRequested - 在尝试用新开窗口打开 uri 时触发的事件
 *     PermissionRequested - 在尝试获取特殊权限时触发的事件，比如地理位置等
 * 
 * 
 * 本例用于演示
 * 1、如何监听 WebView 在尝试用新开窗口打开 uri 时触发的事件
 * 2、如何监听 WebView 在尝试获取特殊权限时触发的事件，比如地理位置等
 */

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo8 : Page
    {
        public WebViewDemo8()
        {
            this.InitializeComponent();

            webView1.NewWindowRequested += WebView1_NewWindowRequested;
            webView2.PermissionRequested += WebView2_PermissionRequested;
        }

        // 在尝试用新开窗口打开 uri 时触发的事件
        private async void WebView1_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            // 交由我处理吧（否则的话系统会用浏览器打开）
            args.Handled = true;

            // 需要新开窗口的 uri（本例中此值为 https://www.baidu.com/）
            Uri uri = args.Uri;

            // uri 的 referrer（本例中此值为 https://www.baidu.com/ 并不是 uri 的 referrer，为啥？）
            Uri referrer = args.Referrer;

            await new MessageDialog(uri.ToString(), "需要新开窗口的 uri").ShowAsync();
        }

        // 在尝试获取特殊权限时触发的事件，比如地理位置等
        private void WebView2_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            /*
             * WebViewPermissionRequest - 特殊权限请求对象
             *     PermissionType - 特殊权限类型
             *     WebViewPermissionState - 特殊权限请求的状态（Unknown, Defer, Allow, Deny）
             *     Uri - 请求特殊权限的 uri
             *     Allow() - 授予请求的权限
             *     Deny() - 拒绝请求的权限
             *     Defer() - 延迟决定是否授予
             */
            WebViewPermissionRequest permissionRequest = args.PermissionRequest;
        }
    }
}
