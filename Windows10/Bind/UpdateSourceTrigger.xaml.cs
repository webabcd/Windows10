/*
 * UpdateSourceTrigger - 数据更新的触发方式
 *     Default - 失去焦点后触发
 *     PropertyChanged - 属性值发生改变后触发
 *     Explicit - 需要通过 BindingExpression.UpdateSource() 显示触发
 *     
 *     
 * BindingExpression - 绑定信息，可以通过 FrameworkElement 的 GetBindingExpression() 方法获取指定属性的绑定信息
 *     DataItem - 获取绑定的源对象
 *     ParentBinding - 获取绑定的 Binding 对象（Binding 对象里包括 ElementName, Path, Mode 等绑定信息）
 *     UpdateSource() - 将当前值发送到 TwoWay 绑定的源对象的绑定的属性中
 */

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Windows10.Bind
{
    public sealed partial class UpdateSourceTrigger : Page
    {
        public UpdateSourceTrigger()
        {
            this.InitializeComponent();
        }

        private async void btnBinding_Click(object sender, RoutedEventArgs e)
        {
            // 显示触发 txtExplicit 的数据更新
            BindingExpression be = txtExplicit.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();

            // 获取绑定的相关信息
            Binding binding = be.ParentBinding;
            TextBlock textBlock = be.DataItem as TextBlock;
            MessageDialog messageDialog = new MessageDialog($"BindingExpression.DataItem:{textBlock.Name}, Binding.Mode:{binding.Mode}");
            await messageDialog.ShowAsync();
        }
    }
}
