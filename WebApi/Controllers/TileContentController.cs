/*
 * 用于演示“轮询服务端以更新 tile 通知”的服务端部分
 */

using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class TileContentController : ApiController
    {
        private Random _random = new Random();

        [HttpGet]
        public HttpResponseMessage Get()
        {
            string tileXml = $@"
                <tile>
                    <visual>
                        <binding template='TileSmall'>
                            <text>Small（小）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileMedium'>
                            <text>Medium（中）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileWide'>
                            <text>Wide（宽）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                        <binding template='TileLarge'>
                            <text>Large（大）{DateTime.Now.ToString("HH:mm:ss")}</text>
                        </binding>
                    </visual>
                </tile>";

            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent(tileXml, Encoding.UTF8, "text/html")
            };

            // Tile 通知的过期时间，超过这个时间就会清除这个 Tile（对于“轮询服务端以更新 Tile 通知”来说，如果不指定此值，则默认 3 天后过期）
            // 通过 ToString("R") 可以把日期格式化为“RFC 1123”格式
            result.Headers.Add("X-WNS-Expires", DateTime.UtcNow.AddSeconds(60).ToString("R")); // 60 秒后过期

            // 在启用 tile 的队列功能时，如果 tile 的 Tag 相同则新的内容会更新旧的内容（Tag 值的前 16 个字符相同则认为是相同的 Tag）
            // 不指定的 Tag 的话则认为 Tag 都是不同的
            result.Headers.Add("X-WNS-Tag", _random.Next().ToString());

            return result;
        }
    }
}
