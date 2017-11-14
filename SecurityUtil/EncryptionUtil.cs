using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SecurityUtil.Keys;

namespace SecurityUtil
{
    public static class EncryptionUtil
    {
        public static byte[] HashFile(byte[] fileData)
        {
            byte[] output = null;
            try
            {
                var alg = SHA512.Create();
                output = alg.ComputeHash(fileData);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return output;
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] output = null;
            try
            {
                output = File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return output;
        }

        public static string SignFile(byte[] fileData, string xmlPrivateKey)
        {
            string output = null;
            try
            {
                var alg = new RSACryptoServiceProvider();
                alg.FromXmlString(xmlPrivateKey);

                var hashedData = HashFile(fileData);

                var signedHash = alg.SignHash(hashedData, "SHA512");

                output = Convert.ToBase64String(signedHash);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }

            return output;
        }

        public static bool VerifyFile(byte[] fileData, string xmlPublicKey, string signature)
        {
            bool output = false;
            try
            {
                var alg = new RSACryptoServiceProvider();
                alg.FromXmlString(xmlPublicKey);

                var hashedData = HashFile(fileData);

                output = alg.VerifyHash(hashedData, "SHA512", Convert.FromBase64String(signature));
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return output;
        }

        public static void WriteBytesToFile(string filePath, byte[] fileData)
        {
            try
            {
                File.WriteAllBytes(filePath, fileData);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
        }

        #region Asymmetric Encryption

        public static byte[] Decrypt(byte[] data, string privateKey)
        {
            byte[] output = null;
            try
            {
                var myAlg = new RSACryptoServiceProvider();
                myAlg.FromXmlString(privateKey);
                output = myAlg.Decrypt(data, true);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return output;
        }

        public static string Encrypt(byte[] data, string publicKey)
        {
            byte[] output = null;
            try
            {
                var myAlg = new RSACryptoServiceProvider();
                myAlg.FromXmlString(publicKey);
                output = myAlg.Encrypt(data, true);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return Convert.ToBase64String(output);
        }

        public static AsymmetricKeys GenerateAsymmetricKeys()
        {
            AsymmetricKeys keys = null;
            try
            {
                var myAlg = new RSACryptoServiceProvider();
                keys = new AsymmetricKeys
                {
                    PublicKey = myAlg.ToXmlString(false),
                    PrivateKey = myAlg.ToXmlString(true)
                };
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }
            return keys;
        }

        #endregion Asymmetric Encryption

        #region Symmetric Encryption

        public static string Decrypt(SymmetricParameters p, string encryptedQueryString)
        {
            try
            {
                var myalg = Rijndael.Create();

                var msOut = new MemoryStream();
                var myEncryptingStream = new CryptoStream(msOut, myalg.CreateDecryptor(p.SecretKey, p.IV),
                    CryptoStreamMode.Write);

                byte[] encryptedQueryBytes = Convert.FromBase64String(encryptedQueryString);

                var msIn = new MemoryStream(encryptedQueryBytes) { Position = 0 };
                msIn.CopyTo(myEncryptingStream);
                myEncryptingStream.Flush();
                myEncryptingStream.FlushFinalBlock();

                msOut.Position = 0;
                byte[] queryBytes = new byte[msOut.Length];
                msOut.Read(queryBytes, 0, encryptedQueryBytes.Length);

                return Convert.ToBase64String(queryBytes);
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }

            return encryptedQueryString;
        }

        public static byte[] DecryptFile(SymmetricParameters p, byte[] fileData)
        {
            var myalg = Rijndael.Create();

            var msOut = new MemoryStream();
            var myEncryptingStream = new CryptoStream(msOut, myalg.CreateDecryptor(p.SecretKey, p.IV),
                CryptoStreamMode.Write);

            var msIn = new MemoryStream(fileData) { Position = 0 };
            msIn.CopyTo(myEncryptingStream);
            myEncryptingStream.Flush();
            myEncryptingStream.FlushFinalBlock();

            msOut.Position = 0;
            return msOut.ToArray();
        }

        public static string Encrypt(SymmetricParameters p, string queryString)
        {
            var myalg = Rijndael.Create();

            var msOut = new MemoryStream();
            var myEncryptingStream = new CryptoStream(msOut, myalg.CreateEncryptor(p.SecretKey, p.IV),
                CryptoStreamMode.Write);

            byte[] queryBytes = Convert.FromBase64String(queryString);

            var msIn = new MemoryStream(queryBytes) { Position = 0 };
            msIn.CopyTo(myEncryptingStream);
            myEncryptingStream.Flush();
            myEncryptingStream.FlushFinalBlock();

            msOut.Position = 0;
            byte[] encryptedQueryBytes = new byte[msOut.Length];
            msOut.Read(encryptedQueryBytes, 0, encryptedQueryBytes.Length);

            return Convert.ToBase64String(encryptedQueryBytes);
        }

        public static byte[] EncryptFile(SymmetricParameters p, byte[] fileData)
        {
            MemoryStream msOut = null;
            try
            {
                var myalg = Rijndael.Create();

                msOut = new MemoryStream();
                var cryptoStream = new CryptoStream(msOut, myalg.CreateEncryptor(p.SecretKey, p.IV),
                    CryptoStreamMode.Write);

                var msIn = new MemoryStream(fileData) { Position = 0 };
                msIn.CopyTo(cryptoStream);
                cryptoStream.Flush();
                cryptoStream.FlushFinalBlock();

                msOut.Position = 0;
            }
            catch (Exception ex)
            {
                FileLog.Log(ex);
            }

            return msOut.ToArray();
        }

        public static string GenerateHashKey(HttpRequestBase request)
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(request.Browser.Browser);
            myStr.Append(request.Browser.Platform);
            myStr.Append(request.Browser.MajorVersion);
            myStr.Append(request.Browser.MinorVersion);
            var sha = new SHA512CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            return Convert.ToBase64String(hashdata);
        }

        public static SymmetricParameters GenerateSymmetricParameters()
        {
            var myalg = Rijndael.Create();
            myalg.GenerateIV();
            myalg.GenerateKey();
            var ps = new SymmetricParameters
            {
                IV = myalg.IV,
                SecretKey = myalg.Key
            };
            return ps;
        }

        public static SymmetricParameters GenerateSymmetricParameters(string password, string salt)
        {
            var myalg = Rijndael.Create();

            var mygenerator = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt));
            var ps = new SymmetricParameters
            {
                SecretKey = mygenerator.GetBytes(myalg.KeySize / 8),
                IV = mygenerator.GetBytes(myalg.BlockSize / 8)
            };
            return ps;
        }

        #endregion Symmetric Encryption
    }
}