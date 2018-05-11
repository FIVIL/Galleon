using System;
using System.Collections.Generic;
using System.Text;

namespace Galleon.Crypto
{
    public class RsaEncryptionProvider:IDisposable
    {
        private KeyContainer KeyPair { get; set; }
        public string PublicKeyS { get => KeyPair.PublicKeyS; }
        public byte[] PublicKey { get => KeyPair.PublicKey; }
        #region ctor
        /// <summary>
        /// First Use For Creating Wallet
        /// </summary>
        public RsaEncryptionProvider()
        {
            KeyPair = new KeyContainer();
        }
        /// <summary>
        /// for client inorder to decrypt
        /// </summary>
        /// <param name="filename">private key file path</param>
        public RsaEncryptionProvider(string filename)
        {
            KeyPair = new KeyContainer(filename);
        }
        /// <summary>
        /// for server inorder to encrypt
        /// </summary>
        /// <param name="publickey">public key</param>
        public RsaEncryptionProvider(byte[] publickey)
        {
            KeyPair = new KeyContainer(publickey);
        }
        #endregion
        public string Encrypt(string message)
        {
            var byt = Encoding.UTF8.GetBytes(message);
            if (byt.Length < 112)
            {
                var shortRsa = new ShortMessageRsa(KeyPair);
                return shortRsa.Encrypt(message);
            }
            else
            {
                var longRsa = new LongMessageRsa(KeyPair);
                return longRsa.Encrypt(message);
            }

        }
        public string Decrypt(string message)
        {
            var byt = Convert.FromBase64String(message);
            if (byt[0] == 0 && byt[2] == 0xff)
            {
                var longRsa = new LongMessageRsa(KeyPair);
                return longRsa.Decrypt(message);
            }
            else
            {
                var shortRsa = new ShortMessageRsa(KeyPair);
                return shortRsa.Decrypt(message);
            }
        }
        public void ExportKey(string filepath)
        {
            KeyPair.ExportPrivateKey(filepath);
        }

        public void Dispose()
        {
            ((IDisposable)KeyPair).Dispose();
        }
    }
}
