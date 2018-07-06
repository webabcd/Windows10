/*
 * 本例用于演示自定义控件的基础知识，依赖属性和附加属性
 */

using MyControls;
using Windows.UI.Xaml.Controls;

namespace Windows10.Controls.CustomControl
{
    public sealed partial class Demo1 : Page
    {
        public Demo1()
        {
            this.InitializeComponent();

            this.Loaded += Demo1_Loaded;
        }
        
        private void Demo1_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // 设置依赖属性
            control3.Title = "我是依赖属性";

            // 设置附加属性
            control3.SetValue(MyAttachedProperty.SubTitleProperty, "我是附加属性");
        }
    }
}
