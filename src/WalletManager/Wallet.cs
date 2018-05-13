using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Crypto;
using Galleon.ContractManager.TransactionManager;
using Galleon.Util;
namespace Galleon.WalletManager
{
    public class Wallet
    {
        public KeyContainer KeyPair { get; private set; }
        private string PrivateKeyFilePath { get; set; }
        public string PublicKey { get => KeyPair.PublicKeyS; }
        /// <summary>
        /// un spend transaction outputs dedicated to this wallet
        /// </summary>
        public Dictionary<string, TransactionOutput> MyUTXOs { get; private set; }

        #region ctor
        /// <summary>
        /// very first time wallet generator which will generate the private and public key
        /// </summary>
        public Wallet()
        {
            KeyPair = new KeyContainer();
            KeyPair.ExportPrivateKey(Galleon.IO.File.PrivateKeyFileName);
            PrivateKeyFilePath = Galleon.IO.File.PrivateKeyFileName;
        }
        /// <summary>
        /// cloning wallet from existing privatekey for next logins.
        /// </summary>
        /// <param name="filePath">private key file path</param>
        public Wallet(string filePath)
        {
            PrivateKeyFilePath = filePath;
        }
        #endregion

        #region Refresh and Validation
        /// <summary>
        /// refresh list of utxos with network
        /// </summary>
        /// <param name="refresher">refresh and return new utxos list and get public key for checking</param>
        /// <returns>whether updating was successfull or not.</returns>
        public bool Refresh(Func<string, Dictionary<string, TransactionOutput>> refresher)
        {
            try
            {
                MyUTXOs = refresher(PublicKey);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// restricly check validation of utxos with networkd inorder to send some funds
        /// </summary>
        /// <param name="validator">validator function</param>
        /// <returns>whether validation was successfull or not.</returns>
        private bool ChechValidity(Func<Dictionary<string, TransactionOutput>, string, bool> validator)
        {
            return validator(MyUTXOs, PublicKey);
        }
        #endregion

        #region Issunig Transaction
        /// <summary>
        /// calculating balance only for showing to the  owner not sendeing funds.
        /// </summary>
        public double Balance
        {
            get
            {
                double retValue = 0;
                foreach (var item in MyUTXOs.Values)
                {
                    retValue += item.Amount;
                }
                return retValue;
            }
        }
        /// <summary>
        /// Issuing new Transaction.
        /// </summary>
        /// <param name="validator">validator function which validate are the funds that you clame are yours or not.</param>
        /// <param name="CleanUp">after creating transaction will clean up system utxos and mark them as processing.</param>
        /// <param name="recipientPublicKey">the Recipient PublicKey</param>
        /// <param name="amount">the amount to transsfer</param>
        /// <param name="seq">transaction sequance number</param>
        /// <param name="transactionName">transaction name(optional)</param>
        /// <returns>newly generated transaction for process and publish into network.</returns>
        public Transaction IsuueNewTransaction(Func<Dictionary<string, TransactionOutput>, string, bool> validator,
            Func<List<TransactionOutput>, bool> CleanUp,
            string recipientPublicKey, double amount, uint seq, string transactionName)
        {
            if (Balance < amount) throw new Exception("not enough credit!!");
            if (!ChechValidity(validator))
                throw new Exception("sth went wrong!!", new Exception("credit amount confilicted with the newtwork!!"));
            var inputs = new List<TransactionInput>();
            double TotalInputsAmounts = 0;
            foreach (var item in MyUTXOs.Values)
            {
                TotalInputsAmounts += item.Amount;
                inputs.Add(new TransactionInput(item.HashString));
                item.IsProcessing = true;
                if (TotalInputsAmounts >= amount) break;
            }
            var newTransaction = new Galleon.ContractManager.TransactionManager.Transaction(transactionName, DataUtilities.Version, PublicKey, recipientPublicKey,
                amount, seq, inputs);
            newTransaction.GenerateSignture(PrivateKeyFilePath);
            List<TransactionOutput> ProcessingUtxos = new List<TransactionOutput>();
            foreach (var item in inputs)
            {
                ProcessingUtxos.Add(MyUTXOs[item.TransactionOutputHash]);
                MyUTXOs.Remove(item.TransactionOutputHash);
            }
            if (!CleanUp(ProcessingUtxos)) throw new Exception("sth might be wrong with network connection!",
                 new Exception("couldnt update network!"));
            return newTransaction;
        }
        #endregion
    }
}
