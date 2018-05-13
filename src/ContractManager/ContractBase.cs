using Galleon.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Galleon.ContractManager
{
    public class ContractBase : IEquatable<ContractBase>
    {
        public Guid ID { get; protected set; }
        public string ContractName { get; protected set; }
        public string ContractHash { get; protected set; }
        public string ContractIssuer { get; protected set; }
        [JsonIgnore]
        protected Signture _signture { get; set; }
        public string Signture { get; protected set; }
        public DateTime IssuanceTime { get; protected set; }
        public DateTime MinedTime { get; set; }
        public string ContainerBlockHash { get; set; }
        public uint BlockNumber { get; set; }
        #region ctor
        [JsonConstructor]
        public ContractBase(Guid ID, string ContractName, string ContractHash, string ContractIssuer, string Signture, 
            DateTime IssuanceTime, DateTime MinedTime, string ContainerBlockHash, uint BlockNumber)
        {
            this.ID = ID;
            this.ContractName = ContractName;
            this.ContractHash = ContractHash;
            this.ContractIssuer = ContractIssuer;
            this.Signture = Signture;
            this.IssuanceTime = IssuanceTime;
            this.MinedTime = MinedTime;
            this.ContainerBlockHash = ContainerBlockHash;
            this.BlockNumber = BlockNumber;
            _signture = new Signture(Signture);
        }

        public ContractBase(string contractName, string contractIssuer)
        {
            ID = Guid.NewGuid();
            ContractName = contractName;
            ContractIssuer = contractIssuer;
            IssuanceTime = DateTime.Now;
        }
        #endregion
        public override bool Equals(object obj)
        {
            return Equals(obj as ContractBase);
        }

        public bool Equals(ContractBase other)
        {
            return other != null &&
                   ID.Equals(other.ID) &&
                   ContractName == other.ContractName &&
                   ContractIssuer == other.ContractIssuer &&
                   Signture == other.Signture &&
                   IssuanceTime == other.IssuanceTime &&
                   MinedTime == other.MinedTime &&
                   ContainerBlockHash == other.ContainerBlockHash &&
                   BlockNumber == other.BlockNumber;
        }
        public static bool operator ==(ContractBase base1, ContractBase base2)
        {
            return EqualityComparer<ContractBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(ContractBase base1, ContractBase base2)
        {
            return !(base1 == base2);
        }
    }
}
