/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     
 *     
 * 本例用于演示 WebView 的基础知识
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo1 : Page
    {
        public WebViewDemo1()
        {
            this.InitializeComponent();
        }

        private void btnNavigateUrl_Click(object sender, RoutedEventArgs e)
        {
            // 加载指定的 url
            webView.Navigate(new Uri("http://webabcd.cnblogs.com/", UriKind.Absolute));

            // 获取或设置浏览器的 url
            // webView.Source = new Uri("http://webabcd.cnblogs.com/", UriKind.Absolute);

            // web 页面中的某一个 frame 加载前
            webView.FrameNavigationStarting += webView_FrameNavigationStarting;
            // web 页面中的某一个 frame 加载中
            webView.FrameContentLoading += webView_FrameContentLoading;
            // web 页面中的某一个 frame 的 DOM 加载完成
            webView.FrameDOMContentLoaded += webView_FrameDOMContentLoaded;
            // web 页面中的某一个 frame 导航完成（成功或失败）
            webView.FrameNavigationCompleted += webView_FrameNavigationCompleted;

            // web 页面加载前
            webView.NavigationStarting += webView_NavigationStarting;
            // web 页面加载中
            webView.ContentLoading += webView_ContentLoading;
            // web 页面的 DOM 加载完成
            webView.DOMContentLoaded += webView_DOMContentLoaded;
            // web 页面导航完成（成功或失败）
            webView.NavigationCompleted += webView_NavigationCompleted;

            // 当脚本运行时，可能会导致 app 无响应。此事件会定期执行，然后可查看 ExecutionTime 属性，如果要暂停脚本，则将 StopPageScriptExecution 属性设置为 true 即可
            webView.LongRunningScriptDetected += webView_LongRunningScriptDetected;
            // 在 WebView 对 SmartScreen 筛选器报告为不安全的内容显示警告页面时发生
            webView.UnsafeContentWarningDisplaying += webView_UnsafeContentWarningDisplaying;
            // 在 WebView 尝试下载不受支持的文件时发生
            webView.UnviewableContentIdentified += webView_UnviewableContentIdentified;


            // 用于导航 web 的一系列 api，顾名思义，不解释了
            // webView.CanGoBack;
            // webView.GoBack();
            // webView.CanGoForward;
            // webView.GoForward();
            // webView.Stop();
            // webView.Refresh();

            // 设置焦点
            // webView.Focus(FocusState.Programmatic);

            // 清除 WebView 的缓存和 IndexedDB 数据
            // await WebView.ClearTemporaryWebDataAsync();

            // WebView 在实例化时可以指定其是托管在 UI 线程（WebViewExecutionMode.SameThread）还是托管在非 UI 线程（WebViewExecutionMode.SeparateThread），默认是托管在 UI 线程的
            // WebView wv = new WebView(WebViewExecutionMode.SeparateThread);

            // 获知 WebView 是托管在 UI 线程还是非 UI 线程
            // WebViewExecutionMode executionMode = webView.ExecutionMode;
        }


        void webView_FrameNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // 是否取消此 url 的加载
            // args.Cancel = true;

            // args.Uri
        }
        void webView_FrameContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            // args.Uri
        }
        void webView_FrameDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            // args.Uri
        }
        void webView_FrameNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // 导航是否成功
            // args.IsSuccess

            // 导航失败时，失败原因的 WebErrorStatus 枚举
            // args.WebErrorStatus
        }


        void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // 是否取消此 url 的加载
            // args.Cancel = true;

            // args.Uri
        }
        void webView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            // args.Uri
        }
        void webView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            // args.Uri
        }
        void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // 导航是否成功
            // args.IsSuccess

            // 导航失败时，失败原因的 WebErrorStatus 枚举
            // args.WebErrorStatus
        }


        // 在 WebView 尝试下载不受支持的文件时发生
        void webView_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            // 引用页
            // args.Referrer

            // args.Uri
        }

        // 在 WebView 对 SmartScreen 筛选器报告为不安全的内容显示警告页面时发生
        void webView_UnsafeContentWarningDisplaying(WebView sender, object args)
        {

        }

        // 当脚本运行时，可能会导致 app 无响应。此事件会定期执行，然后可查看 ExecutionTime 属性，如果要暂停脚本，则将 StopPageScriptExecution 属性设置为 true 即可
        void webView_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            // 获取 WebView 执行的一个长时间运行的脚本的运行时间（单位：毫秒）
            // args.ExecutionTime

            // 是否暂停在 WebView 中执行的已运行很长时间的脚本
            // args.StopPageScriptExecution
        }
    }
}
