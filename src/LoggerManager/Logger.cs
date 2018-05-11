using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Galleon.Util;
namespace Galleon.LoggerManager
{
    public static class Logger
    {
        public static List<Log> Logs { get; set; }
        public static string LogFileName { get; set; }
        public static System.Numerics.BigInteger Count { get; set; }
        static Logger()
        {
            Logs = new List<Log>();
        }
        public static void Log(string data, string Class, string function = "")
        {
            if (Logs.Count >= 9999)
            {
                WriteLogs();
                Logs.Clear();
            }
            Logs.Add(new Log(Count++, data, Class, function));
        }
        public static void ReadLogs()
        {
            var logs = Galleon.IO.File.ReadAllText(DataUtilities.LogFilePath, LogFileName, IO.FileExtensions.log);
            Logs = logs.FromJson<List<Log>>();
        }
        public static void WriteLogs()
        {
            if (Logs.Count >= 9999)
            {
                Galleon.IO.File.WriteAllText(Logs.ToJson(), DataUtilities.LogFilePath, LogFileName, IO.FileExtensions.log);
                ++Count;
                LogFileName = "LogsFrom" + Count.ToString();
            }
            else
            {
                Galleon.IO.File.WriteAllText(Logs.ToJson(), DataUtilities.LogFilePath, LogFileName, IO.FileExtensions.log);
            }
        }
    }
}
