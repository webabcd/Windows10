/*
 * DependencyObject - 依赖对象（可以在 DependencyObject 上定义 DependencyProperty）
 *     RegisterPropertyChangedCallback() - 为指定的依赖属性注册一个变化回调
 *     UnregisterPropertyChangedCallback() - 取消 RegisterPropertyChangedCallback() 的注册
 *     
 * 
 * 本例用于演示如何监听 DependencyObject 的依赖属性的变化
 */

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace Windows10.Controls.BaseControl.DependencyObjectDemo
{
    public sealed partial class RegisterPropertyChangedCallbackDemo : Page
    {
        private long _token = -1;

        public RegisterPropertyChangedCallbackDemo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 为 WidthProperty 注册一个变化回调（返回值是一个 token 用于取消注册用）
            _token = rect1.RegisterPropertyChangedCallback(Rectangle.WidthProperty, WidthChanged);
            
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 为 WidthProperty 取消指定的变化回调（通过 token 来指定取消的是哪个变化回调）
            rect1.UnregisterPropertyChangedCallback(Rectangle.WidthProperty, _token);

            base.OnNavigatedFrom(e);
        }
        
        private void WidthChanged(DependencyObject sender, DependencyProperty prop)
        {
            double width = (double)sender.GetValue(prop);

            lblMsg.Text = $"width: {width}";
        }
    }
}
