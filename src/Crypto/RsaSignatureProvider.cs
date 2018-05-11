using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Galleon.Util;
namespace Galleon.Crypto
{
    public class RsaSignatureProvider : IDisposable
    {

        private RSAPKCS1SignatureFormatter RSAFormatter { get; set; }
        private RSAPKCS1SignatureDeformatter RSADeformatter { get; set; }
        private KeyContainer KeyPair { get; set; }
        public string PublicKeyS { get => KeyPair.PublicKeyS; }
        public byte[] PublicKey { get => KeyPair.PublicKey; }
        private SHA256 Sha256 { get; set; }
        #region ctor
        /// <summary>
        /// First Use For Creating Wallet
        /// </summary>
        public RsaSignatureProvider()
        {
            KeyPair = new KeyContainer();
            RSAFormatter = new RSAPKCS1SignatureFormatter(KeyPair.Rsa);
            Sha256 = SHA256.Create();
            RSAFormatter.SetHashAlgorithm("SHA256");
            RSADeformatter = new RSAPKCS1SignatureDeformatter(KeyPair.Rsa);
            RSADeformatter.SetHashAlgorithm("SHA256");
        }
        /// <summary>
        /// for using wallet and sending transaction
        /// </summary>
        /// <param name="filename">private key file path</param>
        public RsaSignatureProvider(string filename)
        {
            KeyPair = new KeyContainer(filename);
            RSAFormatter = new RSAPKCS1SignatureFormatter(KeyPair.Rsa);
            Sha256 = SHA256.Create();
            RSAFormatter.SetHashAlgorithm("SHA256");
            RSADeformatter = new RSAPKCS1SignatureDeformatter(KeyPair.Rsa);
            RSADeformatter.SetHashAlgorithm("SHA256");
        }
        /// <summary>
        /// for miners inorder to verify signture using public key
        /// </summary>
        /// <param name="publickey">public key</param>
        public RsaSignatureProvider(byte[] publickey)
        {
            KeyPair = new KeyContainer(publickey);
            RSADeformatter = new RSAPKCS1SignatureDeformatter(KeyPair.Rsa);
            Sha256 = SHA256.Create();
            RSADeformatter.SetHashAlgorithm("SHA256");
        }
        #endregion

        #region Generate Signture
        public Signture GenerateSignture(byte[] data)
        {
            var hash = Sha256.ComputeHash(data);
            return new Signture(RSAFormatter.CreateSignature(hash));
        }
        private Signture GenerateSigntureASCII(string data)
        {
            var hash = Sha256.ComputeHash(Encoding.ASCII.GetBytes(data));
            return new Signture(RSAFormatter.CreateSignature(hash));
        }
        public Signture GenerateSignture(string data,StringEncoding encoding=StringEncoding.UTF8)
        {
            var hash = Sha256.ComputeHash(data.FromString(encoding));
            return new Signture(RSAFormatter.CreateSignature(hash));
        }
        #endregion

        #region Verify Signature
        public bool VerifySignature(byte[] data, Signture sign)
        {
            var hash = Sha256.ComputeHash(data);
            return RSADeformatter.VerifySignature(hash, sign);
        }
        private bool VerifySignatureASCII(string data, Signture sign)
        {
            var hash = Sha256.ComputeHash(Encoding.ASCII.GetBytes(data));
            return RSADeformatter.VerifySignature(hash, sign);
        }
        public bool VerifySignature(string data, Signture sign,StringEncoding encoding=StringEncoding.UTF8)
        {
            var hash = Sha256.ComputeHash(data.FromString(encoding));
            return RSADeformatter.VerifySignature(hash, sign);
        }
        #endregion
        public void ExportPrivateKey(string filepath)
        {
            KeyPair.ExportPrivateKey(filepath);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    KeyPair.Dispose();
                    KeyPair = null;
                    Sha256.Clear();
                    Sha256.Dispose();
                    Sha256 = null;
                    if (RSAFormatter != null)
                    {
                        RSAFormatter = null;
                    }
                    if (RSADeformatter != null)
                    {
                        RSADeformatter = null;
                    }
                    GC.Collect();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RsaSignatureProvider() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
