/*
 * 用于演示 uwp 的后台上传任务
 */

using HttpMultipartParser;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class UploadController : ApiController
    {
        private Random _random = new Random();

        [HttpPost]
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;

            try
            {
                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("/UploadFiles/"));
                if (!d.Exists)
                {
                    d.Create();
                }

                // 用于保存 uwp 后台上传任务上传的单个文件
                if (HttpContext.Current.Request.Headers["Filename"] != null)
                {
                    string fileName = HttpContext.Current.Request.Headers["Filename"];
                    string filePath = HttpContext.Current.Server.MapPath("/UploadFiles/" + Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName));
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        HttpContext.Current.Request.InputStream.CopyTo(fs);
                    }
                }
                // 用于保存 uwp 后台上传任务上传的多个文件
                else
                {
                    Stream stream = HttpContext.Current.Request.InputStream;
                    MultipartFormDataParser parser = new MultipartFormDataParser(stream);

                    for (int i = 0; i < parser.Files.Count; i++)
                    {
                        FilePart filePart = parser.Files[i];
                        string fileName = filePart.FileName;
                        string filePath = HttpContext.Current.Server.MapPath("/UploadFiles/" + Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName));
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            byte[] bytes = new byte[filePart.Data.Length];
                            filePart.Data.Read(bytes, 0, (int)bytes.Length);
                            filePart.Data.Close();

                            fs.Write(bytes, 0, bytes.Length);
                        }
                    }

                    stream.Close();
                }
                /*
                // 用于保存 uwp 后台上传任务上传的多个文件（经测试无法工作，所以改用了上面的 HttpMultipartParser 方案）
                else
                {
                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        var file = HttpContext.Current.Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(file.FileName);
                            string filePath = HttpContext.Current.Server.MapPath("/UploadFiles/" + Path.GetFileNameWithoutExtension(fileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(fileName));
                            file.SaveAs(filePath);
                        }
                    }
                }
                */

                result = new HttpResponseMessage
                {
                    Content = new StringContent("", Encoding.UTF8, "text/html")
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
}
