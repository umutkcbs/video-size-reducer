using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using VideoEditorBE.Processor;

namespace VideoEditorBE.Controllers
{
    [Route("api")]
    [ApiController]
    public class EditorController : Controller
    {
        public static string status = "Waiting";
        public static string statusChecker = " ";

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /*
        [HttpPost("/upload")]
        public async Task<IHttpActionResult> VideoUpload()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binary data.
            }

            return Ok();
        } */

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IList<IFormFile> files)
        {
            string fPath = "";
            var filePath = Path.GetTempFileName();
            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length > 0)
                {
                    using (var inputStream = new FileStream(filePath, FileMode.Create))
                    {
                        // read file to stream
                        await formFile.CopyToAsync(inputStream);
                        // stream to byte array
                        byte[] array = new byte[inputStream.Length];
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.Read(array, 0, array.Length);
                        // get file name
                        string fName = formFile.FileName;
                        fPath = inputStream.Name;
                        Console.WriteLine(inputStream.Name);

                    }
                    VideoProcessor.Processs(fPath);
                }
            }
            //System.IO.File.Delete(fPath);
            return this.RedirectToAction("Index");
        }

        [HttpGet("status")]
        public string Status()
        {
            Process[] pname = Process.GetProcessesByName("ffmpeg");
            if (pname.Length == 0)
            {
                if (statusChecker == "runned")
                {
                    Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory);
                    statusChecker = "not running";
                    status = "Done!";
                }
            }
            else
            {
                status = "Processing...";
                statusChecker = "runned";
            }


            //return AppDomain.CurrentDomain.BaseDirectory;
            return status;
        }
    }
}
