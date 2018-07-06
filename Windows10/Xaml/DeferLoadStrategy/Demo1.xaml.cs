/*
 * 演示 x:DeferLoadStrategy 的相关知识点
 *
 * 本例演示通过“FindName”加载延迟加载元素
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Xaml.DeferLoadStrategy
{
    public sealed partial class Demo1 : Page
    {
        public Demo1()
        {
            this.InitializeComponent();

            this.Loaded += DeferLoadStrategyDemo_Loaded;
        }

        private void DeferLoadStrategyDemo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 抛出异常
                textBlock.Text = "我是 TextBlock";
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
                lblMsg.Text += Environment.NewLine;
            }

            // 可以通过 FindName() 来加载 x:DeferLoadStrategy="Lazy" 元素
            this.FindName(nameof(textBlock));

            textBlock.Text = "我是 TextBlock";
        }
    }
}
