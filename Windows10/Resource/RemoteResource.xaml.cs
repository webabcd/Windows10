/*
 * 演示如何加载并使用外部的 ResourceDictionary
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.Web.Http;

namespace Windows10.Resource
{
    public sealed partial class RemoteResource : Page
    {
        // 需要加载的 ResourceDictionary 的 http 地址
        string resourceDictionaryUrl = "http://localhost:44914/xaml/ResourceDictionary.txt";

        public RemoteResource()
        {
            this.InitializeComponent();
        }

        private async void btnLoadRemoteResource_Click(object sender, RoutedEventArgs e)
        {
            // 下载远程的 ResourceDictionary 文件
            HttpClient client = new HttpClient();
            string resourceDictionaryString = await client.GetStringAsync(new Uri(resourceDictionaryUrl, UriKind.Absolute));

            // 将字符串转换为 ResourceDictionary 对象
            ResourceDictionary resourceDictionary = XamlReader.Load(resourceDictionaryString) as ResourceDictionary;

            // 将指定的 ResourceDictionary 集成到 Page.Resources 内的资源字典中
            this.Resources.MergedDictionaries.Add(resourceDictionary);

            // 使用远程 ResourceDictionary 中的资源
            textBlock.Foreground = (SolidColorBrush)this.Resources["BrushGreen"];
        }
    }
}
