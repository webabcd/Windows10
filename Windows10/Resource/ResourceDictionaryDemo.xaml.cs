/*
 * 演示“ResourceDictionary”相关知识点
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Resource
{
    public sealed partial class ResourceDictionaryDemo : Page
    {
        public ResourceDictionaryDemo()
        {
            this.InitializeComponent();

            this.Loaded += ResourceDictionaryDemo_Loaded;
        }

        private void ResourceDictionaryDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // 实例化一个 ResourceDictionary
            ResourceDictionary rd = new ResourceDictionary
            {
                Source = new Uri("ms-appx:///Resource/ResourceDictionary3.xaml", UriKind.Absolute)
            };

            // 将指定的 ResourceDictionary 集成到 Page.Resources 内的资源字典中
            this.Resources.MergedDictionaries.Add(rd);

            // 使用上面集成进来的资源字典中的资源
            textBlock4.Foreground = (SolidColorBrush)this.Resources["BrushBlue"];

            /*
             * 上面的例子演示的是如何处理指定的 FrameworkElement 中的资源
             * 如果需要处理 application 级的资源的话，可以通过 Application.Current.Resources 来获取 application 级的资源（对应的 xaml 为 App.xaml）
             */
        }

        private void btnGetResourceValue_Click(object sender, RoutedEventArgs e)
        {
            // 获取 application 级的指定资源的值
            lblMsg.Text = "SystemAccentColor: " + Application.Current.Resources["SystemAccentColor"].ToString();
            lblMsg.Text += Environment.NewLine;

            // 获取指定 ResourceDictionary 中的指定资源的值
            lblMsg.Text += "Page.Resources 中的 ColorRed 的值: " + this.Resources["ColorRed"].ToString();
            lblMsg.Text += Environment.NewLine;

            // 获取指定 ResourceDictionary 中的指定资源的值
            ResourceDictionary resourceDictionary1 = this.Resources.MergedDictionaries[0];
            lblMsg.Text += "Page.Resources.MergedDictionaries[0] 中的 ColorRed 的值: " + resourceDictionary1["ColorRed"].ToString();
        }
    }
}
