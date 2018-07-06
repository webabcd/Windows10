/*
 * 演示 x:DeferLoadStrategy 的相关知识点
 *
 * 本例演示通过“绑定”加载延迟加载元素
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Xaml.DeferLoadStrategy
{
    public sealed partial class Demo2 : Page
    {
        public Demo2()
        {
            this.InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                ElementName = nameof(textBox2), // textBox2 是延迟加载元素，将其与 textBox1 绑定后，textBox2 就会被加载
                Path = new PropertyPath(nameof(TextBox.Text)),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBox1, TextBox.TextProperty, binding);
        }
    }
}
