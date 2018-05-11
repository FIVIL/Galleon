using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Galleon.Crypto
{
    class ShortMessageRsa
    {
        private KeyContainer KeyPair { get; set; }
        public string PublicKeyS { get => KeyPair.PublicKeyS; }
        public byte[] PublicKey { get => KeyPair.PublicKey; }
        #region ctor
        public ShortMessageRsa(KeyContainer keyPair)
        {
            KeyPair = keyPair;
        }
        #endregion
        public string Encrypt(string message)
        {
            var byt = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(KeyPair.Rsa.Encrypt(byt, false));
        }
        public string Decrypt(string message)
        {
            var byt = Convert.FromBase64String(message);
            return Encoding.UTF8.GetString(KeyPair.Rsa.Decrypt(byt, false));
        }
    }
}
