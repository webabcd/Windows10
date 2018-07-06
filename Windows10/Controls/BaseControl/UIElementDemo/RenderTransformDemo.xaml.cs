/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     RenderTransform - 2D 变换（位移，旋转，缩放，扭曲等）
 *     RenderTransformOrigin - 2D 变换的原点（这是一个相对值，元素左上角为 0,0，元素右下角为 1,1，支持小于 0 或大于 1 的值）
 *     
 *     
 * 本例用于演示 UIElement 的 2D 变换的应用
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class RenderTransformDemo : Page
    {
        public RenderTransformDemo()
        {
            this.InitializeComponent();
        }
    }
}
