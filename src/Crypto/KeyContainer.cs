using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Galleon.IO;
namespace Galleon.Crypto
{
    public class KeyContainer : IDisposable
    {
        public byte[] PrivateKey { get; private set; }
        public byte[] PublicKey { get; private set; }
        public string PublicKeyS { get; private set; }
        public RSACryptoServiceProvider Rsa { get; private set; }
        public KeyContainer()
        {
            Rsa = new RSACryptoServiceProvider();
            PrivateKey = Rsa.ExportCspBlob(true);
            PublicKey = Rsa.ExportCspBlob(false);
            PublicKeyS = Convert.ToBase64String(PublicKey);
        }
        public KeyContainer(string filepath)
        {
            PrivateKey = Galleon.IO.File.ReadAllBytes(filepath, IO.FileExtensions.sec);
            Rsa = new RSACryptoServiceProvider();
            Rsa.ImportCspBlob(PrivateKey);
            PrivateKey = Rsa.ExportCspBlob(true);
            PublicKey = Rsa.ExportCspBlob(false);
            PublicKeyS = Convert.ToBase64String(PublicKey);
        }
        public KeyContainer(byte[] publickey)
        {
            PublicKey = publickey;
            Rsa = new RSACryptoServiceProvider();
            Rsa.ImportCspBlob(PublicKey);
            PublicKey = Rsa.ExportCspBlob(false);
            PublicKeyS = Convert.ToBase64String(PublicKey);
        }
        public void ExportPrivateKey(string filename)
        {
            if (Rsa.PublicOnly) throw new Exception("no private key!");
            PrivateKey.WriteAllBytes(filename, FileExtensions.sec);
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
                    Rsa.Clear();
                    Rsa.Dispose();
                    Rsa = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~KeyContainer() {
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
