using System;
using System.Security.Cryptography;
using System.Text;

namespace Providers
{
    public class HashCore
    {
        public static string Md5(string Txt, bool Replace)
        {
            byte[] _md5;
            using(HashAlgorithm md5 = MD5.Create())
            {
                _md5 = md5.ComputeHash(Encoding.ASCII.GetBytes(Txt));
            }
            if(Replace == true)
                return BitConverter.ToString(_md5).Replace("-", String.Empty).ToLower();
            else
                return BitConverter.ToString(_md5);
        }


        public static string Sha1(string Txt, bool Replace)
        {
            byte[] _sha1;
            using (HashAlgorithm sha1 = SHA1.Create())
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
            using (HashAlgorithm sha256 = SHA256.Create())
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
            using (HashAlgorithm sha384 = SHA384.Create())
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
            using (HashAlgorithm sha512 = SHA512.Create())
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