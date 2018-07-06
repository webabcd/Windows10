/*
 * 演示数据账号的添加和管理
 * 
 * UserDataAccountManager - 数据账号管理器
 *     ShowAddAccountAsync() - 弹出账号添加界面
 *     ShowAccountSettingsAsync() - 弹出账号管理界面
 *     RequestStoreAsync() - 返回当前用户的数据账号存储区域
 *     GetForUser() - 返回指定用户的数据账号存储区域（通过返回的 UserDataAccountManagerForUser 对象的 RequestStoreAsync() 方法）
 *     
 * UserDataAccountStore - 数据账号存储区域
 *     FindAccountsAsync() - 返回所有的数据账号
 *     GetAccountAsync() - 返回指定的数据账号
 *     
 * UserDataAccount - 数据账号
 *     UserDisplayName - 用户名
 *     Id - 数据账号在本地设备上的唯一标识
 *     SaveAsync() - 保存
 *     DeleteAsync() - 删除
 *     ... 还有很多其他属性和方法
 *     
 *     
 * 注：根据使用的功能需要在 Package.appxmanifest 做相关配置
 * 1、用到 Windows.System.User 的话需要配置 <Capability Name="userAccountInformation" />
 * 2、还可能需要 <Capability Name="appointments" />, <Capability Name="contacts" />
 */

using System;
using System.Linq;
using System.Collections.Generic;
using Windows.ApplicationModel.UserDataAccounts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UserAndAccount
{
    public sealed partial class DataAccount : Page
    {
        public DataAccount()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 获取当前用户下的全部数据账号
            UserDataAccountStore store = await UserDataAccountManager.RequestStoreAsync(UserDataAccountStoreAccessType.AllAccountsReadOnly);
            IReadOnlyList<UserDataAccount> accounts = await store.FindAccountsAsync();
            lblMsg.Text += string.Join(",", accounts.Select(p => p.UserDisplayName));
            lblMsg.Text += Environment.NewLine;
        }

        private async void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            // 弹出账号添加界面，如果添加成功会返回新建的数据账号的在本地设备上的唯一标识
            string userDataAccountId = await UserDataAccountManager.ShowAddAccountAsync(UserDataAccountContentKinds.Email | UserDataAccountContentKinds.Appointment | UserDataAccountContentKinds.Contact);

            if (string.IsNullOrEmpty(userDataAccountId))
            {
                lblMsg.Text += "用户取消了或添加账号失败";
                lblMsg.Text += Environment.NewLine;
            }
            else
            {
                UserDataAccountStore store = await UserDataAccountManager.RequestStoreAsync(UserDataAccountStoreAccessType.AllAccountsReadOnly);
                if (store != null)
                {
                    // 通过数据账号在本地设备上的唯一标识来获取 UserDataAccount 对象
                    UserDataAccount account = await store.GetAccountAsync(userDataAccountId);
                    lblMsg.Text += "新增的数据账号：" + account.UserDisplayName;
                }
            }
        }
    }
}
