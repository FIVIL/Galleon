using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
namespace Galleon.LoggerManager
{
    public class Log
    {
        public Guid ID { get; private set; }
        public System.Numerics.BigInteger Count { get; private set; }
        public string Data { get; private set; }
        public DateTime Time { get; private set; }
        public string LoggerClass { get; private set; }
        public string LoggerFunction { get; private set; }

        public Log(BigInteger count, string data, string loggerClass, string loggerFunction)
        {
            ID = Guid.NewGuid();
            Count = count;
            Data = data;
            LoggerClass = loggerClass;
            LoggerFunction = loggerFunction;
            Time = DateTime.Now;
        }
    }
}
