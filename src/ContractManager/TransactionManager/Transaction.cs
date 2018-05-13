using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Crypto;
using Newtonsoft.Json;
using Galleon.Principles;
using Galleon.Util;
namespace Galleon.ContractManager.TransactionManager
{
    public class Transaction : ContractBase, IPrinciples, IContract
    {
        public byte TransactionVersion { get; set; }
        public TransactionPrinciples TransactionPrinciple { get; set; }
        public string TransactionIssuer { get; protected set; }
        public string Reciepient { get; protected set; }
        public double Amount { get; set; }
        public uint Sequence { get; protected set; }
        public bool IsBlockReward { get; set; }
        public List<TransactionInput> TransactionInputs { get; protected set; }
        public List<TransactionOutput> TransactionOutputs { get; protected set; }

        [JsonIgnore]
        public double InputsBalance
        {
            get
            {
                double retValue = 0;
                foreach (var item in TransactionInputs)
                {
                    if (item.UTXO != null) retValue += item.UTXO.Amount;
                }
                return retValue;
            }
        }
        [JsonIgnore]
        public double OutputsBalance
        {
            get
            {
                double retValue = 0;
                foreach (var item in TransactionOutputs)
                {
                    retValue += item.Amount;
                }
                return retValue;
            }
        }
        #region ctor
        /// <summary>
        /// json constructon.
        /// only and only for json deserialization!!!
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractHash"></param>
        /// <param name="ContractIssuer"></param>
        /// <param name="Signture"></param>
        /// <param name="IssuanceTime"></param>
        /// <param name="MinedTime"></param>
        /// <param name="ContainerBlockHash"></param>
        /// <param name="BlockNumber"></param>
        /// <param name="TransactionVersion"></param>
        /// <param name="TransactionPrinciple"></param>
        /// <param name="TransactionIssuer"></param>
        /// <param name="Reciepient"></param>
        /// <param name="Amount"></param>
        /// <param name="Sequence"></param>
        /// <param name="IsBlockReward"></param>
        /// <param name="TransactionInputs"></param>
        /// <param name="TransactionOutputs"></param>
        [JsonConstructor]
        public Transaction(Guid ID, string ContractName, string ContractHash, string ContractIssuer, string Signture,
            DateTime IssuanceTime, DateTime MinedTime, string ContainerBlockHash, uint BlockNumber,
            byte TransactionVersion, TransactionPrinciples TransactionPrinciple, string TransactionIssuer, string Reciepient, double Amount, uint Sequence, bool IsBlockReward, List<TransactionInput> TransactionInputs,
            List<TransactionOutput> TransactionOutputs) :
            base(ID, ContractName, ContractHash, ContractIssuer, Signture, IssuanceTime, MinedTime, ContainerBlockHash, BlockNumber)
        {
            this.TransactionVersion = TransactionVersion;
            this.TransactionPrinciple = TransactionPrinciple;
            this.TransactionIssuer = TransactionIssuer;
            this.Reciepient = Reciepient;
            this.Amount = Amount;
            this.Sequence = Sequence;
            this.IsBlockReward = IsBlockReward;
            this.TransactionInputs = TransactionInputs;
            this.TransactionOutputs = TransactionOutputs;
        }
        /// <summary>
        /// for issuing new transaction in wallet
        /// </summary>
        /// <param name="name">transaction name</param>
        /// <param name="version">transaction version</param>
        /// <param name="issuer">the issuer public key</param>
        /// <param name="reciepient">the reciepent public key</param>
        /// <param name="amount">amout of cash for sending</param>
        /// <param name="seq">sequance number</param>
        /// <param name="transactionInputs">inputs</param>
        public Transaction(string name, byte version, string issuer, string reciepient, double amount, uint seq, List<TransactionInput> transactionInputs) :
            base(name, issuer)
        {
            TransactionVersion = version;
            TransactionIssuer = issuer;
            Reciepient = reciepient;
            Amount = amount;
            Sequence = seq;
            TransactionInputs = transactionInputs;
            TransactionOutputs = new List<TransactionOutput>();
            IsBlockReward = false;
            Apply(TransactionVersion);
        }

