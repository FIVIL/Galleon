using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galleon.ContractManager.TransactionManager
{
    public class TransactionInput
    {
        public string TransactionOutputHash { get; private set; }
        public TransactionOutput UTXO { get; set; }
        public TransactionInput(string TOH)
        {
            TransactionOutputHash = TOH;
        }
        public TransactionInput()
        {

        }
        [JsonConstructor]
        public TransactionInput(string TransactionOutputHash, TransactionOutput UTXO)
        {
            this.TransactionOutputHash = TransactionOutputHash;
            this.UTXO = UTXO;
        }
    }
}
