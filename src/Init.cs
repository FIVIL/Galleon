using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Db;
using Galleon.Util;
namespace Galleon
{
    public static class Init
    {
        public static Config Config { get; private set; }
        public static bool Initialized { get; private set; }
        public const string configdat = "config";
        static Init()
        {
            Initialized = false;
        }
        public static void Initizer(string PassWord, Func<string> getPassWord)
        {
            if (Initialized) return;
            Initialized = true;
            Galleon.IO.AesFileEncryptionPrivider.Create(PassWord);
            if (System.IO.File.Exists(Galleon.IO.File.PathMaker(configdat, IO.FileExtensions.dat))) InitizerLogin(PassWord);
            else InitizerFirstLogin(getPassWord);
        }
        private static void InitizerFirstLogin(Func<string> getPassWord)
        {
            Config cf = new Config();
            cf.PassWord = getPassWord();
            cf.FilePathRoot = "";
            var cfj = cf.ToJson(Newtonsoft.Json.Formatting.Indented);
            Galleon.IO.File.WriteAllText(cfj, configdat, IO.FileExtensions.sec);
            System.IO.Directory.CreateDirectory("Db");
            System.IO.Directory.CreateDirectory(@"Db\blocks");
            System.IO.Directory.CreateDirectory(@"Db\transactions");
            System.IO.Directory.CreateDirectory(@"Db\private");
            System.IO.Directory.CreateDirectory(@"logs");
            Config = cf;
        }
        private static void InitizerLogin(string PassWord)
        {
            Config = Galleon.IO.File.ReadAllText(configdat, IO.FileExtensions.sec).FromJson<Config>();
        }
    }
}
