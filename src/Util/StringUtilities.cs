using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Blake2Sharp;
using Newtonsoft.Json;
using Galleon.Principles;
namespace Galleon.Util
{
    public static class StringUtilities
    {
        #region Hash
        public static string GetHashString(this string data, HashAlgorithms hash)
        {
            switch (hash)
            {
                case HashAlgorithms.SHA256:
                    return Sha256(data);
                case HashAlgorithms.SHA512:
                    return Sha512(data);
                case HashAlgorithms.Blake2b:
                    return Blake2b(data);
                case HashAlgorithms.DoubleBlake2b:
                    return DoubleBlake2b(data);
                default:
                    return DoubleBlake2b(data);
            }
        }
        private static string DoubleBlake2b(string data)
        {
            return Convert.ToBase64String(Blake2B.ComputeHash(Blake2B.ComputeHash(Encoding.UTF8.GetBytes(data))));
        }
        private static string Blake2b(string data)
        {
            return Convert.ToBase64String(Blake2B.ComputeHash(Encoding.UTF8.GetBytes(data)));
        }
        private static string Sha512(string data)
        {
            var bytedata = Encoding.UTF8.GetBytes(data);
            byte[] hash;
            using (var hasher = SHA512.Create())
            {
                hash = hasher.ComputeHash(bytedata);
            }
            return Convert.ToBase64String(hash);
        }
        private static string Sha256(string data)
        {
            var bytedata = Encoding.UTF8.GetBytes(data);
            byte[] hash;
            using (var hasher = SHA256.Create())
            {
                hash = hasher.ComputeHash(bytedata);
            }
            return Convert.ToBase64String(hash);
        }
        #endregion

        #region Json
        public static string ToJson(this object obj, Newtonsoft.Json.Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        #endregion

        #region String Formating
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        public static string ToUTF8String(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        public static string ToASCIIString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static byte[] FromBase64String(this string text)
        {
            return Convert.FromBase64String(text);
        }
        public static byte[] FromUTF8String(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
        public static byte[] FromASCIIString(this string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }
        #endregion

        #region Principles
        public static string GetPrinciplesKey(this byte version, PrinciplesType type)
        {
            return type + ":" + version.ToString();
        }
        #endregion
    }
}
