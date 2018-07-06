/*
 * 本例用于演示当通过 toast 激活 app 时（前台方式激活），如何获取相关信息
 * 
 * 
 * 在 App.xaml.cs 中 override void OnActivated(IActivatedEventArgs args)，以获取相关的 toast 信息
 * 
 * ToastNotificationActivatedEventArgs - 通过 toast 激活应用程序时（前台方式激活）的事件参数
 *     Argument - 由 toast 传递过来的参数
 *     UserInput - 由 toast 传递过来的输入框数据
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         比如，如果是通过“打开文件”激活的话，则此值为 File
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     User - 获取激活了此 app 的 User 对象
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.Notification.Toast
{
    public sealed partial class Demo : Page
    {
        private ToastNotificationActivatedEventArgs _toastArgs;

        public Demo()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 ToastNotificationActivatedEventArgs 对象（从 App.xaml.cs 传来的）
            _toastArgs = e.Parameter as ToastNotificationActivatedEventArgs;

            if (_toastArgs != null)
            {
                // 获取 toast 的参数
                lblMsg.Text = "argument: " + _toastArgs.Argument;
                lblMsg.Text += Environment.NewLine;

                // 获取 toast 的 输入框数据
                // UserInput 是一个 ValueSet 类型的数据，其继承自 IEnumerable 接口，可以 foreach（不能 for）
                foreach (string key in _toastArgs.UserInput.Keys)
                {
                    lblMsg.Text += $"key:{key}, value:{_toastArgs.UserInput[key]}";
                    lblMsg.Text += Environment.NewLine;
                }
            }
        }
    }
}
