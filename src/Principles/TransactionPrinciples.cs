using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Util;
namespace Galleon.Principles
{
    public class TransactionPrinciples:BasePrinciples
    {
        public double Max { get; private set; }
        public double Min { get; private set; }

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="version">version</param>
        /// <param name="versionDecryption">version Decryption.</param>
        /// <param name="shortTerms">short Terms of version.</param>
        /// <param name="max">maximum ammount allowed for Transaction.</param>
        /// <param name="min">minimum ammount allowed for Transaction.</param>
        public TransactionPrinciples(byte version, string versionDecryption, string shortTerms,double max,double min):
            base(version,versionDecryption,shortTerms)
        {
            Max = max;
            Min = min;
        }
        /// <summary>
        /// json constructor.
        /// Tip:only for json Deserialization.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="iD"></param>
        /// <param name="versionDecryption"></param>
        /// <param name="shortTerms"></param>
        [JsonConstructor]
        public TransactionPrinciples(byte Version, Guid ID, string VersionDecryption, string ShortTerms, double Max, double Min)
            : base(Version, ID, VersionDecryption, ShortTerms)
        {
            this.Max = Max;
            this.Min = Min;
        }
        public TransactionPrinciples(string json)
        {
            var tp = json.FromJson<TransactionPrinciples>();
            this.Version = tp.Version;
            this.ID = tp.ID;
            this.VersionDecryption = tp.VersionDecryption;
            this.ShortTerms = tp.ShortTerms;
            this.Max = tp.Max;
            this.Min = tp.Min;
        }
        #endregion
    }
}
