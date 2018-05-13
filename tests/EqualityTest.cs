using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Galleon.ContractManager.TransactionManager;
using Galleon.ContractManager;
using Galleon.Principles;
using Galleon.Util;
namespace Galleon.tests
{
    public class EqualityTest
    {
        [Fact]
        public void PrinciplesTest()
        {
            var bp1 = new BasePrinciples(1, "nothing", "no");
            var bp2 = bp1.ToJson().FromJson<BasePrinciples>();
            Assert.Equal(bp1, bp2);
            Assert.True(bp1 == bp2);
            Assert.False(bp1 != bp2);
            Assert.True(bp1 != null);
            BasePrinciples bp3 = null;
            Assert.False(bp3 != null);
            Assert.True(bp3 == null);
            bp3 = new BasePrinciples(2, "anyting", "yes");
            Assert.False(bp1 == bp3);
            Assert.True(bp1 != bp3);
        }
        [Fact]
        public void TransactionPrinciplesTest()
        {
            var bp1 = new TransactionPrinciples(1, "nothing", "no", double.MaxValue, double.MinValue);
            var bp2 = bp1.ToJson().FromJson<BasePrinciples>();
            Assert.Equal(bp1, bp2);
            Assert.True(bp1 == bp2);
            Assert.False(bp1 != bp2);
            Assert.True(bp1 != null);
            BasePrinciples bp3 = null;
            Assert.False(bp3 != null);
            Assert.True(bp3 == null);
            bp3 = new TransactionPrinciples(2, "anyting", "yes", double.MaxValue, double.MinValue);
            Assert.False(bp1 == bp3);
            Assert.True(bp1 != bp3);
        }
        [Fact]
        public void BaseContractTest()
        {
            var bp1 = new ContractBase("a", "b");
            var bp2 = bp1.ToJson().FromJson<ContractBase>();
            Assert.Equal(bp1, bp2);
            Assert.True(bp1 == bp2);
            Assert.False(bp1 != bp2);
            Assert.True(bp1 != null);
            ContractBase bp3 = null;
            Assert.False(bp3 != null);
            Assert.True(bp3 == null);
            bp3 = new ContractBase("c", "d");
            Assert.False(bp1 == bp3);
            Assert.True(bp1 != bp3);
        }
        [Fact]
        public void TransactionTest()
        {
            DataUtilities.Principles.TryAdd((DataUtilities.Version).GetPrinciplesKey(Principles.PrinciplesType.Transaction), new Galleon.Principles.TransactionPrinciples(1, "hi", "short", 1000, 1));
            var bp1 = new Transaction("a", DataUtilities.Version, "b", "c", 12.5, 0, new List<TransactionInput>(){
                new TransactionInput("1"),
                new TransactionInput("2"),
            });
            var o1 = new TransactionOutput("c", 10, "d");
            var o2 = new TransactionOutput("c", 2.5, "e");
            bp1.TransactionInputs[0].UTXO = o1;
            bp1.TransactionInputs[1].UTXO = o2;
            var bp2 = bp1.ToJson().FromJson<Transaction>();
            Assert.Equal(bp1, bp2);
            Assert.True(bp1 == bp2);
            Assert.False(bp1 != bp2);
            Assert.True(bp1 != null);
            Transaction bp3 = null;
            Assert.False(bp3 != null);
            Assert.True(bp3 == null);
            bp3 = new Transaction("a", DataUtilities.Version, "b", "c", 12.5, 0, new List<TransactionInput>(){
                new TransactionInput("1"),
                new TransactionInput("2"),
            });
            Assert.False(bp1 == bp3);
            Assert.True(bp1 != bp3);
        }
        [Fact]
        public void GetHashCodTransactionTest()
        {
            DataUtilities.Principles.TryAdd((DataUtilities.Version).GetPrinciplesKey(Principles.PrinciplesType.Transaction), new Galleon.Principles.TransactionPrinciples(1, "hi", "short", 1000, 1));
            var bp1 = new Transaction("a", DataUtilities.Version, "b", "c", 12.5, 0, new List<TransactionInput>(){
                new TransactionInput("1"),
                new TransactionInput("2"),
            });
            var o1 = new TransactionOutput("c", 10, "d");
            var o2 = new TransactionOutput("c", 2.5, "e");
            bp1.TransactionInputs[0].UTXO = o1;
            bp1.TransactionInputs[1].UTXO = o2;
            var bp2 = bp1.ToJson().FromJson<Transaction>();
            Assert.Equal(bp1.GetHashCode(), bp2.GetHashCode());
            var bp3 = new Transaction("a", DataUtilities.Version, "b", "c", 12.5, 0, new List<TransactionInput>(){
                new TransactionInput("1"),
                new TransactionInput("2"),
            });
            Assert.NotEqual(bp1.GetHashCode(), bp3.GetHashCode());
        }
    }
}
