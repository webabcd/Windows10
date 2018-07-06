/*
 * 演示如何与 StaticResource 绑定（关于 StaticResource 的说明请参见：/Resource/StaticResourceDemo.xaml）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Bind
{
    public sealed partial class BindingStaticResource : Page
    {
        public BindingStaticResource()
        {
            this.InitializeComponent();

            this.Loaded += BindingStaticResource_Loaded;
        }

        // 在 C# 端绑定 StaticResource
        private void BindingStaticResource_Loaded(object sender, RoutedEventArgs e)
        {
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                Source = panel.Resources["MyStyle"]
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBox, TextBox.StyleProperty, binding);
        }
    }
}
