/*
 * UIElement - UIElement（继承自 DependencyObject, 请参见 /Controls/BaseControl/DependencyObjectDemo/）
 *     Visibility - 可见性
 *         Visible - 显示
 *         Collapsed - 不显示，且不占位
 *     Opacity - 不透明度（0.0 - 1.0 之间，默认值为 1.0）
 *     CacheMode - 缓存模式
 *         null - 默认值
 *         BitmapCache - 用 GPU 渲染 RenderTransform 和 Opacity
 *             如果 RenderTransform 或 Opacity 使用了 storyboard 动画的话，动画一律将变为 Dependent Animation 而不是 Independent Animation，这样性能就会变差。一般来说不用设置成 BitmapCache 模式
 *     
 *     
 * 本例用于演示 UIElement 的其他特性
 */

using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.BaseControl.UIElementDemo
{
    public sealed partial class OthersDemo : Page
    {
        public OthersDemo()
        {
            this.InitializeComponent();

            // this.Visibility = Visibility.Collapsed;
            // this.Opacity = 0.5;
            // this.CacheMode = null;
            // this.CacheMode = new BitmapCache();
        }
    }
}
