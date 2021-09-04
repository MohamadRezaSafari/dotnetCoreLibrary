using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Providers
{
    public class UploadCore
    {
        private readonly string[] ImageExtensions = new string[] { ".jpeg", ".jpg", ".png", ".gif" };
        private readonly string[] DocExtensions = new string[] { ".pdf", ".doc", ".docx", ".pptx", ".ppt" };


        // string name = await _uploadCore.ImageApi(file, _hostingEnvironment.WebRootPath , Request.Form["extension"], 3);
        public async Task<string> ImageApi(IFormFile file, dynamic Path, string extension, int maxSize)
        {
            if (FileSizeCore.KB(file.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSizeCore.Mb(file.Length) > maxSize)
            {
                throw new Exception("Maximum Size = " + maxSize + " mb");
            }
            if (!ImageExtensions.Contains(extension))
            {
                throw new Exception("Invalid extension");
            }

            var name = RandCore.GUID() + extension;
            var savePath = Path + $"{"/images/upload/"}\\{name}";

            using (var _stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(_stream);
            }

            return name;
        }


        // string name = await _uploadCore.ImageApi(file, _hostingEnvironment.WebRootPath , Request.Form["extension"], 3, 100, 100);
        public async Task<string> ImageApi(IFormFile file, dynamic Path, string extension, int maxSize , int width, int height)
        {
            if (FileSizeCore.KB(file.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSizeCore.Mb(file.Length) > maxSize)
            {
                throw new Exception("Maximum Size = " + maxSize + " mb");
            }
            if (!ImageExtensions.Contains(extension))
            {
                throw new Exception("Invalid extension");
            }

            var name = RandCore.GUID() + extension;
            var savePath = Path + $"{"/images/upload/"}\\{name}";

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var fileBytes = ms.ToArray();

                var img = Image.FromStream(ms);
                var thumbnail = img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                thumbnail.Save(savePath, ImageFormat.Jpeg);
            }

            return name;
        }

        // string name = await _uploadCore.FileApi(file, _hostingEnvironment.WebRootPath, Request.Form["extension"], 3);
        public async Task<string> FileApi(IFormFile file, dynamic Path, string extension, int maxSize)
        {
            if (FileSizeCore.KB(file.Length) <= 0)
            {
                throw new Exception("File is empty");
            }
            if (FileSizeCore.Mb(file.Length) > maxSize)
            {
                throw new Exception("Maximum Size = " + maxSize + " mb");
            }
            if (!DocExtensions.Contains(extension))
            {
                throw new Exception("Invalid extension");
            }

            var name = RandCore.Mix() + extension;
            var savePath = Path + $"{"/files/"}\\{name}";

            using (var _stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(_stream);
            }

            return name;
        }



        public void DeleteFile(string dir, string name)
        {
            try
            {
                var fullPath = dir + name;

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
