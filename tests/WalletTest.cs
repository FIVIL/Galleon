using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Galleon.Crypto;
using Galleon.WalletManager;
using Galleon.Util;
using Galleon.ContractManager.TransactionManager;

namespace Galleon.tests
{
    public class WalletTest
    {
        private Dictionary<string, TransactionOutput> utxos = new Dictionary<string, TransactionOutput>();
        private Dictionary<string, TransactionOutput> utxosp = new Dictionary<string, TransactionOutput>();
        private Dictionary<string, TransactionOutput> utxosm { get; set; } = new Dictionary<string, TransactionOutput>();
        //[Fact] //uncomment this for being activet,test is pased and work fine
        public void TestWallet()
        {
            Init.Initizer("Xunit",()=>"Xunit");
            DataUtilities.Principles.TryAdd((DataUtilities.Version).GetPrinciplesKey(Principles.PrinciplesType.Transaction), new Galleon.Principles.TransactionPrinciples(1, "hi", "short", 1000, 1));
            var W0 = new Wallet();
            var W1 = new Wallet();
            var W2 = new Wallet();
            var W3 = new Wallet();
            var T = new Transaction(DataUtilities.Version, W0.PublicKey, W1.PublicKey, 100, 0,
                Guid.NewGuid().ToString().GetHashString(HashAlgorithms.SHA256));
            T.GenerateSignture(Galleon.IO.File.PrivateKeyFileName);
            T.TransactionOutputs.Add(new TransactionOutput(T.Reciepient, T.Amount, T.TransactionHash));
            utxos.Add(T.TransactionOutputs[0].HashString, T.TransactionOutputs[0]);
            Assert.True(T.Process(x => true, null, null, Cleaner));
            W1.Refresh(refresh);
            Assert.Equal(100, W1.Balance);
            var T2 = W1.IsuueNewTransaction(Validator, Clean, W2.PublicKey, 15, 2, "h");
            W1.Refresh(refresh);
            W2.Refresh(refresh);
            Assert.Equal(0, W1.Balance);
            Assert.Equal(0, W2.Balance);
            var TF1 = new Transaction(DataUtilities.Version, W3.PublicKey, 10,10);
            TF1.GenerateSignture(Galleon.IO.File.PrivateKeyFileName);
            TF1.TransactionOutputs.Add(new TransactionOutput(TF1.Reciepient, TF1.Amount, TF1.TransactionHash));
            utxos.Add(TF1.TransactionOutputs[0].HashString, TF1.TransactionOutputs[0]);
            Assert.True(T2.Process(x => false, null, CFA, Cleaner));
            Assert.True(TF1.Process(x => false, checkforminerreward, CFA, Cleaner));
            W1.Refresh(refresh);
            W2.Refresh(refresh);
            W3.Refresh(refresh);
            Assert.Equal(85, W1.Balance);
            Assert.Equal(15, W2.Balance);
            Assert.Equal(10, W3.Balance);
            var T3 = W1.IsuueNewTransaction(Validator, Clean, W2.PublicKey, 15, 3, "h2");
            W1.Refresh(refresh);
            W2.Refresh(refresh);
            Assert.Equal(0, W1.Balance);
            Assert.Equal(15, W2.Balance);
            var TF2 = new Transaction(DataUtilities.Version, W3.PublicKey, 10,10);
            TF2.TransactionOutputs.Add(new TransactionOutput(TF2.Reciepient, TF2.Amount, TF2.TransactionHash));
            utxos.Add(TF2.TransactionOutputs[0].HashString, TF2.TransactionOutputs[0]);
            Assert.True(T3.Process(x => false, null, CFA, Cleaner));
            TF2.GenerateSignture(Galleon.IO.File.PrivateKeyFileName);
            Assert.True(TF2.Process(x => false, checkforminerreward, CFA, Cleaner));
            W1.Refresh(refresh);
            W2.Refresh(refresh);
            W3.Refresh(refresh);
            Assert.Equal(70, W1.Balance);
            Assert.Equal(30, W2.Balance);
            Assert.Equal(20, W3.Balance);
        }
        #region funcs
        private bool checkforminerreward(Transaction t)
        {
            //in reall app remember to make utxo work
            var res = true;
            foreach (var item in t.TransactionOutputs)
            {
                if (!utxosm.ContainsKey(item.HashString))
                {
                    res = false;
                    break;
                }
            }
            //has to change
            return true;
        }
        private bool Validator(Dictionary<string, TransactionOutput> ut, string publickey)
        {
            var res = true;
            foreach (var item in ut)
            {
                if (!utxos.ContainsKey(item.Key))
                {
                    res = false;
                    break;
                }
                if (!item.Value.IsMine(publickey))
                {
                    res = false;
                    break;
                }
            }
            return res;
        }
        private bool CFA(List<TransactionInput> p)
        {
            var res = true;
            foreach (var item in p)
            {
                if (!utxosp.ContainsKey(item.TransactionOutputHash))
                {
                    res = false;
                    break;
                }
                else
                {
                    item.UTXO = utxosp[item.TransactionOutputHash];
                }
            }
            return res;
        }
        private bool Clean(List<TransactionOutput> outp)
        {
            foreach (var item in outp)
            {
                if (utxos.ContainsKey(item.HashString))
                    utxos.Remove(item.HashString);
                else if (utxosm.ContainsKey(item.HashString))
                    utxosp.Remove(item.HashString);
                utxosp.Add(item.HashString, item);

            }
            return true;
        }
        private bool Cleaner(List<TransactionInput> inp, List<TransactionOutput> outp)
        {
            try
            {
                foreach (var item in inp)
                {
                    if (item.UTXO != null)
                        utxosp.Remove(item.UTXO.HashString);
                }
                foreach (var item in outp)
                {
                    item.IsProcessing = false;
                    utxos.Add(item.HashString, item);
                }
            }
            catch { }
            return true;
        }
        private Dictionary<string, TransactionOutput> refresh(string key)
        {
            var p = new Dictionary<string, TransactionOutput>();
            foreach (var item in utxos.Values)
            {
                if (item.IsMine(key))
                    p.Add(item.HashString, item);
            }
            return p;
        }
        #endregion
    }
}
