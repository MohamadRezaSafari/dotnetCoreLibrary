using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
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
        private readonly string[] ZipExtensions = new string[] { ".zip", ".rar" };
        private readonly string[] VideoExtensions = new string[] { ".mp4", ".webm", ".ogg", ".gif" };
        private string ImageName;
        private string ImageName_thumb;
        private string name;


        // await _uploadCore.UploadImageAsync(_hostingEnvironment.WebRootPath, "upload", 10, pic);
        public async Task<string> UploadImageAsync(dynamic path, string folder, int MaxSize, IFormFile file)
        {
            if (file != null && file.Length > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                if (FileSizeCore.Mb(file.Length) > MaxSize)
                    throw new Exception("Big File!");

                try
                {
                    var savePath = path + $"{"/img/"}\\{folder}\\";
                    ImageName = folder + "_" + RandCore.Mix() + Path.GetExtension(file.FileName);
                    string _path = Path.Combine(savePath, Path.GetFileName(ImageName));
                    using (var stream = File.Create(_path))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            return null;
        }



        public async Task<string> UploadImageAsync(dynamic path, int MaxSize, IFormFile file)
        {
            if (file != null && file.Length > 0 && ImageExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("image"))
            {
                if (FileSizeCore.Mb(file.Length) > MaxSize)
                    throw new Exception("Big File!");

                try
                {
                    var savePath = path + $"{"/img/"}";
                    ImageName = RandCore.Mix() + Path.GetExtension(file.FileName);
                    string _path = Path.Combine(savePath, Path.GetFileName(ImageName));
                    using (var stream = File.Create(_path))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return ImageName;
                }
                catch (Exception error)
                {
                    throw new Exception(error.Message);
                }
            }
            return null;
        }




        //public string UploadZipFile(string dir, int MaxSize, HttpPostedFileBase file)
        //{
        //    string fileName = String.Empty;

        //    try
        //    {
        //        if (FileSizeCore.Mb(file.ContentLength) > MaxSize)
        //            throw new Exception("Big File!");

        //        if (file != null && file.ContentLength > 0 && ZipExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
        //        {
        //            fileName = RandCore.DateTimeTick() + Path.GetExtension(file.FileName);
        //            string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(fileName));
        //            file.SaveAs(path);
        //        }

        //        return fileName;
        //    }
        //    catch (Exception error)
        //    {
        //        throw new Exception(error.Message);
        //    }
        //}



        //public string UploadFile(string dir, int MaxSize, HttpPostedFileBase file)
        //{
        //    string fileName = String.Empty;

        //    try
        //    {
        //        if (FileSizeCore.Mb(file.ContentLength) > MaxSize)
        //            throw new Exception("Big File!");

        //        if (file != null && file.ContentLength > 0 && DocExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("application"))
        //        {
        //            fileName = RandCore.DateTimeTick() + Path.GetExtension(file.FileName);
        //            string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(fileName));
        //            file.SaveAs(path);
        //        }

        //        return fileName;
        //    }
        //    catch (Exception error)
        //    {
        //        throw new Exception(error.Message);
        //    }
        //}




        //public string UploadVideo(string dir, int MaxSize, HttpPostedFileBase file)
        //{
        //    string videoName = String.Empty;

        //    try
        //    {
        //        if (FileSizeCore.Mb(file.ContentLength) > MaxSize)
        //            throw new Exception("Big File!");

        //        if (file != null && file.ContentLength > 0 && VideoExtensions.Contains(Path.GetExtension(file.FileName).ToLower()) && file.ContentType.Contains("video"))
        //        {
        //            videoName = RandCore.Mix() + Path.GetExtension(file.FileName);
        //            string path = Path.Combine(HostingEnvironment.MapPath(dir), Path.GetFileName(videoName));
        //            file.SaveAs(path);
        //        }

        //        return videoName;
        //    }
        //    catch (Exception error)
        //    {
        //        throw new Exception(error.Message);
        //    }
        //}



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
        public async Task<string> ImageApi(IFormFile file, dynamic Path, string extension, int maxSize, int width, int height)
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
