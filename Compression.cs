using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;

namespace Providers
{
    public class Compression
    {
        private static string ImagePath = "~/Upload/IMG/";
        private static string ImageFolderPath = "~/Upload/IMG/";
        private static int imageQuality;
        private static string _name;

        // string name = await Compression.VariousQualityFromStream(file, _hostingEnvironment, "low", null, 3);
        public static async Task<string> VariousQualityFromStream(IFormFile file, IHostingEnvironment hostingEnvironment, string quality, string folder, int maxSize)
        {
            if (FileSizeCore.Mb(file.Length) >= maxSize)
                throw new Exception("Big File!");

            Image original = Image.FromStream(file.OpenReadStream(), true, true);
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                string originalFileName = RandCore.Mix();

                switch (quality)
                {
                    case "low":
                        imageQuality = 10;
                        break;
                    case "medium":
                        imageQuality = 50;
                        break;
                    case "high":
                        imageQuality = 100;
                        break;
                    default:
                        imageQuality = 70;
                        break;
                }

                await Task.Run(async () =>
                {
                    EncoderParameter encoderParameter = new EncoderParameter(encoder, imageQuality);
                    encoderParameters.Param[0] = encoderParameter;

                    _name = folder + "-" + originalFileName + ".jpeg";
                    string fileOut = Path.Combine(hostingEnvironment.WebRootPath + $"{"/images/upload/"}", _name);
                    FileStream ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                    original.Save(ms, jpgEncoder, encoderParameters);
                    await ms.FlushAsync();
                    ms.Close();
                });    
            }
            return _name;
        }


        /*
         *   Compression.VariousQuality("e6418d2c-d816-4b85-ac54-f5b2a0a3bace.jpeg", "low");
         */
        public static string VariousQuality(string fileName, string quality)
        {
            if (!Directory.Exists(ImageFolderPath))
                Directory.CreateDirectory(ImageFolderPath);

            string file = Path.Combine(ImagePath + fileName);
            if (!File.Exists(file))
                throw new Exception("File not found");

            Image original = Image.FromFile(file);
            ImageCodecInfo jpgEncoder = null;
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    jpgEncoder = codec;
                    break;
                }
            }
            if (jpgEncoder != null)
            {
                Encoder encoder = Encoder.Quality;
                EncoderParameters encoderParameters = new EncoderParameters(1);
                string originalFileName = Path.GetFileNameWithoutExtension(file);


                switch (quality)
                {
                    case "low":
                        imageQuality = 10;
                        break;
                    case "medium":
                        imageQuality = 50;
                        break;
                    case "high":
                        imageQuality = 100;
                        break;
                    default:
                        imageQuality = 70;
                        break;
                }


                EncoderParameter encoderParameter = new EncoderParameter(encoder, imageQuality);
                encoderParameters.Param[0] = encoderParameter;

                _name = originalFileName + "__quality(" + imageQuality + ").jpeg";
                string fileOut = Path.Combine(ImagePath, _name);
                FileStream ms = new FileStream(fileOut, FileMode.Create, FileAccess.Write);
                original.Save(ms, jpgEncoder, encoderParameters);
                ms.Flush();
                ms.Close();                
            }
            return _name;
        }
    }
}