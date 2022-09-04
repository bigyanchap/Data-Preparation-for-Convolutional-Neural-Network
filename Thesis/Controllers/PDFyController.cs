using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;

/***
 * FILE PROCESSOR: 
 * combine png images to pdf.
 * Author: Bigyan Chapagain
 * Date: September 4, 2022
 * ***/
namespace Thesis.Controllers
{
    [Route("[controller]")]
    public class PDFyController : ControllerBase
    {
        public PDFyController(){}

        [HttpGet, Route("pdfy")]
        public async Task<IActionResult> PDFy()
        {
            try
            {
                string sourceLocation = "E:\\Training\\2022MEAN_Slides";
                string destLocation = "E:\\Training\\2022MEAN_AutoPDFy";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    //The directory name that contains images will be pdf filename.
                    var pdfFileName = dir_splits[dir_splits.Length - 1];
                    var destFullPathAndFileName = Path.Combine(destLocation + "\\" + pdfFileName+".pdf");
                    string[] files = Directory.GetFiles(dir);
                    //convert memory stream to file stream
                    MemoryStream memStream = CreatePDFFromImages(files);
                    Stream streamToWriteTo = System.IO.File.Open(destFullPathAndFileName, FileMode.Create);
                    await memStream.CopyToAsync(streamToWriteTo);
                }
                return Ok(new { Success = true, Message = "Images converted to PDF file Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }

        private MemoryStream CreatePDFFromImages(string[] files)
        {
            MemoryStream memStream = new MemoryStream();
            using (MagickImageCollection images = new MagickImageCollection())

            {
                
                foreach(var file in files)
                {
                    if (file.ToLower().Contains(".png"))
                    {
                        MagickImage img = new MagickImage(file);
                        img.Format = MagickFormat.Pdf;
                        images.Add(img);
                    }
                }
                images.Write(memStream); // Write all image to MemoryStream
                memStream.Position = 0;
                return memStream;
            }

        }

    }
    
}
