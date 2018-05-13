using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Util;
namespace Galleon.ContractManager.TransactionManager
{
    public class TransactionOutput : IEquatable<TransactionOutput>
    {
        public Guid ID { get; private set; }
        public string HashString { get; private set; }
        public string Reciepient { get; private set; }
        public double Amount { get; private set; }
        public bool IsProcessing { get; set; }
        public string ContainerTransactionHash { get; set; }
        public string ContainerBlockHash { get; set; }
        public string ParentTransactionHash { get; private set; }
        public DateTime IssuingTime { get; private set; }
        public TransactionOutput(string Reciepient, double Amount, string ParentTransactionHash)
        {
            this.Reciepient = Reciepient;
            this.Amount = Amount;
            this.ParentTransactionHash = ParentTransactionHash;
            ID = Guid.NewGuid();
            HashString = (Reciepient + Amount.ToString() + ParentTransactionHash + ID.ToString()).GetHashString(HashAlgorithms.SHA256);
            IsProcessing = false;
            IssuingTime = DateTime.Now;
        }
        [JsonConstructor]
        public TransactionOutput(Guid ID,string HashString,string Reciepient, double Amount, string ParentTransactionHash, bool IsProcessing,
            string ContainerTransactionHash, string ContainerBlockHash, DateTime IssuingTime)
        {
            this.ID = ID;
            this.HashString = HashString;
            this.Reciepient = Reciepient;
            this.Amount = Amount;
            this.ParentTransactionHash = ParentTransactionHash;
            this.IsProcessing = IsProcessing;
            this.ContainerTransactionHash = ContainerTransactionHash;
            this.ContainerBlockHash = ContainerBlockHash;
            this.IssuingTime = IssuingTime;
        }
        public bool IsMine(string Key)
        {
            return (Key == Reciepient) && !IsProcessing;
        }

        #region equality
        public override bool Equals(object obj)
        {
            return Equals(obj as TransactionOutput);
        }

        public bool Equals(TransactionOutput other)
        {
            return other != null &&
                   ID.Equals(other.ID) &&
                   Reciepient == other.Reciepient &&
                   Amount == other.Amount &&
                   IsProcessing == other.IsProcessing &&
                   ContainerTransactionHash == other.ContainerTransactionHash &&
                   ContainerBlockHash == other.ContainerBlockHash &&
                   ParentTransactionHash == other.ParentTransactionHash &&
                   IssuingTime == other.IssuingTime;
        }
        public override int GetHashCode() => Galleon.Util.StringUtilities.ToJson(this).GetHashCode();
        public static bool operator ==(TransactionOutput output1, TransactionOutput output2) => EqualityComparer<TransactionOutput>.Default.Equals(output1, output2);

        public static bool operator !=(TransactionOutput output1, TransactionOutput output2) => !(output1 == output2);
        #endregion

    }
}
