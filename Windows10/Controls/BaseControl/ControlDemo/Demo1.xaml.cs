/*
 * Control - Control（继承自 UIElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     Background - 背景 Brush
 *     Foreground - 前景 Brush
 *     BorderBrush - 边框 Brush
 *     BorderThickness - 边框 Thickness
 *     Padding - Padding
 *     FontSize - 字号大小（单位：像素）
 *     FontFamily - 首选字体，多个用“,”分隔，找不到第 1 个就用第 2 个，找不到第 2 个就用第 3 个，以此类推
 *     FontStretch - 字体的拉伸值（FontStretch 枚举），默认值是 Normal（大部分字体都不支持这个属性）
 *     FontStyle - 字体样式（FontStyle 枚举）
 *         Normal - 默认值
 *         Italic - 使用字体自带的斜体
 *         Oblique - 通过程序让正常字体倾斜（对于自身不带斜体的字体可以使用此值让字体倾斜）
 *     FontWeight - 字体粗细（FontWeights 实体类），默认值是 Normal
 *     CharacterSpacing - 用于设置字符间距
 *         具体字符间隔像素计算公式如下：字体大小 * CharacterSpacing值 / 1000 = 字符间距像素值
 *     IsTextScaleFactorEnabled - 是否启用文本自动放大功能（默认值是 true）
 *         在“设置”->“轻松使用”中可以调整文本缩放大小，IsTextScaleFactorEnabled 就是用于决定 TextBlock 显示的文本是否跟着这个设置走
 *     HorizontalContentAlignment - 内容的水平对齐方式
 *     VerticalContentAlignment - 内容的垂直对齐方式
 *     IsEnabled - 是否可用
 *     IsEnabledChanged - IsEnabled 属性的值发生变化时触发的事件
 *     Template - 控件模板，参见 /Controls/UI/ControlTemplate.xaml
 *     
 *     
 * 本例用于演示 Control 的基础知识
 */

using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Text;

namespace Windows10.Controls.BaseControl.ControlDemo
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
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/hololens.jpg", UriKind.Absolute));
            imageBrush.Stretch = Stretch.Fill;

            textBox.Background = imageBrush; // 关于各种类型的 Brush 请参见 /Drawing/Brush.xaml
            textBox.Foreground = new SolidColorBrush(Colors.DarkGreen);
            textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            textBox.BorderThickness = new Thickness(0, 0, 0, 100); // 边框的占用空间是完全是在 Control 内部的
            textBox.Padding = new Thickness(20);

            textBox.FontSize = 24;
            textBox.FontFamily = new FontFamily("微软雅黑,宋体");
            textBox.FontStretch = FontStretch.Normal;
            textBox.FontStyle = FontStyle.Normal;
            textBox.FontWeight = FontWeights.Normal;
            textBox.CharacterSpacing = 100;
            textBox.IsTextScaleFactorEnabled = true;

            // 对于 TextBox 来说 HorizontalContentAlignment 和 VerticalContentAlignment 都是无效的
            // 但是对于 Button 来说则可以通过 HorizontalContentAlignment 和 VerticalContentAlignment 来指定按钮上文字的对齐方式
            textBox.HorizontalContentAlignment = HorizontalAlignment.Center;// 无效，如果需要设置文字内容的水平对齐方式的话请使用 textBox.TextAlignment
            textBox.VerticalContentAlignment = VerticalAlignment.Center; // 无效

            textBox.IsEnabledChanged += TextBox_IsEnabledChanged;
            textBox.IsEnabled = false; // 注：如果要修 IsEnabled = false 的样式请查看名为 Disabled 的 VisualState
            textBox.IsEnabled = true;
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            textBox.Text += Environment.NewLine;
            textBox.Text += $"textBox.IsEnabled, OldValue:{e.OldValue}, NewValue:{e.NewValue}"; 
        }
    }
}
