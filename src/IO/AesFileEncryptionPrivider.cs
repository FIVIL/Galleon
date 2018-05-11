using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Galleon.Util;
namespace Galleon.IO
{
    class AesFileEncryptionPrivider
    {
        private static string EncryptionKey { get; set; } = null;
        public static void Create(string key)
        {
            if (EncryptionKey != null) return;
            EncryptionKey = key;
        }
        #region write
        private static void WriteFileASCII(string clearText, string FilePath)
        {
            byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
            WriteFile(clearBytes, FilePath);
        }
        private static void WriteFileBase64(string clearText, string FilePath)
        {
            byte[] clearBytes = Convert.FromBase64String(clearText);
            WriteFile(clearBytes, FilePath);
        }
        public static void WriteFile(string clearText, string filePath, StringEncoding encoding = StringEncoding.UTF8)
        {
            byte[] clearBytes = null;
            switch (encoding)
            {
                case StringEncoding.Base64:
                    clearBytes = Convert.FromBase64String(clearText);
                    break;
                case StringEncoding.UTF8:
                    clearBytes = Encoding.UTF8.GetBytes(clearText);
                    break;
                case StringEncoding.ASCII:
                    clearBytes = Encoding.ASCII.GetBytes(clearText);
                    break;
                default:
                    clearBytes = Encoding.UTF8.GetBytes(clearText);
                    break;
            }
            WriteFile(clearBytes, filePath);
        }
        public static void WriteFile(byte[] clearBytes, string filePath)
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey)) throw new Exception("No key");
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
                    System.IO.File.WriteAllBytes(filePath, ms.ToArray());
                }
            }

        }
        #endregion

        #region read
        public static string ReadFile(string filePath,StringEncoding encoding)
        {
            switch (encoding)
            {
                case StringEncoding.Base64:
                    return ReadFileBase64String(filePath);
                case StringEncoding.UTF8:
                    return Encoding.UTF8.GetString(ReadFile(filePath));
                case StringEncoding.ASCII:
                    return ReadFileASCIIString(filePath);
                default:
                    return Encoding.UTF8.GetString(ReadFile(filePath));
            }
        }
        private static string ReadFileASCIIString(string FilePath)
        {
            return Encoding.ASCII.GetString(ReadFile(FilePath));
        }
        private static string ReadFileBase64String(string FilePath)
        {
            return Convert.ToBase64String(ReadFile(FilePath));
        }
        public static byte[] ReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey)) throw new Exception("No key");
            //cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = System.IO.File.ReadAllBytes(filePath);
            //File.Delete(FilePath);
            byte[] RetValuel;
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
                    RetValuel = ms.ToArray();
                }
            }
            return RetValuel;
        }
        #endregion
    }
}
