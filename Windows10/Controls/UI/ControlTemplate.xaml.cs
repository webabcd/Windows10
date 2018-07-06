/*
 * 演示“ControlTemplate”相关知识点
 *
 * 注：
 * 1、控件模板是 xaml 语言使用的一种方案，其无法在 c# 中定义
 * 2、Template 属性来自 Windows.UI.Xaml.Controls.Control
 */

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Windows10.Controls.UI
{
    public sealed partial class ControlTemplate : Page
    {
        public ControlTemplate()
        {
            this.InitializeComponent();
        }

        private void btnChangeControlTemplate_Click(object sender, RoutedEventArgs e)
        {
            // 获取 Application 中的资源
            // (Windows.UI.Xaml.Style)Application.Current.Resources["MyControlTemplate"];

            // 获取关联 xaml 内的资源
            if (button1.Template == (Windows.UI.Xaml.Controls.ControlTemplate)grid.Resources["ButtonControlTemplate1"])
            {
                // 指定控件模板
                button1.Template = (Windows.UI.Xaml.Controls.ControlTemplate)grid.Resources["ButtonControlTemplate2"];
            }
            else
            {
                button1.Template = (Windows.UI.Xaml.Controls.ControlTemplate)grid.Resources["ButtonControlTemplate1"];
            }
        }
    }
}
