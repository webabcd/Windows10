/*
 * 演示如何通过协议打开指定的 app 并传递数据以及获取返回数据
 * 
 * 1、在 Package.appxmanifest 中新增一个“协议”声明，并做相关配置
 *    本例中的这部分的配置如下，协议名必须全小写
 *    <uap:Extension Category="windows.protocol">
 *      <uap:Protocol Name="webabcd" ReturnResults="optional">
 *        <uap:Logo>Assets\StoreLogo.png</uap:Logo>
 *      </uap:Protocol>
 *    </uap:Extension>
 *    其中关于 ReturnResults 的说明如下：
 *        optional - 可以通过 LaunchUriAsync() 启动，也可以通过 LaunchUriForResultsAsync() 启动
 *        always - 只能通过 LaunchUriForResultsAsync() 启动
 *        none - 只能通过 LaunchUriAsync() 启动
 * 2、在 App.xaml.cs 中 override void OnActivated(IActivatedEventArgs args)，以获取相关的协议信息
 * 
 * 
 * ProtocolForResultsActivatedEventArgs - 通过可返回结果的协议激活应用程序时的事件参数
 *     Uri - 协议的 uri
 *     CallerPackageFamilyName - 激活当前 app 的 app 的 PackageFamilyName
 *     ProtocolForResultsOperation - 获取 ProtocolForResultsOperation 对象
 *     Data - 返回一个 ValueSet 对象，用于获取激活此 app 的 app 传递过来的数据
 *     Kind - 此 app 被激活的类型（ActivationKind 枚举）
 *         本例为 ActivationKind.ProtocolForResults
 *     PreviousExecutionState - 此 app 被激活前的状态（ApplicationExecutionState 枚举）
 *         比如，如果此 app 被激活前就是运行状态的或，则此值为 Running
 *     SplashScreen - 获取此 app 的 SplashScreen 对象
 *     User - 获取激活了此 app 的 User 对象
 *     
 * ProtocolForResultsOperation - 用于返回数据给激活当前 app 的 app
 *     ReportCompleted(ValueSet data) - 返回指定的数据
 *     
 * Launcher - 用于启动与指定 Uri 相关的应用程序
 *     LaunchUriAsync(Uri uri) - 打开指定的 Uri，无返回结果
 *     LaunchUriForResultsAsync(Uri uri, LauncherOptions options, ValueSet inputData) - 打开指定的 Uri，并可以获取到被激活的 app 返回的数据
 *         使用此种方式的话，必须要通过 LauncherOptions 的 TargetApplicationPackageFamilyName 指定目标程序的 PackageFamilyName
 *         使用此种方式的话，除了通过协议 uri 传递数据外，还可以通过 ValueSet 传递数据
 *         使用此种方式的话，会返回一个 LaunchUriStatus 类型的枚举数据
 *             Success - 成功激活了
 *             AppUnavailable - 没有通过 TargetApplicationPackageFamilyName 找到指定的 app
 *             ProtocolUnavailable - 指定的目标程序不支持此协议
 *             Unknown - 激活时发生了未知错误
 * 
 * LauncherOptions - 启动外部应用程序时的相关选项
 *     TargetApplicationPackageFamilyName - 指定目标程序的 PackageFamilyName
 *     
 *         
 * 注：通过 ValueSet 传递数据，最大不能超过 100KB
 */

using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Windows10.App2AppCommunication
{
    public sealed partial class LaunchUriForResults : Page
    {
        private ProtocolForResultsActivatedEventArgs _protocolForResultsArgs;
        private ProtocolForResultsOperation _protocolForResultsOperation;

        public LaunchUriForResults()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 获取 ProtocolForResultsActivatedEventArgs 对象（从 App.xaml.cs 传来的）
            _protocolForResultsArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;

            // 显示协议的详细信息
            if (_protocolForResultsArgs != null)
            {
                _protocolForResultsOperation = _protocolForResultsArgs.ProtocolForResultsOperation;

                grid.Background = new SolidColorBrush(Colors.Blue);
                lblMsg.Foreground = new SolidColorBrush(Colors.White);

                lblMsg.Text = "激活程序的自定义协议为: " + _protocolForResultsArgs.Uri.AbsoluteUri;
                lblMsg.Text += Environment.NewLine;

                if (_protocolForResultsArgs.Data.ContainsKey("InputData"))
                {
                    string inputData = _protocolForResultsArgs.Data["InputData"] as string;

                    lblMsg.Text += $"收到了数据：{inputData}";
                    lblMsg.Text += Environment.NewLine;
                }                

                btnOpenProtocol.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnBack.Visibility = Visibility.Collapsed;
            }
        }

        // 打开自定义协议 webabcd: 并传递数据给目标程序，然后获取目标程序的返回数据
        private async void btnOpenProtocol_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("webabcd:data");
            var options = new LauncherOptions();
            // 用 LaunchUriForResultsAsync() 的话必须要指定目标程序的 PackageFamilyName
            options.TargetApplicationPackageFamilyName = "48c7dd54-1ef2-4db7-a75e-7e02c5eefd40_vsy44gny1fmrj"; 

            ValueSet inputData = new ValueSet();
            inputData["InputData"] = "input data";

            lblMsg.Text = "打开 webabcd: 协议，并传递数据";
            lblMsg.Text += Environment.NewLine;

            LaunchUriResult result = await Launcher.LaunchUriForResultsAsync(uri, options, inputData);
            if (result.Status == LaunchUriStatus.Success && result.Result != null && result.Result.ContainsKey("ReturnData"))
            {
                ValueSet theValues = result.Result;
                string returnData = theValues["ReturnData"] as string;

                lblMsg.Text += $"收到返回数据：{returnData}";
                lblMsg.Text += Environment.NewLine;
            }
        }

        // 返回数据
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ValueSet result = new ValueSet();
            result["ReturnData"] = "return data";

            _protocolForResultsOperation.ReportCompleted(result);
        }
    }
}
