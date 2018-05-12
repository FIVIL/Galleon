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
        public static void WriteFile(string clearText, string filePath, StringEncoding encoding = StringEncoding.UTF8)
        {
            var clearBytes = clearText.FromString(encoding);
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
        public static string ReadFile(string filePath, StringEncoding encoding)
        {
            return ReadFile(filePath).ToString(encoding);
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
