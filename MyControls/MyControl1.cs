/*
 * 开发一个自定义控件，并定义一个依赖属性（Dependency Property）
 * 
 * 依赖属性：可以用于样式, 模板, 绑定, 动画
 * 附加属性：全局可用的依赖属性
 */

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace MyControls
{
    /// <summary>
    /// 开发一个自定义控件，并定义一个依赖属性（Dependency Property）
    /// </summary>
    // 注意：
    // 在 winrc 中用 c# 写的类必须是 sealed 的（否则编译时会报错 Exporting unsealed types is not supported.Please mark type 'MyControls.MyControl1' as sealed）
    // 如果是 dll 项目则无此限制
    public sealed class MyControl1 : Control 
    {
        public MyControl1()
        {
            // 指定默认样式为 typeof(MyControl1)，即使用 TargetType 为 MyControl1 的样式，即 <Style xmlns:local="using:MyControls" TargetType="local:MyControl1" />
            // 如果不指 DefaultStyleKey 的话，则默认使用基类即 Control 的样式
            this.DefaultStyleKey = typeof(MyControl1);
        }

        // 通过 DependencyObject.GetValue() 和 DependencyObject.SetValue() 访问依赖属性，这里由 Title 属性封装一下，以方便对依赖属性的访问
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // 注册一个依赖属性
        // 注意：
        // 在 winrc 中不支持 public 类型的 field（在 dll 项目无此限制），所以这里改为 private 的，之后再用 public 属性的方式封装一下即可
        // 如果使用了 public 类型的 field 的话，编译时会报错 Type 'MyControls.MyControl1' contains externally visible field 'Windows.UI.Xaml.DependencyProperty MyControls.MyControl1.TitlePropertyField'.  Fields can be exposed only by structures
        private static readonly DependencyProperty TitlePropertyField =
            DependencyProperty.Register(
                "Title", // 依赖属性的名称
                typeof(string),  // 依赖属性的数据类型
                typeof(MyControl1),  // 依赖属性所属的类
                new PropertyMetadata("", PropertyMetadataCallback)); // 指定依赖属性的默认值，以及值发生改变时所调用的方法

        // 用属性的方式封装一下 TitlePropertyField
        public static DependencyProperty TitleProperty
        {
            get
            {
                return TitlePropertyField;
            }
        }

        private static void PropertyMetadataCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            object newValue = args.NewValue; // 发生改变之后的值
            object oldValue = args.OldValue; // 发生改变之前的值
        }
    }
}