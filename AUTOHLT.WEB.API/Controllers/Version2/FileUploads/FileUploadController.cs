using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AUTOHLT.WEB.API.Controllers.Version2.FileUploads
{
    [RoutePrefix("api/version2/fileupload")]
    public class FileUploadController : BaseController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> UploadFile()
        {
            //var rootPath = HttpContext.Current.Server.MapPath($"~/App_Data");
            // luu 1 file
            //var file = HttpContext.Current.Request.Files.Get("Avatar");
            // file.SaveAs($"{rootPath}/{file.FileName}");


            // luu nhieu file
            var rootPath = HttpContext.Current.Server.MapPath($"~/App_Data");
            var provider = new MultipartFormDataStreamProvider(rootPath);
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var fileData in provider.FileData)
            {
                var name = fileData.Headers.ContentDisposition.FileName;
                name = name.Trim('"');
                var localFileName = fileData.LocalFileName;
                var filePath = Path.Combine(rootPath, name);
                File.Move(localFileName, filePath);
            }
            return Ok();
        }
    }
}