using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using Galleon.ContractManager.TransactionManager;
using Galleon.Util;
namespace Galleon.tests
{
    public class JsonTests
    {
        [Fact]
        public void JsonTransactionTest()
        {
            DataUtilities.Principles.TryAdd((DataUtilities.Version).GetPrinciplesKey(Principles.PrinciplesType.Transaction), new Galleon.Principles.TransactionPrinciples(1, "hi", "short", 1000, 1));
            var T = new Transaction("a", DataUtilities.Version, "b", "c", 12.5, 0, new List<TransactionInput>(){
                new TransactionInput("1"),
                new TransactionInput("2"),
            });
            var o1 = new TransactionOutput("c", 10, "d");
            var o2 = new TransactionOutput("c", 2.5, "e");
            T.TransactionInputs[0].UTXO = o1;
            T.TransactionInputs[1].UTXO = o2;
            var json = T.ToJson();
            var NT = json.FromJson<Transaction>();
            Assert.True(Equal(T, NT));
            Assert.Equal(json, NT.ToJson());
        }
        private bool Equal(Transaction t1,Transaction t2)
        {
            return (t1.ID == t2.ID) &&
                (t1.ContractName == t2.ContractName) &&
                (t1.ContractHash == t2.ContractHash) &&
                (t1.TransactionVersion == t2.TransactionVersion) &&
                (t1.TransactionPrinciple.ID == t2.TransactionPrinciple.ID) &&
                (t1.TransactionPrinciple.Version == t2.TransactionPrinciple.Version) &&
                (t1.TransactionPrinciple.VersionDecryption == t2.TransactionPrinciple.VersionDecryption) &&
                (t1.TransactionPrinciple.ShortTerms == t2.TransactionPrinciple.ShortTerms) &&
                (t1.TransactionPrinciple.Min == t2.TransactionPrinciple.Min) &&
                (t1.TransactionPrinciple.Max == t2.TransactionPrinciple.Max) &&
                (t1.TransactionIssuer == t2.TransactionIssuer) &&
                (t1.Reciepient == t2.Reciepient) &&
                (t1.Amount == t2.Amount) &&
                (t1.Sequence == t2.Sequence) &&
                (t1.TransactionInputs[0].TransactionOutputHash == t2.TransactionInputs[0].TransactionOutputHash) &&
                (t1.TransactionInputs[0].UTXO.ID == t2.TransactionInputs[0].UTXO.ID) &&
                (t1.TransactionInputs[0].UTXO.IssuingTime == t2.TransactionInputs[0].UTXO.IssuingTime) &&
                 (t1.TransactionInputs[1].TransactionOutputHash == t2.TransactionInputs[1].TransactionOutputHash) &&
                (t1.TransactionInputs[1].UTXO.ID == t2.TransactionInputs[1].UTXO.ID) &&
                (t1.TransactionInputs[1].UTXO.IssuingTime == t2.TransactionInputs[1].UTXO.IssuingTime);
        }
    }
}
