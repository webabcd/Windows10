/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     ContainsFullScreenElementChanged - WebView 中的内容进入全屏或退出全屏时触发的事件
 *     ContainsFullScreenElement - WebView 中的内容是否处于全屏状态
 *     UnsupportedUriSchemeIdentified - 在尝试导航至 WebView 不支持的协议的 uri 时触发的事件
 *     UnviewableContentIdentified - 在尝试导航至 WebView 不支持的类型的文件时触发的事件
 * 
 * 
 * 本例用于演示
 * 1、如何监听 WebView 中的内容的进入全屏和退出全屏的事件，以及如何获知当前 WebView 中的内容是否处于全屏状态
 * 2、如何监听 WebView 在尝试导航至不支持的协议的 uri 时触发的事件
 * 3、如何监听 WebView 在尝试导航至不支持的类型的文件时触发的事件
 */

using System;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo7 : Page
    {
        public WebViewDemo7()
        {
            this.InitializeComponent();

            webView1.ContainsFullScreenElementChanged += WebView1_ContainsFullScreenElementChanged;
            webView2.UnsupportedUriSchemeIdentified += WebView2_UnsupportedUriSchemeIdentified;
            webView3.UnviewableContentIdentified += WebView3_UnviewableContentIdentified;
        }

        // WebView 中的内容进入全屏或退出全屏时触发的事件
        private void WebView1_ContainsFullScreenElementChanged(WebView sender, object args)
        {
            ApplicationView applicationView = ApplicationView.GetForCurrentView();

            // WebView 中的内容处于全屏状体
            if (sender.ContainsFullScreenElement)
            {
                // 将 app 设置为全屏模式
                applicationView.TryEnterFullScreenMode();
            }
            else
            {
                // 将 app 退出全屏模式
                applicationView.ExitFullScreenMode();
            }
        }

        // 在尝试导航至 WebView 不支持的协议的 uri 时触发的事件
        private async void WebView2_UnsupportedUriSchemeIdentified(WebView sender, WebViewUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            // 交由我处理吧（否则的话系统会弹出对话框，以便跳转至支持此协议的其他 app 或者在商店搜索支持此协议的 app）
            args.Handled = true;

            // 尝试导航至的 uri
            Uri myUri = args.Uri;
            await new MessageDialog(myUri.ToString(), "自定义 uri").ShowAsync();
        }

        // 在尝试导航至 WebView 不支持的类型的文件时触发的事件
        private async void WebView3_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            // 文件类型，本例中此值为 "application/pdf"
            string mediaType = args.MediaType;

            // 尝试导航至的 uri（本例中此值为 https://www.apple.com/cn/iphone/business/docs/iOS_Security_Guide.pdf）
            Uri uri = args.Uri;

            // uri 的 referrer（本例中此值为 https://www.apple.com/cn/iphone/business/docs/iOS_Security_Guide.pdf 并不是 uri 的 referrer，为啥？）
            Uri referrer = args.Referrer;

            if (args.Uri.AbsolutePath.EndsWith(".pdf"))
            {
                // 通过 launcher 打开 pdf 文件
                if (await Launcher.LaunchUriAsync(args.Uri))
                {
                  
                }
                else
                {
                
                }
            }
        }
    }
}
