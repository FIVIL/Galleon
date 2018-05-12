using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Galleon.Crypto;

namespace Galleon.tests
{
    public class CryptoTest
    {
        [Fact]
        public void TestAes()
        {
            Init.Initizer("Xunit",()=> "Xunit");
            var aes = new AesEncryptionProvider();
            var key = aes.Key;
            var aes2 = new AesEncryptionProvider();
            var message = "Unit Testing With Xunit is cool!!";
            var sb = new StringBuilder(message);
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(i.ToString());
                sb.Append(" : ");
                sb.Append(message);
                sb.AppendLine();
            }
            var Message = sb.ToString();
            var Enc1 = aes.Encrypt(message);
            var Enc2 = aes.Encrypt(Message);
            var Enc3 = aes2.Encrypt(Message);
            var Enc4 = aes.Encrypt(Message);
            Assert.NotEqual(Enc1, Enc2);
            Assert.NotEqual(Enc2, Enc3);
            Assert.Equal(Enc2, Enc4);
            Assert.Equal(message, aes.Decrpty(Enc1));
            Assert.Equal(Message, aes.Decrpty(Enc2));
            Assert.NotEqual(Message, aes.Decrpty(Enc1));
            Assert.Equal(Message, aes2.Decrpty(Enc3));
        }
        [Fact]
        public void TestRsaSignature()
        {
            Init.Initizer("Xunit",()=> "Xunit");
            var rsasign1 = new RsaSignatureProvider();
            var rsasign2 = new RsaSignatureProvider(rsasign1.PublicKey);
            rsasign1.ExportPrivateKey("testkey.dat");
            var rsasign3 = new RsaSignatureProvider("testkey.dat");
            var rsasign4 = new RsaSignatureProvider();
            var message = "Unit Testing With Xunit is cool!!";
            var sb = new StringBuilder(message);
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(i.ToString());
                sb.Append(" : ");
                sb.Append(message);
                sb.AppendLine();
            }
            var Message = sb.ToString();
            var sign1 = rsasign1.GenerateSignture(message);
            var sign2 = rsasign1.GenerateSignture(Message);
            var sign3 = rsasign3.GenerateSignture(Message);
            var sign4 = rsasign4.GenerateSignture(Message);
            Assert.NotEqual(sign1, sign2);
            Assert.NotEqual(sign2, sign4);
            Assert.True(rsasign1.VerifySignature(message, sign1));
            Assert.True(rsasign1.VerifySignature(Message, sign2));
            Assert.False(rsasign1.VerifySignature(message, sign2));
            Assert.True(rsasign1.VerifySignature(Message, sign2));
            Assert.True(rsasign1.VerifySignature(Message, sign3));
            Assert.True(rsasign3.VerifySignature(message, sign1));
            Assert.False(rsasign4.VerifySignature(message, sign1));
            rsasign1.Dispose();
            rsasign2.Dispose();
            rsasign3.Dispose();
            rsasign4.Dispose();
        }
        [Fact]
        public void TestRsa()
        {
            Init.Initizer("Xunit",()=> "Xunit");
            var rsa1 = new RsaEncryptionProvider();
            var rsa2 = new RsaEncryptionProvider(rsa1.PublicKey);
            rsa1.ExportKey("testkey.dat");
            var rsa3 = new RsaEncryptionProvider("testkey.dat");
            var rsa4 = new RsaEncryptionProvider();
            var message = "Unit Testing With Xunit is cool!!";
            var sb = new StringBuilder(message);
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(i.ToString());
                sb.Append(" : ");
                sb.Append(message);
                sb.AppendLine();
            }
            var Message = sb.ToString();
            var enc1 = rsa1.Encrypt(message);
            var enc2 = rsa1.Encrypt(Message);
            var enc3 = rsa2.Encrypt(Message);
            var enc4 = rsa4.Encrypt(Message);
            Assert.NotEqual(enc1, enc2);
            Assert.NotEqual(enc2, enc4);
            var dec1 = rsa1.Decrypt(enc1);
            var dec2 = rsa1.Decrypt(enc2);
            var dec3 = rsa3.Decrypt(enc2);
            var dec4 = rsa4.Decrypt(enc4);
            Assert.Equal(message, dec1);
            Assert.NotEqual(dec1, dec2);
            Assert.Equal(dec2, dec3);
            Assert.Equal(dec3, dec4);
            Assert.Equal(dec4, Message);
            Assert.Equal(dec2, Message);
            rsa1.Dispose();
            rsa2.Dispose();
            rsa3.Dispose();
            rsa4.Dispose();
        }
    }
}
