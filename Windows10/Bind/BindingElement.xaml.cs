/*
 * 演示如何与 Element 绑定
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Bind
{
    public sealed partial class BindingElement : Page
    {
        public BindingElement()
        {
            this.InitializeComponent();

            SetBindingDemo1();
            SetBindingDemo2();
        }

        // 在 C# 端做绑定（方式一）
        private void SetBindingDemo1()
        {
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                ElementName = nameof(textBox2),
                Path = new PropertyPath(nameof(TextBox.Text)),
                Mode = BindingMode.TwoWay // 默认是 OneWay 的
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBox1, TextBox.TextProperty, binding);
        }

        // 在 C# 端做绑定（方式二）
        private void SetBindingDemo2()
        {
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                ElementName = nameof(textBox4),
                Path = new PropertyPath(nameof(TextBox.Text)),
                Mode = BindingMode.TwoWay // 默认是 OneWay 的
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            textBox3.SetBinding(TextBox.TextProperty, binding);
        }
    }
}
