/*
 * 用于 WebView 演示“如何加载指定 HttpMethod 的请求，以及如何自定义请求的 http header”
 */

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class WebViewPostController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post()
        {
            Stream stream = await this.Request.Content.ReadAsStreamAsync();
            StreamReader sr = new StreamReader(stream);
            string postData = sr.ReadToEnd();
            sr.Dispose();

            string myHeader = this.Request.Headers.GetValues("myHeader").FirstOrDefault();

            HttpResponseMessage result = new HttpResponseMessage
            {
                Content = new StringContent($"post data: {postData}<br /> myHeader: {myHeader}", Encoding.UTF8, "text/html")
            };

            return result;
        }
    }
}
