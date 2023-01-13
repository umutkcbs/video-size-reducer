using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using VideoEditorBE.Controllers;
using static System.Net.Mime.MediaTypeNames;

//[DllImport("user32.dll")]

namespace VideoEditorBE.Processor
{
    public class VideoProcessor
    {
        public static string outputName = "";
        public static void Processs(string fPath)
        {
            EditorController.status = "Processing...";
            try
            {
                outputName = DateTime.Now.Ticks.ToString() + ".mp4";

                Process process = new Process();
                process.StartInfo.FileName = "ffmpeg";
                process.StartInfo.Arguments = "-i " + fPath + " -vf \"pad=ceil(iw/2)*2:ceil(ih/2)*2\" -acodec copy -threads 12 -vcodec libx264 -crf 28 " + outputName;
                process.Start();
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                EditorController.status = ex.ToString();
            }

        }
    }
}
