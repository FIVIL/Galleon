using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Galleon.Principles
{
    public class BasePrinciples : IEquatable<BasePrinciples>
    {
        public byte Version { get; protected set; }
        public Guid ID { get; protected set; }
        public string VersionDecryption { get; protected set; }
        public string ShortTerms { get; protected set; }
        #region ctor
        /// <summary>
        /// base constructor.
        /// </summary>
        /// <param name="version">version</param>
        /// <param name="versionDecryption">version Decryption.</param>
        /// <param name="shortTerms">short Terms of version.</param>
        public BasePrinciples(byte version, string versionDecryption, string shortTerms)
        {
            ID = Guid.NewGuid();
            Version = version;
            VersionDecryption = versionDecryption;
            ShortTerms = shortTerms;
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
        public BasePrinciples(byte Version, Guid ID, string VersionDecryption, string ShortTerms)
        {
            this.Version = Version;
            this.ID = ID;
            this.VersionDecryption = VersionDecryption;
            this.ShortTerms = ShortTerms;
        }
        #endregion


        public override bool Equals(object obj)
        {
            return Equals(obj as BasePrinciples);
        }

        public bool Equals(BasePrinciples other)
        {
            return other != null &&
                   Version == other.Version &&
                   ID.Equals(other.ID) &&
                   VersionDecryption == other.VersionDecryption &&
                   ShortTerms == other.ShortTerms;
        }

        public override int GetHashCode() => Galleon.Util.StringUtilities.ToJson(this).GetHashCode();

        public static bool operator ==(BasePrinciples principles1, BasePrinciples principles2) => EqualityComparer<BasePrinciples>.Default.Equals(principles1, principles2);

        public static bool operator !=(BasePrinciples principles1, BasePrinciples principles2) => !(principles1 == principles2);
    }
}
