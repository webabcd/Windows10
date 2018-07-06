/*
 * 演示“CustomResource”相关知识点
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Resources;

namespace Windows10.Resource
{
    public sealed partial class CustomResourceDemo : Page
    {
        public CustomResourceDemo()
        {
            // 这是必须的，需要先要指定当前使用的自定义 CustomXamlResourceLoader 实例
            CustomXamlResourceLoader.Current = new CustomResourceTest();

            this.InitializeComponent();
        }
    }
}
