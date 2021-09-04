using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Providers
{
    public class AesCore
    {              
        private readonly string Str = @"DGkLYJemh[$e<C\#OM{M`xY.b3W4}V_F'MZhxVHTZLRnweP4}9q8]-t52hZE+E*W";
        private readonly string _iv = "~^WF]7#$d^vmR[5n";
        private byte[] Key;
        private byte[] IV;
        Aes encryptor = Aes.Create();

        public AesCore()
        {
            SHA256 mySHA256 = SHA256.Create();
            Key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(Str));
            IV = Encoding.ASCII.GetBytes(_iv);
        }

        public string EncryptString(string plainText)
        {
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = Key;
            encryptor.IV = IV;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);
            return cipherText;
        }



        public string DecryptString(string cipherText)
        {
            encryptor.Mode = CipherMode.CBC;
            encryptor.Key = Key;
            encryptor.IV = IV;

            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);
            string plainText = String.Empty;
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] plainBytes = memoryStream.ToArray();
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }
            return plainText;
        }
    }
}