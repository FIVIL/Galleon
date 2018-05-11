using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Galleon.Crypto
{
    public class AesEncryptionProvider : IDisposable
    {
        public string Key { get; private set; }
        private static Random Rnd;
        #region ctor
        static AesEncryptionProvider()
        {
            Rnd = new Random();
        }
        /// <summary>
        /// create new instance with known key
        /// </summary>
        /// <param name="key">the key</param>
        public AesEncryptionProvider(string key)
        {
            Key = key;
        }
        /// <summary>
        /// create new instance with a random key
        /// </summary>
        public AesEncryptionProvider()
        {
            var KeyLength = Rnd.Next(13, 15);
            StringBuilder sb = new StringBuilder(((char)('a' + KeyLength)).ToString());
            for (int i = 0; i < KeyLength; i++)
            {
                var CS = Rnd.Next(3);
                if (CS == 0)
                {
                    sb.Append(value: ((char)Rnd.Next('a', 'z' + 1)).ToString());
                }
                else if (CS == 1)
                {
                    sb.Append(value: ((char)Rnd.Next('A', 'Z' + 1)).ToString());
                }
                else
                {
                    sb.Append(value: ((char)Rnd.Next('0', '9' + 1)).ToString());
                }
            }
            Key = sb.ToString();
        }
        #endregion

        #region encrypt
        public string EncryptASCII(string clearText)
        {
            return Encrypt(Encoding.ASCII.GetBytes(clearText));
        }
        public string Encrypt(string clearText)
        {
            return Encrypt(Encoding.UTF8.GetBytes(clearText));
        }
        public string Encrypt(byte[] clearBytes)
        {
            string ret = string.Empty;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    ret = Convert.ToBase64String(ms.ToArray());
                }
            }
            return ret;
        }
        #endregion

        #region decrypt
        public string DecrptyASCII(string cipherText)
        {
            return Encoding.ASCII.GetString(Decrypt(cipherText));
        }
        public string Decrpty(string cipherText)
        {
            return Encoding.UTF8.GetString(Decrypt(cipherText));
        }
        public byte[] Decrypt(string cipherText)
        {
            //cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            byte[] RetValuel;
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
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

        public void Dispose()
        {
            Rnd = null;
            Key = string.Empty;
        }
        #endregion
    }
}
