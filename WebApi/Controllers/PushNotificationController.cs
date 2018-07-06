/*
 * 演示如何向 app 推送通知
 * 由于本例没有上商店，所以本例是无法演示的，需要看演示效果的话运行一下自己写的“打字通”的 /TypingGame/PushNotification/Sample.xaml，然后用其生成的 channel 地址在 /WebApi/Controllers/PushNotificationController.cs 推送通知
 *
 *
 * 注：
 * 关于推送通知服务请求和响应头的详细说明参见：https://msdn.microsoft.com/zh-cn/library/windows/apps/hh465435.aspx
 */

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class PushNotificationController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            // 向某个 app 推送通知的 channel 地址（通过客户端的 PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync() 方法生成）
            string notifyUrl = "https://hk2.notify.windows.com/?token=AwYAAAANZzLsCX%2fl1aavCSQhi%2fdEBO5wdplj7S4a3o4t8wGSGo05hRE6VC7xEMCFtGDrVuV%2f9J2ItuVri1F4Z0YNjtbuCqf6LQvov0UE3%2flD1sP1poaS1Qp30UQ%2fWVKVUBCjPFuWFLuyuq7UuuTvJcCcQzey";

            // 在商店后台的 dashboard 中的“Package SID”中可以找到此值（可以在 https://apps.dev.microsoft.com/ 中查找）
            string sid = "ms-app://s-1-15-2-1792688850-3283391166-**********-**********-**********-1809961044-230289451";
            // 在商店后台的 dashboard 中的“Application Secrets”中可以找到此值（可以在 https://apps.dev.microsoft.com/ 中查找）
            string secret = "koghs4zz*************S+5sEoqoNb4";

            OAuthHelper oAuth = new OAuthHelper();
            OAuthToken token = oAuth.GetAccessToken(secret, sid);

            HttpResponseMessage result = null;

            try
            {
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, notifyUrl);

                // 推送消息的类型：wns/toast | wns/badge | wns/tile | wns/raw
                request.Headers.Add("X-WNS-Type", "wns/toast");
                // 设置 access-token
                request.Headers.Add("Authorization", String.Format("Bearer {0}", token.AccessToken));

                // 需要推送的 toast 通知的内容
                string toastXml = $@"
                    <toast activationType='foreground' launch='PushNotification-Toast-Arguments'>
                        <visual>
                            <binding template='ToastGeneric'>
                                <text>toast - title</text>
                                <text>toast - content {DateTime.Now.ToString("mm:ss")}</text>
                            </binding>
                        </visual>
                    </toast>";

                // toast, tile, badge 为 text/xml; raw 为 application/octet-stream
                request.Content = new StringContent(toastXml, Encoding.UTF8, "text/xml");

                HttpResponseMessage response = await httpClient.SendAsync(request);
                /*
                 * 响应代码说明
                 *     200 - OK，WNS 已接收到通知
                 *     400 - 错误的请求
                 *     401 - 未授权，token 可能无效
                 *     403 - 已禁止，manifest 中的 identity 可能不对
                 *     404 - 未找到
                 *     405 - 方法不允许
                 *     406 - 无法接受
                 *     410 - 不存在，信道不存在或过期
                 *     413 - 请求实体太大，限制为 5000 字节
                 *     500 - 内部服务器错误
                 *     503 - 服务不可用
                 */
                HttpStatusCode statusCode = response.StatusCode;
                result = new HttpResponseMessage
                {
                    Content = new StringContent(statusCode.ToString(), Encoding.UTF8, "text/html")
                };
            }
            catch (Exception ex)
            {
                result = new HttpResponseMessage
                {
                    Content = new StringContent(ex.ToString(), Encoding.UTF8, "text/html"),
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            return result;
        }
    }



    /*
     * 用于反序列化从 https://login.live.com/accesstoken.srf 获取到的结果
     */
    [DataContract]
    public class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }



    /*
     * 用于从 https://login.live.com/accesstoken.srf 做 OAuth 验证的帮助类
     */
    public class OAuthHelper
    {
        /// <summary>
        /// 获取 https://login.live.com/accesstoken.srf 的 OAuth 验证的 access-token
        /// </summary>
        /// <param name="secret">在商店后台的 dashboard 中的“Application Secrets”中可以找到此值（可以在 https://apps.dev.microsoft.com/ 中查找）</param>
        /// <param name="sid">在商店后台的 dashboard 中的“Package SID”中可以找到此值（可以在 https://apps.dev.microsoft.com/ 中查找）</param>
        /// <returns></returns>
        public OAuthToken GetAccessToken(string secret, string sid)
        {
            var urlEncodedSecret = UrlEncode(secret);
            var urlEncodedSid = UrlEncode(sid);
            var body = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com",
                                     urlEncodedSid,
                                     urlEncodedSecret);

            string response;
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = client.UploadString("https://login.live.com/accesstoken.srf", body);
            }
            return GetOAuthTokenFromJson(response);
        }

        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        private static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
    }
}
