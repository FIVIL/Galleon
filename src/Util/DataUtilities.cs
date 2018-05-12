using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Principles;
namespace Galleon.Util
{
    public static class DataUtilities
    {
        public static Random Rnd { get; private set; }
        [Obsolete("only for testing phase, use Db and networking system instead.")]
        public static Dictionary<string, BasePrinciples> Principles { get; private set; }
        [Obsolete("only for testing phase, use Db and networking system instead.")]
        public static byte Version { get; private set; }
        [Obsolete("only for testing phase, use IO system instead.")]
        static DataUtilities()
        {
            Rnd = new Random();
            Principles = new Dictionary<string, BasePrinciples>();
            Version = 1;
        }
    }
}
