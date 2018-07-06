/*
 * 演示如何将微软账号的登录和注销集成到 app 中
 * 
 * AccountsSettingsPane - 账号配置界面
 *     Show() - 弹出界面
 *     GetForCurrentView() - 获取当前的 AccountsSettingsPane 实例
 *     AccountCommandsRequested - 弹出界面时触发的事件（事件参数为：AccountsSettingsPaneCommandsRequestedEventArgs）
 *     
 * AccountsSettingsPaneCommandsRequestedEventArgs
 *     GetDeferral() - 获取异步操作对象
 *     HeaderText - 弹出界面上需要显示的标题文字
 *     Commands - 返回一个 SettingsCommand 列表，用于在界面上添加自定义按钮
 * 
 *     
 *     
 * 流程简述：
 * 
 * 登录第一步，将指定的 web 账号提供商添加到账号登录界面中
 * WebAccountProvider - web 账号提供商（本例以微软账号为例）
 * WebAuthenticationCoreManager - 用于获取指定的 WebAccountProvider
 * WebAccountProviderCommand - 用于将指定的 WebAccountProvider 添加到账号登录界面中
 * 
 * 登录第二步，用户在账号登录界面中登录后
 * WebTokenRequest - 通过指定的 WebAccountProvider 获取这个 web 请求对象
 * WebTokenRequestResult - 通过 WebAuthenticationCoreManager 和指定的 WebTokenRequest 获取这个 web 请求结果
 * 
 * 注销第一步，将指定的 web 账号添加到账号管理界面中
 * WebAccount - 通过 WebAuthenticationCoreManager 和指定的 WebAccountProvider 和指定的 userId 获取这个 web 账号对象
 * WebAccountCommand - 用于将指定的 WebAccount 添加到账号管理界面中
 * 
 * 注销第二步，用户在账号管理界面中选择注销后
 * WebAccount - 可以调用这个对象的注销方法
 * 
 * 
 * 
 * 注：
 * 本例测试时会报 ProviderError 错误，因为“you must register your app under the Microsoft Store with a Microsoft developer account”
 * 给自己一个提醒，如果需要看效果的话，请参见自己写的“打字通”app 中的 MicrosoftAccount.xaml
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UserAndAccount
{
    public sealed partial class MicrosoftAccount : Page
    {
        const string MicrosoftAccountProviderId = "https://login.microsoft.com";
        const string ConsumerAuthority = "consumers";
        const string AccountScopeRequested = "wl.basic";
        const string AccountClientId = "none";

        private string _userId = null;

        public MicrosoftAccount()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 弹出账号配置界面时触发的事件
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += OnAccountCommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= OnAccountCommandsRequested;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // 弹出账号配置界面
            AccountsSettingsPane.Show();
        }

        private async void OnAccountCommandsRequested(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            AccountsSettingsPaneEventDeferral deferral = e.GetDeferral();

            if (_userId == null)
            {
                // 弹出账号配置界面用于账号登录
                await ShowLoginUI(e);
                // 弹出界面上需要显示的标题文字
                e.HeaderText = "请登录";
            }
            else
            {
                // 弹出账号配置界面用于账号管理（本例用于演示注销）
                await ShowLogoutUI(e);
                // 弹出界面上需要显示的标题文字
                e.HeaderText = "请注销";
            }

            // 在弹出界面上添加自定义按钮
            e.Commands.Add(new SettingsCommand("help", "帮助", HelpInvoked));
            e.Commands.Add(new SettingsCommand("about", "关于", AboutInvoked));

            deferral.Complete();
        }

        private void HelpInvoked(IUICommand command)
        {
            lblMsg.Text = "用户点击了“帮助”按钮";
        }

        private void AboutInvoked(IUICommand command)
        {
            lblMsg.Text = "用户点击了“关于”按钮";
        }



        // 将指定的 web 账号提供商添加到账号登录界面中
        private async Task ShowLoginUI(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            WebAccountProvider provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);
            WebAccountProviderCommand providerCommand = new WebAccountProviderCommand(provider, WebAccountProviderCommandInvoked);
            e.WebAccountProviderCommands.Add(providerCommand);
        }

        // 用户在账号登录界面中登录后
        private async void WebAccountProviderCommandInvoked(WebAccountProviderCommand command)
        {
            try
            {
                WebTokenRequest webTokenRequest = new WebTokenRequest(command.WebAccountProvider, AccountScopeRequested, AccountClientId);

                WebTokenRequestResult webTokenRequestResult = await WebAuthenticationCoreManager.RequestTokenAsync(webTokenRequest);

                if (webTokenRequestResult.ResponseStatus == WebTokenRequestStatus.Success)
                {
                    var response = webTokenRequestResult.ResponseData[0];
                    if (response != null)
                    {
                        // 拿到 userId，实际开发中一般会将此信息保存到本地
                        string userId = response.WebAccount.Id;
                        _userId = userId;
                        lblMsg.Text = "登录成功，userId：" + userId;
                        lblMsg.Text += Environment.NewLine;

                        /*
                         * 微软账号的登录过程到此就完成了
                         * 如果想通过 response.WebAccount.UserName 这种方式来获取账号的用户名或其他信息的话，那是获取不到的
                         * 需要带着 response.Token 去请求 apis.live.net 来获取相关信息，说明如下
                         */

                        // 登录成功后，要通过拿到的 token 像下面这样获取用户信息
                        var restApi = new Uri(@"https://apis.live.net/v5.0/me?access_token=" + response.Token);

                        using (var client = new HttpClient())
                        {
                            var infoResult = await client.GetAsync(restApi);
                            string content = await infoResult.Content.ReadAsStringAsync();
                            /* 获取到的内容类似如下
                             {
                               "id": "abcd1234abcd1234", 
                               "name": "王磊", 
                               "first_name": "磊", 
                               "last_name": "王", 
                               "link": "https://profile.live.com/", 
                               "gender": null, 
                               "locale": "zh_CN", 
                               "updated_time": "2017-04-27T02:24:58+0000"
                             }
                             */
                            var jsonObject = JsonObject.Parse(content);
                            string name = jsonObject["name"].GetString() ?? "";
                            string locale = jsonObject["locale"].GetString() ?? "";
                            lblMsg.Text += $"name:{name}, locale:{locale}";

                            // 通过如下方式拿到用户图片
                            string profileId = jsonObject["id"].GetString() ?? "";
                            string pictureUrl = $"https://apis.live.net/v5.0/{profileId}/picture" ?? "";
                            imagePicture.ImageSource = new BitmapImage(new Uri(pictureUrl));
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "登录失败：" + webTokenRequestResult.ResponseStatus.ToString();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }
        }



        // 将指定的 web 账号添加到账号管理界面中
        private async Task ShowLogoutUI(AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            WebAccountProvider provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(MicrosoftAccountProviderId, ConsumerAuthority);

            WebAccount account = await WebAuthenticationCoreManager.FindAccountAsync(provider, _userId);

            if (account != null)
            {
                // 最后一个参数用于指定当用户选择了账号后，会出现哪些功能按钮（我这里测试只有 SupportedWebAccountActions.Remove 是有效的）
                WebAccountCommand command = new WebAccountCommand(account, WebAccountCommandInvoked, SupportedWebAccountActions.Remove);
                e.WebAccountCommands.Add(command);
            }
            else
            {
                _userId = null;
            }
        }

        // 用户在账号管理界面中选择了某项功能
        private async void WebAccountCommandInvoked(WebAccountCommand command, WebAccountInvokedArgs args)
        {
            // 用户选择的是注销
            if (args.Action == WebAccountAction.Remove)
            {
                await command.WebAccount.SignOutAsync();

                _userId = null;

                lblMsg.Text = "注销成功";
                imagePicture.ImageSource = null;
            }
        }
    }
}