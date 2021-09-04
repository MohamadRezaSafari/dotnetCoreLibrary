using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Providers
{
    public class FileEncryptionCore
    {
        //private readonly IHostingEnvironment _hostingEnvironment;

        //public HomeController(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        private readonly static string defaultKey = "werwerewrwer";
        private readonly static string _iv = "~^WF]7#$d^vmR[5n";
        private static byte[] Key;
        private static byte[] IV;
        private readonly static string[] ImageExtensions = new string[] { ".jpeg", ".jpg", ".png", ".gif" };
        private readonly static string[] DocExtensions = new string[] { ".pdf", ".doc", ".docx", ".pptx", ".ppt" };

        static FileEncryptionCore()
        {
            SHA256 mySHA256 = SHA256.Create();
            Key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(defaultKey));
            IV = Encoding.ASCII.GetBytes(_iv);
        }

        // Web Api AES Decrypt File
        /*
            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStream());
            NameValueCollection formData = provider.FormData;
            IList<HttpContent> files = provider.Files;
            string name = await FileEncryption.WebApiAesDecryptFile(files, "~/Upload/", formData["extension"], 3);
             
        */
        public static async Task<string> WebApiAesDecryptFile(IList<HttpContent> files, string Path, string Extension, int MaxSizeMB)
        {
            if (!ImageExtensions.Contains(Extension.ToLower()) && !DocExtensions.Contains(Extension.ToLower()))
            {
                throw new Exception("Invalid Extension");
            }
                
            string name = RandCore.Mix();
            string _path = Path + name + Extension;

            await Task.Run(async () =>
            {
                var provider = new AesCryptoServiceProvider();
                var decryptor = provider.CreateDecryptor(Key, IV);
                using (var destination = File.Create(_path))
                {
                    using (var cryptoStream = new CryptoStream(destination, decryptor, CryptoStreamMode.Write))
                    {
                        byte[] data = await files[0].ReadAsByteArrayAsync();

                        if (FileSizeCore.KB(data.Length) <= 0)
                        {
                            throw new Exception("File is empty");
                        }
                        if (FileSizeCore.Mb(data.Length) > MaxSizeMB)
                        {
                            throw new Exception("Maximum Size = " + MaxSizeMB + " mb");
                        }
                            
                        await cryptoStream.WriteAsync(data, 0, data.Length);
                    }
                }
            });

            return await Task.FromResult(name + Extension);
        }


        // AES Encrypt File
        /*
         *   FileEncryptionCore.AesEncryptFile(_hostingEnvironment.WebRootPath + "/images/upload/", "2.jpg");
        */
        public static async void AesEncryptFile(string Path, string Name)
        {
            string _path = Path;
            string FileName = _path + Name;
            string name = RandCore.Mix() + ".aes";

            await Task.Run(async () =>
            {
                var provider = new AesCryptoServiceProvider();
                var encryptor = provider.CreateEncryptor(Key, IV);
                using (var destination = File.Create(_path + name))
                {
                    using (var cryptoStream = new CryptoStream(destination, encryptor, CryptoStreamMode.Write))
                    {
                        var data = File.ReadAllBytes(FileName);
                        await cryptoStream.WriteAsync(data, 0, data.Length);
                    }
                }
            });
        }


        // AES Decrypt File
        /**
         *    FileEncryptionCore.AesDecryptFile(_hostingEnvironment.WebRootPath + "/images/upload/", "x2tizim0bck1988727349.aes", ".jpeg");
        */
        public static async void AesDecryptFile(string Path, string Name, string Extension)
        {
            string _path = Path;
            string FileName = _path + Name;
            string name = RandCore.Mix() + Extension;

            var provider = new AesCryptoServiceProvider();
            var decryptor = provider.CreateDecryptor(Key, IV);
            using (var destination = File.Create(_path + name))
            {
                using (var cryptoStream = new CryptoStream(destination, decryptor, CryptoStreamMode.Write))
                {
                    var data = File.ReadAllBytes(FileName);
                    await cryptoStream.WriteAsync(data, 0, data.Length);
                }
            }
        }


        // Rijndael Encrypt File
        /*
         *  FileEncryption.RijndaelEncryptFile(HostingEnvironment.MapPath("~/Upload/1.jpeg"), HostingEnvironment.MapPath("~/Upload/2.aes"));
        */
        public static void RijndaelEncryptFile(string inputFile, string outputFile)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        // Rijndael Decrypt File
        /*
         *  FileEncryption.RijndaelDecryptFile(HostingEnvironment.MapPath("~/Upload/2.aes"), HostingEnvironment.MapPath("~/Upload/3.jpeg"));
        */
        public static void RijndaelDecryptFile(string inputFile, string outputFile)
        {
            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(outputFile, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}