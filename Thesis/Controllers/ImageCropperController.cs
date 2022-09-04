using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Thesis.Controllers
{
    [Route("[controller]")]
    public class ImageCropperController : ControllerBase
    {
        public ImageCropperController(){}

        [HttpGet, Route("cropImages")]
        public async Task<IActionResult> CropImages()
        {
            try
            {
                string sourceLocation = "E:\\Training\\2022MEAN_Slides";
                string destLocation = "E:\\Training\\2022MEAN_Slides_Cropped";

                string[] directories = Directory.GetDirectories(sourceLocation);

                foreach (var dir in directories)
                {
                    string[] dir_splits = dir.Split('\\');
                    var _destLocation = destLocation + "\\" + dir_splits[dir_splits.Length - 1];//create dest by picking the leaf folder

                    //create folder(s) unless non-existant
                    if (!System.IO.Directory.Exists(_destLocation))
                        System.IO.Directory.CreateDirectory(_destLocation);

                    string[] files = Directory.GetFiles(dir);
                    var files_count = files.Length - 1;

                    for (int i = 0; i < files.Length; i++)
                    {
                        var sourceFileFullPath = files[i];
                        var cropArea = new Rectangle(15, 100, 1500, 770);
                        var croppedImageBmp = cropImage(sourceFileFullPath,cropArea);
                        var destFileFullPath = System.IO.Path.Combine(_destLocation, "slide"+(i+1)+".png");
                        croppedImageBmp.Save(destFileFullPath, ImageFormat.Png);
                    }
                }
                return Ok(new { Success = true, Message = "Images converted to PDF file Successfully." });
            }
            catch (Exception ex)
            {
                return Ok(new { Success = false, Message = "Something went wrong." });
            }
        }

        private ImageCodecInfo GetEncoderInfo(string v)
        {
            throw new NotImplementedException();
        }

        private static Bitmap cropImage(string path, Rectangle cropArea)
        {
            Bitmap bmpImage = (Bitmap)Bitmap.FromFile(path);
            return bmpImage.Clone(cropArea,bmpImage.PixelFormat);
        }
    }
    
}
