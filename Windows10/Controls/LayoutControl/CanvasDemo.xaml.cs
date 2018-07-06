/*
 * Canvas - 绝对定位布局控件（继承自 Panel, 请参见 /Controls/LayoutControl/PanelDemo.xaml）
 *     double GetLeft(UIElement element) - 获取指定 UIElement 的 Canvas.Left 值
 *     double GetTop(UIElement element) - 获取指定 UIElement 的 Canvas.Top 值
 *     int GetZIndex(UIElement element) - 获取指定 UIElement 的 Canvas.ZIndex 值
 *     SetLeft(UIElement element, double length) - 设置指定 UIElement 的 Canvas.Left 值
 *     SetTop(UIElement element, double length) - 设置指定 UIElement 的 Canvas.Top 值
 *     SetZIndex(UIElement element, int value) - 设置指定 UIElement 的 Canvas.ZIndex 值
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.LayoutControl
{
    public sealed partial class CanvasDemo : Page
    {
        public CanvasDemo()
        {
            this.InitializeComponent();
        }
    }
}
