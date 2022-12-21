using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Providers
{
    public class EncryptionCore
    {
        private readonly static string EncryptionKey = "v5RLRL5Czm0BDsP86vwT";
        private static byte[] ByteKey;

        private static byte[] Key
        {
            get => ByteKey;
            set => ByteKey = Encoding.UTF8.GetBytes(EncryptionKey);
        }


        public static string ProtectedDataEncryption(string Text)
        {
            byte[] enc = ProtectedData.Protect(Encoding.UTF8.GetBytes(Text), Key, DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(enc);
        }

        // Encryption.ProtectedDataDecryption(x)
        public static string ProtectedDataDecryption(string Encrypted)
        {
            byte[] ByteEncrypted = Convert.FromBase64String(Encrypted.Replace(' ', '+'));
            byte[] dec = ProtectedData.Unprotect(ByteEncrypted, Key, DataProtectionScope.CurrentUser);

            return Encoding.UTF8.GetString(dec);
        }


        public string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        public string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}