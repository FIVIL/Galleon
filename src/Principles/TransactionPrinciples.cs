using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Util;
namespace Galleon.Principles
{
    public class TransactionPrinciples:BasePrinciples, IEquatable<TransactionPrinciples>
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

        #endregion
        public override bool Equals(object obj)
        {
            return Equals(obj as TransactionPrinciples);
        }

        public bool Equals(TransactionPrinciples other)
        {
            return other != null &&
                   base.Equals(other) &&
                   Max == other.Max &&
                   Min == other.Min;
        }
        public override int GetHashCode() => Galleon.Util.StringUtilities.ToJson(this).GetHashCode();
        public static bool operator ==(TransactionPrinciples principles1, TransactionPrinciples principles2) => EqualityComparer<TransactionPrinciples>.Default.Equals(principles1, principles2);

        public static bool operator !=(TransactionPrinciples principles1, TransactionPrinciples principles2) => !(principles1 == principles2);

    }
}
