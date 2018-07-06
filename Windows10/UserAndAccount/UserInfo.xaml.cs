/*
 * 演示如何获取用户的信息
 * 
 * 需要在 Package.appxmanifest 中的“功能”中勾选“用户账户信息”，即 <Capability Name="userAccountInformation" />
 * 如上配置之后，即可通过 api 获取用户的相关信息（系统会自动弹出权限请求对话框）
 * 
 * User - 用户
 *     FindAllAsync() - 查找全部用户，也可以根据 UserType 和 UserAuthenticationStatus 来查找用户
 *         经过测试，其只能返回当前登录用户
 *     GetPropertyAsync(), GetPropertiesAsync() - 获取用户的指定属性
 *         可获取的属性请参见 Windows.System.KnownUserProperties
 *     GetPictureAsync() - 获取用户图片
 *         图片规格有 64x64, 208x208, 424x424, 1080x1080
 *     NonRoamableId - 用户 id
 *         此 id 不可漫游
 *     UserType - 用户类型
 *         LocalUser, RemoteUser, LocalGuest, RemoteGuest
 *     UserAuthenticationStatus - 用户的身份验证状态
 *         Unauthenticated, LocallyAuthenticated, RemotelyAuthenticated
 *     CreateWatcher() - 返回 UserWatcher 对象，用于监听用户的状态变化
 *         本例不做演示
 */

using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UserAndAccount
{
    public sealed partial class UserInfo : Page
    {
        public UserInfo()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 我这里测试的结果是：返回的集合中只有一个元素，就是当前的登录用户
            IReadOnlyList<User> users = await User.FindAllAsync(); // 系统会自动弹出权限请求对话框
            User user = users?[0];
            if (user != null)
            {
                // 对于获取用户的 NonRoamableId, Type, AuthenticationStatus 信息，不同意权限请求也是可以的
                string result = "NonRoamableId: " + user.NonRoamableId + "\n"; 
                result += "Type: " + user.Type.ToString() + "\n";
                result += "AuthenticationStatus: " + user.AuthenticationStatus.ToString() + "\n";

                // 对于获取用户的如下信息及图片，则必须要同意权限请求
                string[] desiredProperties = new string[]
                {
                    KnownUserProperties.DisplayName,
                    KnownUserProperties.FirstName,
                    KnownUserProperties.LastName,
                    KnownUserProperties.ProviderName,
                    KnownUserProperties.AccountName,
                    KnownUserProperties.GuestHost,
                    KnownUserProperties.PrincipalName,
                    KnownUserProperties.DomainName,
                    KnownUserProperties.SessionInitiationProtocolUri,
                };
                // 获取用户的指定属性集合
                IPropertySet values = await user.GetPropertiesAsync(desiredProperties);
                foreach (string property in desiredProperties)
                {
                    result += property + ": " + values[property] + "\n";
                }
                // 获取用户的指定属性
                // object displayName = await user.GetPropertyAsync(KnownUserProperties.DisplayName);
                
                lblMsg.Text = result;


                // 获取用户的图片
                IRandomAccessStreamReference streamReference = await user.GetPictureAsync(UserPictureSize.Size64x64);
                if (streamReference != null)
                {
                    IRandomAccessStream stream = await streamReference.OpenReadAsync();
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    imageProfile.Source = bitmapImage;
                }
            }
        }
    }
}
