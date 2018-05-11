using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Galleon.IO
{
    public static class File
    {
        #region byte[]

        #region read
        public static byte[] ReadAllBytes(string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            return System.IO.File.ReadAllBytes(path);
        }
        public static byte[] ReadAllBytes(string fileName, FileExtensions extension)
        {
            return ReadAllBytes("", fileName, extension);
        }
        public static async Task<byte[]> ReadAllBytesAsync(string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            return await Task.Run(() => System.IO.File.ReadAllBytes(path));
        }
        public static async Task<byte[]> ReadAllBytesAsync(string fileName, FileExtensions extension)
        {
            return await ReadAllBytesAsync("", fileName, extension);
        }
        #endregion

        #region write
        public static void WriteAllBytes(this byte[] file, string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            System.IO.File.WriteAllBytes(path, file);
        }
        public static void WriteAllBytes(this byte[] file, string fileName, FileExtensions extension)
        {
            WriteAllBytes(file, "", fileName, extension);
        }
        public static async Task WriteAllBytesAsync(this byte[] file, string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            await Task.Run(() => System.IO.File.WriteAllBytes(path, file));
        }
        public static async Task WriteAllBytesAsync(this byte[] file, string fileName, FileExtensions extension)
        {
            await WriteAllBytesAsync(file, "", fileName, extension);
        }
        #endregion

        #endregion

        #region text

        #region read
        public static string ReadAllText(string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            return System.IO.File.ReadAllText(path);
        }
        public static string ReadAllText(string fileName, FileExtensions extension)
        {
            return ReadAllText("", fileName, extension);
        }
        public static async Task<string> ReadAllTextAsync(string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            return await Task.Run(() => System.IO.File.ReadAllText(path));
        }
        public static async Task<string> ReadAllTextAsync(string fileName, FileExtensions extension)
        {
            return await ReadAllTextAsync("", fileName, extension);
        }
        #endregion

        #region write
        public static void WriteAllText(this string file, string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            System.IO.File.WriteAllText(path, file);
        }
        public static void WriteAllText(this string file, string fileName, FileExtensions extension)
        {
            WriteAllText(file, "", fileName, extension);
        }
        public static async Task WriteAllTextAsync(this string file, string filePath, string fileName, FileExtensions extension)
        {
            string path = ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
            await Task.Run(() => System.IO.File.WriteAllText(path, file));
        }
        public static async Task WriteAllTextAsync(this string file, string fileName, FileExtensions extension)
        {
            await WriteAllTextAsync(file, "", fileName, extension);
        }
        #endregion

        #endregion

        #region pathmaker
        public static string PathMaker(this string fileName, string filePath, FileExtensions extension)
        {
            return ((string.IsNullOrEmpty(filePath) == false) ? (filePath + "\\") : "") + fileName + "." + extension.ToString();
        }
        public static string PathMaker(this string fileName, FileExtensions extension)
        {
            return fileName.PathMaker("", extension);
        }

        #endregion
    }
}
