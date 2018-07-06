/*
 * 演示如何关联指定的协议（即用本程序处理指定的协议）
 * 
 * 1、在 Package.appxmanifest 中新增一个“协议”声明，并做相关配置
 *    本例中的这部分的配置如下，协议名必须全小写
 *    <uap:Extension Category="windows.protocol">
 *      <uap:Protocol Name="webabcd">
 *        <uap:Logo>Assets\StoreLogo.png</uap:Logo>
 *      </uap:Protocol>
 *    </uap:Extension>
 * 2、在 App.xaml.cs 中 override void OnActivated(IActivatedEventArgs args)，以获取相关的协议信息
 * 
 * 
 * ProtocolActivatedEventArgs - 通过协议激活应用程序时的事件参数
 *     Uri - 协议的 uri
 *     CallerPackageFamilyName - 激活当前 app 的 app 的 PackageFamilyName
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         本例为 ActivationKind.Protocol
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     User - 获取激活了此 app 的 User 对象
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Windows10.AssociationLaunching
{
    public sealed partial class ProtocolAssociation : Page
    {
        private ProtocolActivatedEventArgs _protocolArgs;

        public ProtocolAssociation()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 ProtocolActivatedEventArgs 对象（从 App.xaml.cs 传来的）
            _protocolArgs = e.Parameter as ProtocolActivatedEventArgs;

            // 显示协议的详细信息
            if (_protocolArgs != null)
            {
                grid.Background = new SolidColorBrush(Colors.Blue);
                lblMsg.Foreground = new SolidColorBrush(Colors.White);

                lblMsg.Text = "激活程序的自定义协议为: " + _protocolArgs.Uri.AbsoluteUri;
            }
        }

        private async void btnProtocol_Click(object sender, RoutedEventArgs e)
        {
            // 打开自定义协议 webabcd:data
            Uri uri = new Uri("webabcd:data");
            await Launcher.LaunchUriAsync(uri);

            // 打开 IE 浏览器，在地址栏输入 webabcd:data，即会打开本程序来处理此协议
        }
    }
}
