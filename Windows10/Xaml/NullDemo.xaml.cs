/*
 * 演示 x:Null 的相关知识点、
 *
 * 在这里插一句：
 * 在 xaml 使用的 {x:Null}, {Binding}, {x:Bind}, {StaticResource} 之类的这种带大括号的语法被称为标记扩展（Markup Extension），在 uwp 中无法开发自定义标记扩展（但是在 wpf 中是可以的）
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Xaml
{
    public sealed partial class NullDemo : Page
    {
        public NullDemo()
        {
            this.InitializeComponent();

            this.Loaded += NullDemo_Loaded;
        }

        private void NullDemo_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text = $"textBlock1.Tag: {textBlock1.Tag ?? "null"}"; // null
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += $"textBlock2.Tag: {textBlock2.Tag ?? "null"}"; // ""
            lblMsg.Text += Environment.NewLine;

            lblMsg.Text += $"textBlock3.Tag: {textBlock3.Tag ?? "null"}"; // null
        }
    }
}
