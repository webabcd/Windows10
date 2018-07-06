/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     
 *     
 * 本例用于演示 
 * 1、WebView 如何加载指定 HttpMethod 的请求
 * 2、WebView 如何自定义请求的 http header
 */
 
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo3 : Page
    {
        public WebViewDemo3()
        {
            this.InitializeComponent();

            this.Loaded += WebViewDemo3_Loaded;
        }

        private void WebViewDemo3_Loaded(object sender, RoutedEventArgs e)
        {
            // 实例化 HttpRequestMessage（可以指定请求的 HttpMethod 以及自定义请求的 http header）
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri("http://localhost:44914/api/webviewpost"));

            // 构造 post 数据
            httpRequestMessage.Content = new HttpStringContent("hello webabcd");

            // 自定义 http header
            httpRequestMessage.Headers.Append("myHeader", "hello header");

            // 通过 NavigateWithHttpRequestMessage 加载指定的 HttpRequestMessage 对象
            webView.NavigateWithHttpRequestMessage(httpRequestMessage);
        }
    }
}
