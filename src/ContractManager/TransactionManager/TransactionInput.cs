using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Galleon.ContractManager.TransactionManager
{
    public class TransactionInput : IEquatable<TransactionInput>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as TransactionInput);
        }

        public bool Equals(TransactionInput other)
        {
            return other != null &&
                   TransactionOutputHash == other.TransactionOutputHash &&
                   EqualityComparer<TransactionOutput>.Default.Equals(UTXO, other.UTXO);
        }
        public override int GetHashCode() => Galleon.Util.StringUtilities.ToJson(this).GetHashCode();
        public static bool operator ==(TransactionInput input1, TransactionInput input2) => EqualityComparer<TransactionInput>.Default.Equals(input1, input2);

        public static bool operator !=(TransactionInput input1, TransactionInput input2) => !(input1 == input2);
    }
}
