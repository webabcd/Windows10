/*
 * 演示“ThemeResource”相关知识点
 *
 *
 * 1、主题共有两种类别：Light 和 Dark，子会继承父的主题类别
 * 2、Application 级别指定 Theme 的话，在 App.xaml 中做如下声明 <Application RequestedTheme="Dark"></Application>
 * 3、FrameworkElement 级别指定 Theme 的话，则指定 FrameworkElement.RequestedTheme 即可
 *
 *
 * Application.Current.RequestedTheme - 获取或设置 Application 级别的主题（ApplicationTheme 枚举：Light, Dark）
 * FrameworkElement.RequestedTheme - 获取或设置 FrameworkElement 级别的主题（ElementTheme 枚举：Default, Light, Dark）
 * 注：ElementTheme 比 ApplicationTheme 多了一个 Default，其含义是当 ElementTheme 为 Default 时，其实际主题为 application 级主题
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Resource
{
    public sealed partial class ThemeResourceDemo : Page
    {
        public ThemeResourceDemo()
        {
            this.InitializeComponent();

            DisplayMessage();
        }

        private void DisplayMessage()
        {
            // 当前 Application 级别的 Theme
            lblMsg.Text = "application theme: " + Application.Current.RequestedTheme.ToString();
            lblMsg.Text += Environment.NewLine;

            // 当前 panel 的 Theme
            lblMsg.Text += "FrameworkElement  theme: " + panel.RequestedTheme.ToString();
        }

        // 动态变换主题，引用的主题资源会重新计算
        private void btnChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (panel.RequestedTheme == ElementTheme.Default)  // 未指定 panel 的主题，则 panel 主题同 application 级主题
            {
                if (Application.Current.RequestedTheme == ApplicationTheme.Dark) // application 是 Dark 主题
                {
                    panel.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    panel.RequestedTheme = ElementTheme.Dark;
                }
            }
            else if (panel.RequestedTheme == ElementTheme.Dark) // panel 是 Dark 主题
            {
                panel.RequestedTheme = ElementTheme.Light;
            }
            else // panel 是 Light 主题
            {
                panel.RequestedTheme = ElementTheme.Dark;
            }

            DisplayMessage();
        }
    }
}
