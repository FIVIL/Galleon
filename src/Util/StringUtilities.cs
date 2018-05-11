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
        public static string ToString(this byte[] bytes, StringEncoding encoding)
        {
            switch (encoding)
            {
                case StringEncoding.Base64:
                    return ToBase64String(bytes);
                case StringEncoding.UTF8:
                    return ToUTF8String(bytes);
                case StringEncoding.ASCII:
                    return ToASCIIString(bytes);
                default:
                    return ToBase64String(bytes);
            }
        }
        private static string ToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        private static string ToUTF8String(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        private static string ToASCIIString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
        public static byte[] FromString(this string text, StringEncoding encoding)
        {
            switch (encoding)
            {
                case StringEncoding.Base64:
                    return FromBase64String(text);
                case StringEncoding.UTF8:
                    return FromUTF8String(text);
                case StringEncoding.ASCII:
                    return FromASCIIString(text);
                default:
                    return FromBase64String(text);
            }
        }
        private static byte[] FromBase64String(string text)
        {
            return Convert.FromBase64String(text);
        }
        private static byte[] FromUTF8String(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
        private static byte[] FromASCIIString(string text)
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
