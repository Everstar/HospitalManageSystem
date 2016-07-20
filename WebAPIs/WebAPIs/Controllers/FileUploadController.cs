using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPIs.Controllers
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        [Route("api/upload2")]
        public Task<HttpResponseMessage> UploadImagePost()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType));
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o =>
                {
                    // Show all the key-value pairs.
                    NameValueCollection nvc = HttpContext.Current.Request.Form;
                    foreach (string kvp in nvc.AllKeys)
                    {
                        Console.WriteLine(kvp);
                    }

                    FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

                    string guid = Guid.NewGuid().ToString();

                    File.Move(finfo.FullName, Path.Combine(root, guid + "_" + provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", "")));

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("File uploaded.")
                    };
                }
            );
            return task;
        }
        [HttpPost]
        [Route("api/upload")]
        public async Task<string> Upload()
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads");

            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo finfo = new FileInfo(provider.FileData.First().LocalFileName);

            string guid = Guid.NewGuid().ToString();
            string path = Path.Combine(root, guid + "_" + provider.FileData.First().Headers.ContentDisposition.FileName.Replace("\"", ""));
            File.Move(finfo.FullName, path);


            var myParameter = provider.FormData.GetValues("userid").FirstOrDefault();
            var count = provider.FileData.Count;

            return count + " / " + myParameter;
        }
    }
}
