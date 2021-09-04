using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Providers
{
    public class RSACryptoSystemCore
    {
        private static RSAParameters PublicKey;
        private static RSAParameters PrivateKey;
        protected static string encrypt;
        protected static string decrypt;


        public static string[] Keys(int KeySize)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    PublicKey = rsa.ExportParameters(false);
                    PrivateKey = rsa.ExportParameters(true);
                };

                return new string[] {
                    string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                        Convert.ToBase64String(PublicKey.Modulus),
                        Convert.ToBase64String(PublicKey.Exponent))
                    ,
                    string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                        Convert.ToBase64String(PrivateKey.Modulus),
                        Convert.ToBase64String(PrivateKey.Exponent),
                        Convert.ToBase64String(PrivateKey.P),
                        Convert.ToBase64String(PrivateKey.Q),
                        Convert.ToBase64String(PrivateKey.DP),
                        Convert.ToBase64String(PrivateKey.DQ),
                        Convert.ToBase64String(PrivateKey.InverseQ),
                        Convert.ToBase64String(PrivateKey.D))
                };
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        // Use Encrypt 
        /*
         *  string[] test = RSACryptoSystem.Keys(2048);
            RSACryptoSystem.UseEncrypt("ali", test[0], 2048);
        */
        public static string UseEncrypt(string Txt, string PublicK, int KeySize)
        {
            byte[] encryptByte;

            encryptByte = Encoding.ASCII.GetBytes(Txt);
            encrypt = Convert.ToBase64String(Encrypt(encryptByte, PublicK, KeySize));

            return encrypt;
        }


        // Use Decrypt 
        /*
         *  string[] test = RSACryptoSystem.Keys(2048);
            RSACryptoSystem.UseDecrypt(TempData["enc"].ToString(), test[1], 2048);
        */
        public static string UseDecrypt(string Txt, string PrivateK, int KeySize)
        {
            byte[] decryptByte;
            byte[] dec;

            decryptByte = Convert.FromBase64String(Txt);
            dec = Decrypt(decryptByte, PrivateK, KeySize);
            decrypt = Encoding.UTF8.GetString(dec);

            return decrypt;
        }


        public static byte[] Encrypt(byte[] input, string _publicKey, int KeySize)
        {
            try
            {
                byte[] encrypted;
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    RSACryptoServiceProviderExtensions.FromXmlString(rsa, _publicKey);
                    encrypted = rsa.Encrypt(input, true);
                };
                return encrypted;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        public static byte[] Decrypt(byte[] input, string _privateKey, int KeySize)
        {
            try
            {
                byte[] decrypted;
                using (var rsa = new RSACryptoServiceProvider(KeySize))
                {
                    rsa.PersistKeyInCsp = false;
                    RSACryptoServiceProviderExtensions.FromXmlString(rsa, _privateKey);
                    decrypted = rsa.Decrypt(input, true);
                };
                return decrypted;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        /*static RSAProvider()
        {
            GenerateKeys();
        }


        private static void GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);

                //File.WriteAllText(HostingEnvironment.MapPath("~/Keys/" + "PrivateKey.xml"), rsa.ToXmlString(true)); 
                //File.WriteAllText(HostingEnvironment.MapPath("~/Keys/" + "PublicKey.xml"), rsa.ToXmlString(false));
            };
        }*/
        /*public static byte[] Encrypt(byte[] input)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(publicKey);
                encrypted = rsa.Encrypt(input, true);
            };
            return encrypted;
        }



        public static byte[] Decrypt(byte[] input)
        {
            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);
                decrypted = rsa.Decrypt(input, true);
            };
            return decrypted;
        }*/
    }



    public static class RSACryptoServiceProviderExtensions
    {
        public static void FromXmlString(this RSACryptoServiceProvider rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string ToXmlString(this RSACryptoServiceProvider rsa)
        {
            RSAParameters parameters = rsa.ExportParameters(true);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(parameters.Modulus),
                Convert.ToBase64String(parameters.Exponent),
                Convert.ToBase64String(parameters.P),
                Convert.ToBase64String(parameters.Q),
                Convert.ToBase64String(parameters.DP),
                Convert.ToBase64String(parameters.DQ),
                Convert.ToBase64String(parameters.InverseQ),
                Convert.ToBase64String(parameters.D));
        }
    }
}