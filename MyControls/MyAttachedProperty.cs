/*
 * 定义一个附加属性（Attached Property）
 * 
 * 依赖属性：可以用于样式, 模板, 绑定, 动画
 * 附加属性：全局可用的依赖属性
 */
 
using Windows.UI.Xaml;

namespace MyControls
{
    /// <summary>
    /// 定义一个附加属性（Attached Property）
    /// </summary>
    public sealed class MyAttachedProperty
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

        // 注册一个附加属性（winrc 中不支持 public 类型的 field，如果是 dll 项目则无此限制）
        private static readonly DependencyProperty SubTitlePropertyField =
            DependencyProperty.RegisterAttached(
                "SubTitle", // 附加属性的名称
                typeof(string), // 附加属性的数据类型
                typeof(MyAttachedProperty), // 附加属性所属的类
                new PropertyMetadata("", PropertyMetadataCallback)); // 指定附加属性的默认值，以及值发生改变时所调用的方法

        // 用属性的方式封装一下 SubTitlePropertyField
        public static DependencyProperty SubTitleProperty
        {
            get
            {
                return SubTitlePropertyField;
            }
        }

        private static void PropertyMetadataCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            object newValue = args.NewValue; // 发生改变之后的值
            object oldValue = args.OldValue; // 发生改变之前的值
        }
    }
}