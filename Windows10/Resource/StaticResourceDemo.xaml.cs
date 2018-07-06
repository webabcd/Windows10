/*
 * 演示“StaticResource”相关知识点
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Resource
{
    public sealed partial class StaticResourceDemo : Page
    {
        public StaticResourceDemo()
        {
            this.InitializeComponent();
        }

        private void btnChangeStaticResource_Click(object sender, RoutedEventArgs e)
        {
            // 获取 Application 中的资源
            // (double)Application.Current.Resources["MyDouble1"];

            // 获取关联 xaml 内的资源（本例中的资源定义在 xaml 中的 Page 下，所以用 this.Resources[""] 来获取）
            if (textBlock3.FontSize == (double)this.Resources["MyDouble1"])
            {
                // 引用指定的资源
                textBlock3.FontSize = (double)this.Resources["MyDouble2"];
            }
            else
            {
                textBlock3.FontSize = (double)this.Resources["MyDouble1"];
            }
        }
    }
}
