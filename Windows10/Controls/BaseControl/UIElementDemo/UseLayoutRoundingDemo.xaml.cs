/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     UseLayoutRounding - 是否使用完整像素布局。默认值为 true
 *     
 *     
 * 本例用于演示 UIElement 的 UseLayoutRounding 的应用
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class UseLayoutRoundingDemo : Page
    {
        public UseLayoutRoundingDemo()
        {
            this.InitializeComponent();
        }
    }
}
