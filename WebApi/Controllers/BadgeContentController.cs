/*
 * 用于演示“轮询服务端以更新 badge 通知”的服务端部分
 */

using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class BadgeContentController : ApiController
    {
        private Random _random = new Random();

        [HttpGet]
        public HttpResponseMessage Get()
        {
            string badgeXml = $"<badge value='{_random.Next(1, 100)}'/>";

            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(badgeXml, Encoding.UTF8, "text/html")
            };

            return result;
        }
    }
}
