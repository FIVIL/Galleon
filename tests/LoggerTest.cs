using System;
using System.Collections.Generic;
using System.Text;
using Galleon.LoggerManager;
using Xunit;
namespace Galleon.tests
{
    public class LoggerTest
    {
        [Fact]
        public void LoggerTester()
        {
            Logger.Count = new System.Numerics.BigInteger();
            Logger.LogFileName = "Logs0";
            Logger.Log("a", "test");
            Logger.WriteLogs();
        }
    }
}
