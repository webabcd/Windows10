/*
 * 用于演示使用绑定过程中的一些技巧
 *
 * 在这里插一句：
 * 在 xaml 使用的 {Binding}, {x:Bind}, {StaticResource} 之类的这种带大括号的语法被称为标记扩展（Markup Extension），在 uwp 中无法开发自定义标记扩展（但是在 wpf 中是可以的）
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows10.Common;

namespace Windows10.Bind
{
    public sealed partial class Tips : Page
    {
        public Tips()
        {
            this.InitializeComponent();

            this.Loaded += Tips_Loaded;
        }

        private void Tips_Loaded(object sender, RoutedEventArgs e)
        {
            BindingAttachedProperty();
        }

        // 在 CodeBehind 端绑定附加属性
        private void BindingAttachedProperty()
        {
            Binding binding = new Binding()
            {
                Path = new PropertyPath("(Grid.Row)"), // 注意要有括号，另外在 CodeBehind 端绑定自定义附加属性暂时没有成功
                Source = textBox3
            };
            BindingOperations.SetBinding(textBox3, TextBox.TextProperty, binding);
        }

        // 通过 x:Bind 绑定时，要做转换
        public object CurrentEmployee { get; set; } = new Employee() { Name = "wanglei", Age = 36, IsMale = true };

        public string MyName { get; set; } = "webabcd";
    }



    /// <summary>
    /// 用于附加属性的演示
    /// </summary>
    public class MyAttachedProperty
    {
        // 获取附加属性
        public static string GetSubTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(SubTitleProperty);
        }

        // 设置附加属性
        public static void SetSubTitle(DependencyObject obj, string value)
        {
            obj.SetValue(SubTitleProperty, value);
        }

        // 注册一个附加属性
        public static readonly DependencyProperty SubTitleProperty =
            DependencyProperty.RegisterAttached(
                "SubTitle", // 附加属性的名称
                typeof(string), // 附加属性的数据类型
                typeof(MyAttachedProperty), // 附加属性所属的类
                new PropertyMetadata("", PropertyMetadataCallback)); // 指定附加属性的默认值，以及值发生改变时所调用的方法

        private static void PropertyMetadataCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            object newValue = args.NewValue; // 发生改变之后的值
            object oldValue = args.OldValue; // 发生改变之前的值
        }
    }
}
