/*
 * 演示 Binding 中的 TargetNullValue 和 FallbackValue 的用法
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Bind
{
    public sealed partial class TargetNullValueFallbackValue : Page
    {
        public TargetNullValueFallbackValue()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 为 textBlock2 提供数据上下文
            textBlock2.DataContext = this;

            /*
            // 实例化 Binding 对象
            Binding binding = new Binding()
            {
                Path = new PropertyPath("Name"),
                TargetNullValue = "TargetNullValue",
                FallbackValue = "FallbackValue"
            };

            // 将目标对象的目标属性与指定的 Binding 对象关联
            BindingOperations.SetBinding(textBlock2, TextBox.TextProperty, binding);
            */
        }

        public string MyName { get; set; } = null;
    }
}
