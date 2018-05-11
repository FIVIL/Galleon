using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Galleon.Util;
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
        public string Encrypt(string message,StringEncoding encoding=StringEncoding.UTF8)
        {
            var byt = message.FromString(encoding);
            return Convert.ToBase64String(KeyPair.Rsa.Encrypt(byt, false));
        }
        public string Decrypt(string message,StringEncoding encoding=StringEncoding.UTF8)
        {
            var byt = Convert.FromBase64String(message);
            return KeyPair.Rsa.Decrypt(byt, false).ToString(encoding);
        }
    }
}
