/*
 * FrameworkElement - FrameworkElement（继承自 UIElement, 请参见 /Controls/BaseControl/UIElementDemo/）
 *     MinWidth, MinHeight - 最小宽高，默认值为 0
 *     MaxWidth, MaxHeight - 最大宽高，默认值为 double.PositiveInfinity
 *     Width, Height - 宽高，默认值为 NaN
 *     ActualWidth, ActualHeight - 实际宽高，来自 UIElement 的 RenderSize 属性（uwp 的 layout 是一个递归系统，更多说明请参见 /MyControls/MyControl2.cs）
 *     Margin - Margin
 *         它是一个 Thickness 类型的对象，在 C# 端就构造这个对象即可
 *         在 xaml 端设置的话，其规则为“左,上,右,下”或“左右,上下”或“左上右下”，可以用逗号分隔也可以用空格分隔
 *     Name - 名字
 *     FindName() - 查找当前页面的指定名字的对象
 *     Parent - 获取当前对象的父对象
 *     Tag - 用于保存任意对象
 *     Language - 设置或获取当前元素及其所有子元素的语言信息（没什么实际效果，就是一个标记而已）
 *     BaseUri - 获取当前对象所在的 xaml 页面的 uri 地址
 *     DataContext - 数据上下文（参见 /Bind/DataContextChanged.xaml）
 *     RequestedTheme - 主题（参见 /Resource/ThemeResourceDemo.xaml.cs）
 *     Resources - 资源字典（参见 /Resource/ResourceDictionaryDemo.xaml）
 *     Style - 样式（参见 /Controls/UI/Style.xaml）
 *     GetBindingExpression() - 获取指定属性的绑定信息（参见 /Bind/UpdateSourceTrigger.xaml.cs）
 *     SetBinding() - 设置绑定信息（参见 /Bind/BindingElement.xaml.cs）
 *         
 *     
 * 本例用于演示 FrameworkElement 的基础知识
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.FrameworkElementDemo
{
    public sealed partial class Demo1 : Page
    {
        public Demo1()
        {
            this.InitializeComponent();

            this.Loaded += Demo1_Loaded;
        }

        private void Demo1_Loaded(object sender, RoutedEventArgs e)
        {
            stackPanel.MinWidth = 0;
            stackPanel.MinHeight = 0;
            stackPanel.MaxWidth = double.PositiveInfinity;
            stackPanel.MaxHeight = double.PositiveInfinity;

            lblMsg.Text += $"stackPanel.ActualWidth:{stackPanel.ActualWidth}, stackPanel.ActualHeight:{stackPanel.ActualHeight}";
            lblMsg.Text += Environment.NewLine;

            if (this.FindName("lblMsg") as TextBlock == lblMsg)
            {
                lblMsg.Text += "this.FindName(\"lblMsg\") as TextBlock == lblMsg";
                lblMsg.Text += Environment.NewLine;
            }

            if (lblMsg.Parent as StackPanel == stackPanel)
            {
                lblMsg.Text += "lblMsg.Parent as StackPanel == stackPanel";
                lblMsg.Text += Environment.NewLine;
            }

            lblMsg.Text += "BaseUri:" + this.BaseUri;
            lblMsg.Text += Environment.NewLine;

            lblMsg.Tag = "i am webabcd";
            lblMsg.Text += "lblMsg.Tag:" + lblMsg.Tag;
            lblMsg.Text += Environment.NewLine;
        }
    }
}
