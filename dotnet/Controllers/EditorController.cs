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
                    string args = "/select, \"" + VideoProcessor.outputName + "\""; //AppDomain.CurrentDomain.BaseDirectory;
                    Process.Start("explorer.exe", args);
                    statusChecker = "not running";
                    status = VideoProcessor.outputName;
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
