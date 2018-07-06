/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     Measure(), Arrange(), InvalidateMeasure(), InvalidateArrange() - 参见 /MyControls/MyControl2.cs
 *     DesiredSize - 获取通过 Measure() 计算后得到的期望尺寸
 *     RenderSize - 获取通过 Arrange() 计算后得到的呈现尺寸
 *     UpdateLayout() - 相当于依次调用 InvalidateMeasure() 和 InvalidateArrange()，然后同步等待结果，而 InvalidateMeasure() 和 InvalidateArrange() 本身是异步处理的
 *    
 * 
 * 注：
 * 1、uwp 的 layout 是一个递归系统，更多说明请参见 /MyControls/MyControl2.cs
 * 2、InvalidateMeasure() 就是递归调用自己和子辈门的 Measure()
 * 3、InvalidateArrange() 就是递归调用自己和子辈门的 Arrange()
 *    
 * 
 * 本例用于演示 UIElement 的布局相关
 */

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class LayoutDemo : Page
    {
        public LayoutDemo()
        {
            this.InitializeComponent();

            this.Loaded += LayoutDemo_Loaded;
        }

        private void LayoutDemo_Loaded(object sender, RoutedEventArgs e)
        {
            lblMsg.Text += stackPanel.DesiredSize.ToString(); // 204,106（期望尺寸，是包括 margin 的）
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += stackPanel.RenderSize.ToString(); // 200,100（呈现尺寸，是不包括 margin 的）
            lblMsg.Text += Environment.NewLine;

            // 更改外观
            stackPanel.Margin = new Thickness(5);
            rectangle1.Height = 300;

            // 更改外观后，布局系统会自动调用 InvalidateMeasure() 和 InvalidateArrange()，但是这是个异步的过程
            // 所以此处获取到的 DesiredSize 和 RenderSize 仍然是更改外观之前的值
            lblMsg.Text += stackPanel.DesiredSize.ToString(); // 204,106（期望尺寸，是包括 margin 的）
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += stackPanel.RenderSize.ToString(); // 200,100（呈现尺寸，是不包括 margin 的）
            lblMsg.Text += Environment.NewLine;

            // 如果想要同步知道结果的话就调用 UpdateLayout()
            stackPanel.UpdateLayout();

            // 所以此处获取到的 DesiredSize 和 RenderSize 为更改外观之后的值
            lblMsg.Text += stackPanel.DesiredSize.ToString(); // 210,310（期望尺寸，是包括 margin 的）
            lblMsg.Text += Environment.NewLine;
            lblMsg.Text += stackPanel.RenderSize.ToString(); // 200,300（呈现尺寸，是不包括 margin 的）
        }
    }
}
