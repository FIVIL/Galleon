using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Util;
namespace Galleon.Transaction
{
    public class TransactionOutput
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
        //[JsonConstructor]
        //public TransactionOutput(Guid ID, string Reciepient, double Amount, string ParentTransactionHash,bool IsProcessing,
        //    string ContainerTransactionHash,string ContainerBlockHash,DateTime IssuingTime)
        //{
        //    this.ID = ID;
        //    this.Reciepient = Reciepient;
        //    this.Amount = Amount;
        //    this.IsProcessing = IsProcessing;
        //    this.ContainerTransactionHash=cont
        //}
        public bool IsMine(string Key)
        {
            return (Key == Reciepient) && !IsProcessing;
        }
    }
}
