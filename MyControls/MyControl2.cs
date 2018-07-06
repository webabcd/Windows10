/*
 * 本例通过一个自定义控件来演示 uwp 中可视元素的 Layout 系统
 * 
 * uwp 的 layout 是一个递归系统，本 demo 就递归的一个过程做说明（步骤顺序参见代码注释中的序号）
 * 
 * 
 * Measure() 的作用是测量尺寸
 * Arrange() 的作用是排列元素
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace MyControls
{
    /// <summary>
    /// 一个每行都会自动缩进的 Panel
    /// </summary>
    public sealed class MyControl2 : Panel
    {
        // 相对上一行的缩进值
        const double INDENT = 20;

        public MyControl2()
        {

        }

        // 1、首先爸爸知道自己能够提供的尺寸 availableSize，然后告诉儿子们
        protected override Size MeasureOverride(Size availableSize) // 测量出期待的尺寸并返回
        {
            // 2、儿子们收到 availableSize 后，又结合了自身的实际情况，然后告诉爸爸儿子们所期望的尺寸 desiredSize
            List<double> widthList = new List<double>();
            Size desiredSize = new Size(0, 0);
            foreach (UIElement child in this.Children)
            {
                // 如果 child 是 FrameworkElement 的话，则当调用其 Measure() 方法时会自动调用其 MeasureOverride() 方法
                child.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                widthList.Add(child.DesiredSize.Width);
                desiredSize.Height += child.DesiredSize.Height;
            }

            if (this.Children.Count > 0)
            {
                desiredSize.Width = widthList.Max();
                desiredSize.Width += INDENT * (this.Children.Count - 1);
            }

            Debug.WriteLine("availableSize: " + availableSize.ToString());
            Debug.WriteLine("desiredSize: " + desiredSize.ToString());

            return desiredSize;
        }

        // 3、爸爸收到儿子们的反馈后，告诉儿子们自己最终提供的尺寸 finalSize
        protected override Size ArrangeOverride(Size finalSize) // 排列元素，并返回呈现尺寸
        {
            // 4、儿子们根据 finalSize 安排各自的位置，然后爸爸的呈现尺寸也就确定了 renderSize
            Point childPosition = new Point(0, 0);
            foreach (UIElement child in this.Children)
            {
                // 如果 child 是 FrameworkElement 的话，则当调用其 Arrange() 方法时会自动调用其 ArrangeOverride() 方法
                child.Arrange(new Rect(childPosition, new Size(child.DesiredSize.Width, child.DesiredSize.Height)));
                childPosition.X += INDENT;
                childPosition.Y += child.DesiredSize.Height;
            }

            Size renderSize = new Size(0, 0);
            renderSize.Width = finalSize.Width;
            renderSize.Height = childPosition.Y;

            Debug.WriteLine("finalSize: " + finalSize.ToString());
            Debug.WriteLine("renderSize: " + renderSize.ToString());

            return finalSize;
        }
    }
}


/*
 * 输出结果如下（运行 /Controls/CustomControl/Demo2.xaml 示例）
 * availableSize: 800,Double.PositiveInfinity
 * desiredSize: 141,120
 * finalSize: 800,120
 * renderSize: 800,120
*/


/*
 * 注：
 * UIElement
 *     调用 Measure() 方法后会更新 DesiredSize 属性
 *     调用 Arrange() 方法后会更新 RenderSize 属性
 *     UpdateLayout() - 强制 layout 递归更新
 * 
 * FrameworkElement - 继承自 UIElement
 *     MeasureOverride() - 在 Measure() 中自动调用
 *     ArrangeOverride() - 在 Arrange() 中自动调用
 *     ActualWidth 和 ActualHeight 来自 RenderSize，每次 UpdateLayout() 后都会被更新
 */


/*
* 注：
* 1、uwp 的 layout 是一个递归系统
* 2、UIElement 的 InvalidateMeasure() 就是递归调用自己和子辈门的 Measure()
* 3、UIElement 的 InvalidateArrange() 就是递归调用自己和子辈门的 Arrange()
* 
* 一个通过 uwp 自带控件说明 layout 的示例，请参见：/Controls/BaseControl/UIElementDemo/LayoutDemo.xaml.cs
*/
