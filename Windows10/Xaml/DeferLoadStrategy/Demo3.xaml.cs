/*
 * 演示 x:DeferLoadStrategy 的相关知识点
 *
 * 本例演示通过“Storyboard”加载延迟加载元素
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Xaml.DeferLoadStrategy
{
    public sealed partial class Demo3 : Page
    {
        public Demo3()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 启动一个引用了延迟加载元素的动画后，该延迟加载元素就会被加载
            sb.Begin();
        }
    }
}
