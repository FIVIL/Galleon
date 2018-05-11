using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Galleon.Crypto
{
    class LongMessageRsa
    {
        private KeyContainer KeyPair { get; set; }
        public string PublicKeyS { get => KeyPair.PublicKeyS; }
        public byte[] PublicKey { get => KeyPair.PublicKey; }
        public AesEncryptionProvider Aes { get; set; }
        #region ctor
        public LongMessageRsa(KeyContainer keyPair)
        {
            KeyPair = keyPair;
        }
        #endregion
        public string Encrypt(string message)
        {
            //create new aes encryptor with random generated key
            Aes = new AesEncryptionProvider();
            Console.WriteLine(Aes.Key);
            //first encrypt the long message with aes
            var EncryptedAesMessage = Aes.Encrypt(message);
            var EncryptedAesMessageByte = Convert.FromBase64String(EncryptedAesMessage);
            //then encrypt aes key using rsa
            var EncryptedKey = KeyPair.Rsa.Encrypt(Encoding.ASCII.GetBytes(Aes.Key), false);
            byte KeyLength = (byte)EncryptedKey.Length;
            //putting all data together
            List<byte> Bl = new List<byte>()
            {
               0,KeyLength,0xff
            };
            Bl.AddRange(EncryptedKey);
            Bl.Add(0);
            Bl.AddRange(EncryptedAesMessageByte);
            return Convert.ToBase64String(Bl.ToArray());
        }
        public string Decrypt(string message)
        {
            var BL = Convert.FromBase64String(message);
            if (BL[0] != 0) throw new Exception("wrong message format");
            if (BL[2] != 0xff) throw new Exception("wrong message format");
            var KeyLength = BL[1];
            var EncryptedKey = BL.Skip(3).Take(KeyLength).ToArray();
            var DecryptedKey = KeyPair.Rsa.Decrypt(EncryptedKey, false);
            var DecryptedKeyText = Encoding.ASCII.GetString(DecryptedKey);
            var EncryptedMessageP = BL.Skip(3 + KeyLength).ToArray();
            if (EncryptedMessageP[0] != 0) throw new Exception("wrong message format");
            var EncryptedMessage = EncryptedMessageP.Skip(1).ToArray();
            Aes = new AesEncryptionProvider(DecryptedKeyText);
            return Aes.Decrpty(Convert.ToBase64String(EncryptedMessage));
        }
    }
}
