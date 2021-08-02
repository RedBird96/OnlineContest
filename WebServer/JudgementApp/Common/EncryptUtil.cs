
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace JudgementApp.Common
{
    public  static class EncryptUtil
    {
        static string passPhrase = "Pas5pr@se";
        static string saltValue = "s@1tValue";
        static string hashAlgorithm = "SHA1";
        static int passwordIterations = 2;
        static string initVector = "@1B2c3D4e5F6g7H8";
        static int keySize = 256;
        public static string[] AccessURL;
       
      public static string[] GetAllUrl()
        {

            return AccessURL;
        }
       
        public static string EncryptString(string plainText)
        {

            byte[] initVectorBytes;
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes;
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] plainTextBytes;
            plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes password;
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes;
            keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey;
            symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor;
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream;
            memoryStream = new MemoryStream();

            CryptoStream cryptoStream;
            cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] cipherTextBytes;
            cipherTextBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            string cipherText;
            cipherText = Convert.ToBase64String(cipherTextBytes);


            return Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cipherText));
        }

        public static string DecryptString(string cipherText)
        {
            cipherText = Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(cipherText));
            byte[] initVectorBytes;
            initVectorBytes = Encoding.ASCII.GetBytes(initVector);

            byte[] saltValueBytes;
            saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            byte[] cipherTextBytes;
            cipherTextBytes = Convert.FromBase64String(cipherText);

            PasswordDeriveBytes password;
            password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

            byte[] keyBytes;
            keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey;
            symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor;
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            MemoryStream memoryStream;
            memoryStream = new MemoryStream(cipherTextBytes);

            CryptoStream cryptoStream;
            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] plainTextBytes;
            plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount;
            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText;
            plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

            return plainText;
        }


        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}