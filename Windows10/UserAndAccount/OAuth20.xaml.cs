/*
 * 演示如何开发一个基于 OAuth 2.0 验证的客户端 
 * 关于 OAuth 2.0 协议请参见：http://tools.ietf.org/html/draft-ietf-oauth-v2-20
 * 
 * WebAuthenticationBroker - 用于 OAuth 2.0 验证的第一步，可以将第三方 UI 无缝整合进 app
 *     AuthenticateAsync(WebAuthenticationOptions options, Uri requestUri, Uri callbackUri) - 请求 authorization code，返回一个 WebAuthenticationResult 类型的数据
 *
 * WebAuthenticationResult - 请求 authorization code（OAuth 2.0 验证的第一步）的结果
 *     ResponseData - 响应的数据         
 *     ResponseStatus - 响应的状态
 * 
 * 
 * 注：本例以微博开放平台为例
 */

using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Windows10.UserAndAccount
{
    public sealed partial class OAuth20 : Page
    {
        public OAuth20()
        {
            this.InitializeComponent();
        }

        private async void buttonWeibo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var appKey = "39261162";
                var appSecret = "652ec0b02f814d514fc288f3eab2efda";
                var callbackUrl = "http://webabcd.cnblogs.com"; // 在新浪微博开放平台设置的回调页

                var requestAuthorizationCode_url =
                    string.Format("https://api.weibo.com/oauth2/authorize?client_id={0}&response_type=code&redirect_uri={1}",
                    appKey,
                    callbackUrl);

                // 第一步：request authorization code
                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                    WebAuthenticationOptions.None,
                    new Uri(requestAuthorizationCode_url),
                    new Uri(callbackUrl));

                // 第一步的结果
                lblMsg.Text = WebAuthenticationResult.ResponseStatus.ToString() + Environment.NewLine;

                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    // 从第一步返回的数据中获取 authorization code
                    var authorizationCode = QueryString(WebAuthenticationResult.ResponseData, "code");
                    lblMsg.Text += "authorizationCode: " + authorizationCode + Environment.NewLine;

                    var requestAccessToken_url =
                        string.Format("https://api.weibo.com/oauth2/access_token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}",
                        appKey,
                        appSecret,
                        callbackUrl,
                        authorizationCode);

                    // 第二步：request access token
                    HttpClient client = new HttpClient();
                    var response = await client.PostAsync(new Uri(requestAccessToken_url), null);

                    // 第二步的结果：获取其中的 access token
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JsonObject jsonObject = JsonObject.Parse(jsonString);
                    var accessToken = jsonObject["access_token"].GetString();
                    lblMsg.Text += "accessToken: " + accessToken + Environment.NewLine;

                    var requestProtectedResource_url =
                        string.Format("https://api.weibo.com/2/statuses/friends_timeline.json?access_token={0}",
                        accessToken);

                    // 第三步：request protected resource，获取需要的数据（本例为获取登录用户好友最新发布的微博）
                    var result = await client.GetStringAsync(new Uri(requestProtectedResource_url)); // 由于本 app 没有提交微博开放平台审核，所以如果使用的账号没有添加到微博开放平台的测试账号中的话，是会出现异常的
                    lblMsg.Text += "result: " + result;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text += Environment.NewLine;
                lblMsg.Text += ex.ToString();
            }
        }

        /// <summary>
        /// 模拟 QueryString 的实现
        /// </summary>
        /// <param name="queryString">query 字符串</param>
        /// <param name="key">key</param>
        private string QueryString(string queryString, string key)
        {
            return Regex.Match(queryString, string.Format(@"(?<=(\&|\?|^)({0})\=).*?(?=\&|$)", key), RegexOptions.IgnoreCase).Value;
        }
    }
}

/*
 * OAuth 2.0 的 Protocol Flow
     +--------+                               +---------------+
     |        |--(A)- Authorization Request ->|   Resource    |
     |        |                               |     Owner     |
     |        |<-(B)-- Authorization Grant ---|               |
     |        |                               +---------------+
     |        |
     |        |                               +---------------+
     |        |--(C)-- Authorization Grant -->| Authorization |
     | Client |                               |     Server    |
     |        |<-(D)----- Access Token -------|               |
     |        |                               +---------------+
     |        |
     |        |                               +---------------+
     |        |--(E)----- Access Token ------>|    Resource   |
     |        |                               |     Server    |
     |        |<-(F)--- Protected Resource ---|               |
     +--------+                               +---------------+
*/
