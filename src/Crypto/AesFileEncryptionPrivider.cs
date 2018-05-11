using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Galleon.Crypto
{
    public static class AesFileEncryptionPrivider
    {
        private static string EncryptionKey { get; set; } = null;
        public static void Create(string key)
        {
            if (EncryptionKey != null) return;
            EncryptionKey = key;
        }
        #region write
        public static void WriteFileASCII(string clearText, string FilePath)
        {
            byte[] clearBytes = Encoding.ASCII.GetBytes(clearText);
            WriteFile(clearBytes, FilePath);
        }
        public static void WriteFileBase64(string clearText, string FilePath)
        {
            byte[] clearBytes = Convert.FromBase64String(clearText);
            WriteFile(clearBytes, FilePath);
        }
        public static void WriteFile(string clearText, string FilePath)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            WriteFile(clearBytes, FilePath);
        }
        public static void WriteFile(byte[] clearBytes, string FilePath)
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
                    File.WriteAllBytes(FilePath, ms.ToArray());
                }
            }

        }
        #endregion

        #region read
        public static string ReadFileString(string FilePath)
        {
            return Encoding.UTF8.GetString(ReadFile(FilePath));
        }
        public static string ReadFileASCIIString(string FilePath)
        {
            return Encoding.ASCII.GetString(ReadFile(FilePath));
        }
        public static string ReadFileBase64String(string FilePath)
        {
            return Convert.ToBase64String(ReadFile(FilePath));
        }
        public static byte[] ReadFile(string FilePath)
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey)) throw new Exception("No key");
            //cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = File.ReadAllBytes(FilePath);
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
