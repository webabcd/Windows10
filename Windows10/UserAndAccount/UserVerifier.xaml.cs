/*
 * 演示如何获取用户的同意
 * 
 * UserConsentVerifier - 验证器（比如 pin 验证等）
 *     CheckAvailabilityAsync() - 验证器的可用性
 *     RequestVerificationAsync(string message) - 请求用户的同意（可以指定用于提示用户的信息）
 */

using System;
using Windows.Security.Credentials.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Windows10.UserAndAccount
{
    public sealed partial class UserVerifier : Page
    {
        public UserVerifier()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                UserConsentVerifierAvailability verifierAvailability = await UserConsentVerifier.CheckAvailabilityAsync();
                switch (verifierAvailability)
                {
                    case UserConsentVerifierAvailability.Available: // 验证器可用
                        lblMsg.Text = "UserConsentVerifierAvailability.Available";
                        break;
                    case UserConsentVerifierAvailability.DeviceBusy:
                        lblMsg.Text = "UserConsentVerifierAvailability.DeviceBusy";
                        break;
                    case UserConsentVerifierAvailability.DeviceNotPresent:
                        lblMsg.Text = "UserConsentVerifierAvailability.DeviceNotPresent";
                        break;
                    case UserConsentVerifierAvailability.DisabledByPolicy:
                        lblMsg.Text = "UserConsentVerifierAvailability.DisabledByPolicy";
                        break;
                    case UserConsentVerifierAvailability.NotConfiguredForUser:
                        lblMsg.Text = "UserConsentVerifierAvailability.NotConfiguredForUser";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.ToString();
            }

            lblMsg.Text += "\n";
        }

        private async void buttonRequestConsent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserConsentVerificationResult consentResult = await UserConsentVerifier.RequestVerificationAsync("我要做一些操作，您同意吗？");
                switch (consentResult)
                {
                    case UserConsentVerificationResult.Verified: // 验证通过
                        lblMsg.Text += "UserConsentVerificationResult.Verified";
                        break;
                    case UserConsentVerificationResult.DeviceBusy:
                        lblMsg.Text += "UserConsentVerificationResult.DeviceBusy";
                        break;
                    case UserConsentVerificationResult.DeviceNotPresent:
                        lblMsg.Text += "UserConsentVerificationResult.DeviceNotPresent";
                        break;
                    case UserConsentVerificationResult.DisabledByPolicy:
                        lblMsg.Text += "UserConsentVerificationResult.DisabledByPolicy";
                        break;
                    case UserConsentVerificationResult.NotConfiguredForUser:
                        lblMsg.Text += "UserConsentVerificationResult.NotConfiguredForUser";
                        break;
                    case UserConsentVerificationResult.RetriesExhausted:
                        lblMsg.Text += "UserConsentVerificationResult.RetriesExhausted";
                        break;
                    case UserConsentVerificationResult.Canceled: // 验证取消
                        lblMsg.Text += "UserConsentVerificationResult.Canceled";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text += ex.ToString();
            }

            lblMsg.Text += "\n";
        }
    }
}
