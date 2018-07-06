/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     Clip - 剪裁 UIElement 的指定区域（目前只支持通过 RectangleGeometry 剪裁 UIElement）
 *     
 *     
 * 本例用于演示 UIElement 的 Clip 的应用
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class ClipDemo : Page
    {
        public ClipDemo()
        {
            this.InitializeComponent();
        }
    }
}
