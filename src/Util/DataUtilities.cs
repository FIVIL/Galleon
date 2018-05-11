using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Principles;
namespace Galleon.Util
{
    public static class DataUtilities
    {
        public static Random Rnd { get; private set; }
        public static Dictionary<string, BasePrinciples> Principles { get; private set; }
        static DataUtilities()
        {
            Rnd = new Random();
            Principles = new Dictionary<string, BasePrinciples>();
        }
    }
}