        [Obsolete("only use in Genesis phase.")]
        /// <summary>
        /// Genesis Creator
        /// </summary>
        public Transaction(byte version, string issuer, string reciepient, double amount, uint seq, string HashString) :
            this("Genesis", version, issuer, reciepient, amount, seq, null)
        {
            ContractHash = HashString;
        }
        /// <summary>
        /// uses in miners for creatging block reward
        /// </summary>
        /// <param name="version">block version</param>
        /// <param name="reciepient">sender and reciepient which both are the same</param>
        /// <param name="seq">block number</param>
        public Transaction(byte version, string reciepient, uint seq, double blockRewardAmount)
            : this("Block Reward", version, reciepient, reciepient, blockRewardAmount, seq, null)
        {
            IsBlockReward = true;
        }
        #endregion

        #region IContract
        public string GetHashString()
        {
            return (ID.ToString() + TransactionIssuer + Reciepient + Amount + TransactionVersion.ToString() + Sequence.ToString()).GetHashString(HashAlgorithms.SHA256);
        }
        /// <summary>
        /// using inside wallet for signing transaction
        /// </summary>
        /// <param name="protectedKey">issuer protectedkey path</param>
        public void GenerateSignture(string privateKey)
        {
            using (var rsa = new RsaSignatureProvider(privateKey))
            {
                var data = (ID.ToString() + TransactionIssuer + Reciepient + Amount + TransactionVersion.ToString() + Sequence.ToString());
                _signture = rsa.GenerateSignture(data);
                Signture = _signture.Value;
            }
        }
        [JsonIgnore]
        /// <summary>
        /// for verifying transaction at miner side
        /// </summary>
        public bool IsSignatureVerified
        {
            get
            {
                bool ret = false;
                using (var rsa = new RsaSignatureProvider(Convert.FromBase64String(TransactionIssuer)))
                {
                    var data = (ID.ToString() + TransactionIssuer + Reciepient + Amount + TransactionVersion.ToString() + Sequence.ToString());
                    ret = rsa.VerifySignature(data, _signture); ;
                }

                return ret;
            }
        }
        #endregion

        #region Process
        /// <summary>
        /// Processing Transaction In Miner
        /// </summary>
        /// <param name="checkGenesis">A function That returns whether a transaction is Genesis or not.</param>
        /// <param name="chechBlockReward">A function that indicate if Transaction is a Block Reward.</param>
        /// <param name="checkForUTXOs">check if transaction inputs are valide in network.</param>
        /// <param name="cleaningUTXOs">clearing old inputs and adding new ones in network UTXOs and update UTXOs.</param>
        /// <returns></returns>
        public bool Process(
            Func<Transaction, bool> checkGenesis,
            Func<Transaction, bool> checkBlockReward,
            Func<List<TransactionInput>, bool> checkForUTXOs,
            Func<List<TransactionInput>, List<TransactionOutput>, bool> cleaningUTXOs
            )
        {
            if (!IsSignatureVerified) return false;
            //check if Transaction Reward Or Genesis
            if (TransactionInputs == null)
            {
                if (!string.IsNullOrEmpty(ContractHash))
                {
                    var res = checkGenesis(this);
                    if (res) return cleaningUTXOs(null, TransactionOutputs);
                    else return res;
                }
                var resBR = checkBlockReward(this);
                if (resBR) return cleaningUTXOs(null, TransactionOutputs);
                else return resBR;
            }
            if (!checkForUTXOs(TransactionInputs))
            {
                return false;
            }
            var Change = InputsBalance - Amount;
            ContractHash = GetHashString();
            TransactionOutputs.Add(new TransactionOutput(Reciepient, Amount, ContractHash));
            TransactionOutputs.Add(new TransactionOutput(TransactionIssuer, Change, ContractHash));
            return cleaningUTXOs(TransactionInputs, TransactionOutputs);
        }
        public bool FinishingTransacion(Func<string, string, bool> finalize)
        {
            return finalize(ContractHash, ContainerBlockHash);
        }

        #endregion

        #region Principles
        public void Apply(byte version)
        {
            TransactionPrinciple = (TransactionPrinciples)DataUtilities.Principles[version.GetPrinciplesKey(PrinciplesType.Transaction)];
        }

        public bool Verify()
        {
            return (Amount <= TransactionPrinciple.Max) && (Amount >= TransactionPrinciple.Min);
        }

        #endregion
    }
}
