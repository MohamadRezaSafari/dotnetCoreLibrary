using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Providers
{
    public class HashHMACCore
    {
        private readonly static string Str = @"}ZL3OR||@s)vA4A}M=64~*rt{_v!-uB4GT.]k]zmsn0c\K-zm#";
        private static byte[] Key = null;

        static HashHMACCore()
        {
            Key = Encoding.ASCII.GetBytes(Str);
        }


        public static string Md5(string Txt, bool Replace)
        {
            byte[] _md5;
            using (HashAlgorithm md5 = new HMACMD5(Key))
            {
                _md5 = md5.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if (Replace == true)
                return BitConverter.ToString(_md5).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_md5);
        }


        public static string Sha1(string Txt, bool Replace)
        {
            byte[] _sha1;
            using (HashAlgorithm sha1 = new HMACSHA1(Key))
            {
                _sha1 = sha1.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if (Replace == true)
                return BitConverter.ToString(_sha1).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_sha1);
        }


        public static string Sha256(string Txt, bool Replace)
        {
            byte[] _sha256;
            using (HashAlgorithm sha256 = new HMACSHA256(Key))
            {
                _sha256 = sha256.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if (Replace == true)
                return BitConverter.ToString(_sha256).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_sha256);
        }


        public static string Sha384(string Txt, bool Replace)
        {
            byte[] _sha384;
            using (HashAlgorithm sha384 = new HMACSHA384(Key))
            {
                _sha384 = sha384.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if (Replace == true)
                return BitConverter.ToString(_sha384).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_sha384);
        }


        public static string Sha512(string Txt, bool Replace)
        {
            byte[] _sha512;
            using (HashAlgorithm sha512 = new HMACSHA512(Key))
            {
                _sha512 = sha512.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if (Replace == true)
                return BitConverter.ToString(_sha512).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_sha512);
        }
    }
}