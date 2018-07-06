/*
 * 演示“Style”相关知识点
 *
 * 注：
 * 1、Style 属性来自 Windows.UI.Xaml.FrameworkElement
 */

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.UI
{
    public sealed partial class Style : Page
    {
        public Style()
        {
            this.InitializeComponent();

            this.Loaded += Style_Loaded;
        }

        // 在 c# 中定义样式
        private void Style_Loaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Style style = new Windows.UI.Xaml.Style();
            style.TargetType = typeof(TextBox);

            Setter setter1 = new Setter();
            setter1.Property = TextBox.BackgroundProperty;
            setter1.Value = Colors.Blue;

            style.Setters.Add(setter1);

            textBox4.Style = style;
        }

        // 改变样式
        private void btnChangeStyle_Click(object sender, RoutedEventArgs e)
        {
            // 获取 Application 中的资源
            // (Windows.UI.Xaml.Style)Application.Current.Resources["myStyle"];

            // 获取关联 xaml 内的资源
            if (textBox2.Style == (Windows.UI.Xaml.Style)grid.Resources["TextBoxStyleBig1"])
            {
                // 指定样式
                textBox2.Style = (Windows.UI.Xaml.Style)grid.Resources["TextBoxStyleBig2"];
            }
            else
            {
                textBox2.Style = (Windows.UI.Xaml.Style)grid.Resources["TextBoxStyleBig1"];
            }
        }
    }
}
