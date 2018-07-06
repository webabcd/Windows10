/*
 * Control - Control（继承自 UIElement, 请参见 /Controls/BaseControl/FrameworkElementDemo/）
 *     GetTemplateChild() - 查找控件模板中的指定名字的元素
 *     OnApplyTemplate() - 应用控件模板时调用（来自 FrameworkElement）
 *     
 * VisualTreeHelper - 可视化树的实用工具类
 *     
 *     
 * 本例用于演示如何在运行时获取 ControlTemplate 和 DataTemplate 中的元素
 */

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows10.Common;

namespace Windows10.Controls.BaseControl.ControlDemo
{
    public sealed partial class Demo3 : Page
    {
        public Employee CurrentEmployee { get; set; } = new Employee() { Name = "wanglei", Age = 36, IsMale = true };

        public Demo3()
        {
            this.InitializeComponent();

            myContentControl.Loaded += MyContentControl_Loaded;
        }

        private void MyContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 通过 VisualTreeHelper 获取可视化树结构（借此可以获取数据模板中的元素）
            TextBlock textBlock = Helper.GetVisualChild<TextBlock>(myContentControl, "textBlock");
            textBlock.FontSize = 48;
        }
    }

    
    public class MyContentControl : ContentControl
    {
        // override OnApplyTemplate() - 应用控件模板时调用
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // 在 OnApplyTemplate() 中通过 GetTemplateChild() 获取控件模板中的指定名字的元素
            Grid grid = (Grid)GetTemplateChild("grid");
            grid.Background = new SolidColorBrush(Colors.Orange);         
        }
    }
}
