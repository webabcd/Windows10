/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     InvokeScriptAsync() - 调用指定的 js 函数，并返回 js 函数的返回值
 *     ScriptNotify - 收到 js 通过 window.external.notify('') 传递过来的数据时触发的事件
 *     AllowedScriptNotifyUris - 允许触发 ScriptNotify 事件的 uri 列表
 *     AddWebAllowedObject() - 将 windows runtime component 中定义的对象注册到 WebView 加载的页面（需要在 WebView 的 NavigationStarting 事件中调用）
 *
 * 
 * 本例用于演示
 * 1、app 与 js 的交互
 * 2、如何将 windows runtime component 中定义的对象注册到 WebView 加载的页面，以便通过 js 调用 wrc
 */

/*
 * 特别注意：各种 uri schema 对于 ScriptNotify 的支持情况如下
 * 1、http:// 不支持
 * 2、https:// 支持，需要在 appxmanifest 中设置允许的 URI（http 的不行，只能是 https 的），也可以通过 WebView 的 AllowedScriptNotifyUris 属性来设置或获取
 *    <ApplicationContentUriRules>
 *       <Rule Match="https://aaa.aaa.aaa" Type="include" />
 *    </ApplicationContentUriRules>
 * 3、ms-appdata:/// 不支持
 * 4、ms-appx-web:/// 支持
 * 5、ms-local-stream:// 支持
 */

using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo4 : Page
    {
        public WebViewDemo4()
        {
            this.InitializeComponent();

            webView.ScriptNotify += webView_ScriptNotify;
            webView.NavigationCompleted += webView_NavigationCompleted;

            // 由于本例的 webview 是在 xaml 中声明并指定需要加载的 html 的，所以对于 NavigationStarting 事件的注册来说要在 xaml 做
            // webView.NavigationStarting += WebView_NavigationStarting;
        }

        void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (args.IsSuccess)
            {
                // 获取 html 标题
                lblMsg.Text = "html title: " + webView.DocumentTitle;
                lblMsg.Text += Environment.NewLine;

                // 获取或设置 html 背景色
                webView.DefaultBackgroundColor = Colors.Orange;
                lblMsg.Text += "html backgroundColor: " + webView.DefaultBackgroundColor.ToString();
            }
        }

        // 收到 js 通过 window.external.notify('') 传递过来的数据时触发的事件
        private async void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            // e.Value - 获取 js 传递过来的数据
            // e.CallingUri - 触发此事件的页面的 uri

            if (e.Value.StartsWith("alert:"))
            {
                // 模拟弹出页面的 alert 框（参见下面的 WebView_NavigationCompleted 中的代码）
                await new MessageDialog(e.Value.Substring(6), "alert").ShowAsync();
            }
            else
            {
                await new MessageDialog(e.CallingUri.ToString() + " " + e.Value).ShowAsync();
            }
        }

        // app 调用 js
        private async void btnAppToJavaScript_Click(object sender, RoutedEventArgs e)
        {
            List<string> arguments = new List<string> { "webabcd" };
            // 调用 js 方法：sayHelloToJs('webabcd'); 并返回结果
            string result = await webView.InvokeScriptAsync("appToJs", arguments);

            await new MessageDialog(result).ShowAsync();
        }

        // 通过 eval 访问 DOM
        private async void btnEval_Click(object sender, RoutedEventArgs e)
        {
            // 设置 document.title 的值（用于演示如何通过 eval 设置 DOM）
            List<string> arguments = new List<string> { "document.title = 'hahaha';" };
            await webView.InvokeScriptAsync("eval", arguments);

            // 获取 document.title 的值（用于演示如何通过 eval 获取 DOM）
            arguments = new List<string> { "document.title;" };
            string result = await webView.InvokeScriptAsync("eval", arguments);
            await new MessageDialog(result).ShowAsync();
        }

        // 通过 eval 向 html 注册 JavaScript 脚本
        private async void btnRegisterJavaScript_Click(object sender, RoutedEventArgs e)
        {
            // 向 html 注册脚本
            List<string> arguments = new List<string> { "function xxx(){return '由 app 向 html 注册的脚本返回的数据';}" };
            await webView.InvokeScriptAsync("eval", arguments);

            // 调用刚刚注册的脚本
            string result = await webView.InvokeScriptAsync("xxx", null);

            await new MessageDialog(result).ShowAsync();
        }

        // js 调用 wrc
        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // 注意：需要在 WebView 的 NavigationStarting 事件中调用 AddWebAllowedObject()
            webView.AddWebAllowedObject("js2wrc_object", new MyRuntimeComponent.JS2WRC());
        }

        // 模拟弹出页面的 alert 框
        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // 在 uwp 的 webview 中是不能弹出 alert 框的
            // 可以向页面注入脚本重写 window.alert 函数，使其调用 window.external.notify 通知 c# 端，然后弹出 MessageDialog 框以模拟页面的 alert 框（参见上面的 webView_ScriptNotify 中的代码）
            // 在 uwp 的 webview 中也是不能弹出 confirm 框的，也不能像实现 alert 框那样如法炮制，因为 JavaScript 是运行在单线程上的，其不会等待 c# 调用的结果，所以如何实现 confirm 框还是另想办法吧
            await webView.InvokeScriptAsync("eval", new string[] { "window.alert = function (alertMessage) {window.external.notify('alert:' + alertMessage)}" });
        }
    }
}