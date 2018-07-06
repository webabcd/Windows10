/*
 * WebView - 内嵌浏览器控件（继承自 FrameworkElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     
 *     
 * 本例用于演示 
 * 1、WebView 如何加载指定的 html
 * 2、WebView 如何加载指定的 url（http, https, ms-appx-web:///, embedded resource, ms-appdata:///）
 * 3、WebView 结合 IUriToStreamResolver 可以实现自定义所有请求（包括 html 的 url 以及 html 内所有引用的 url）返回的内容（ms-local-stream://）
 */

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web;

namespace Windows10.Controls.WebViewDemo
{
    public sealed partial class WebViewDemo2 : Page
    {
        public WebViewDemo2()
        {
            this.InitializeComponent();
        }

        private void btnNavigate_Click(object sender, RoutedEventArgs e)
        {
            // 通过 Navigate 加载指定的 url
            webView.Navigate(new Uri("http://webabcd.cnblogs.com/", UriKind.Absolute));
        }

        private void btnSource_Click(object sender, RoutedEventArgs e)
        {
            // 通过 Source 获取或设置当前的 url
            webView.Source = new Uri("https://www.baidu.com", UriKind.Absolute);
        }

        private void btnNavigateToString_Click(object sender, RoutedEventArgs e)
        {
            // 通过 NavigateToString 加载指定的 html
            webView.NavigateToString("<b>i am webabcd</b>");
        }


        private void btnMsAppxWeb_Click(object sender, RoutedEventArgs e)
        {
            // 加载指定的 ms-appx-web:/// 协议地址（Package 内的数据）
            webView.Navigate(new Uri("ms-appx-web:///Controls/WebViewDemo/demo1.html", UriKind.Absolute));
        }

        private void btnEmbeddedResource_Click(object sender, RoutedEventArgs e)
        {
            // 获取 Package 内嵌入式资源数据
            Assembly assembly = typeof(WebViewDemo2).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Windows10.Controls.WebViewDemo.demo2.html");

            using (StreamReader reader = new StreamReader(stream))
            {
                string html = reader.ReadToEnd();
                webView.NavigateToString(html);
            }
        }

        private async void btnMsAppdata_Click(object sender, RoutedEventArgs e)
        {
            // 将程序包内的 html 文件复制到 ApplicationData 中的 LocalFolder
            StorageFolder localFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("webabcdTest", CreationCollisionOption.OpenIfExists);
            StorageFile htmlFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Controls/WebViewDemo/demo3.html"));
            await htmlFile.CopyAsync(localFolder, "demo3.html", NameCollisionOption.ReplaceExisting);

            // 加载指定的 ms-appdata:/// 协议地址（Application 内的数据）
            string url = "ms-appdata:///local/webabcdTest/demo3.html";
            webView.Navigate(new Uri(url));
        }

        private void btnMsLocalStream_Click(object sender, RoutedEventArgs e)
        {
            // 构造可传递给 NavigateToLocalStreamUri() 的 URI（即 ms-local-stream:// 协议的 URI）
            Uri url = webView.BuildLocalStreamUri("contentIdentifier", "/Controls/WebViewDemo/demo4.html");

            // 在我的示例中，上面方法返回的 url 如下（协议是: ms-local-stream://appname_KEY/folder/file, 其中的 KEY 会根据 contentIdentifier 的不同而不同）
            // "ms-local-stream://48c7dd54-1ef2-4db7-a75e-7e02c5eefd40_636f6e74656e744964656e746966696572/Controls/WebViewDemo/demo4.html"

            // 实例化一个实现 IUriToStreamResolver 接口的类
            StreamUriWinRTResolver myResolver = new StreamUriWinRTResolver();
            // 所有 url（包括 html 的 url 以及 html 内所有引用的 url）都会通过 StreamUriWinRTResolver 来返回数据流
            webView.NavigateToLocalStreamUri(url, myResolver);
        }
    }


    // 实现 IUriToStreamResolver 接口（用于响应所有 url 请求，包括 html 的 url 以及 html 内所有引用的 url）
    // 可以认为这就是一个为 WebView 服务的 http server
    public sealed class StreamUriWinRTResolver : IUriToStreamResolver
    {
        // IUriToStreamResolver 接口只有一个需要实现的方法
        // 根据当前请求的 uri 返回对应的内容流
        public IAsyncOperation<IInputStream> UriToStreamAsync(Uri uri)
        {
            return GetContent(uri).AsAsyncOperation();
        }

        // 根据当前请求的 uri 返回对应的内容流
        private async Task<IInputStream> GetContent(Uri uri)
        {
            string path = uri.AbsolutePath;
            string responseString = "";

            switch (path)
            {
                case "/Controls/WebViewDemo/demo4.html":
                    StorageFile fileRead = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://" + path, UriKind.Absolute));
                    return await fileRead.OpenAsync(FileAccessMode.Read);

                case "/Controls/WebViewDemo/css.css":
                    responseString = "b { color: red; }";
                    break;

                default:
                    break;
            }

            // string 转 IInputStream
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(responseString, BinaryStringEncoding.Utf8);
            var stream = new InMemoryRandomAccessStream();
            await stream.WriteAsync(buffer);
            return stream.GetInputStreamAt(0);
        }
    }
}