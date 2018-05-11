using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Galleon.Crypto;
using Galleon.Util;
namespace Galleon.tests
{
    public class TransactionTest
    {
        [Fact]
        public void TransactionTest1()
        {
            Init.Initizer("Xunit");
            var key1 = new KeyContainer();
            var key2 = new KeyContainer();
            key1.ExportPrivateKey("key.dat");
            DataUtilities.Principles.TryAdd((DataUtilities.Version).GetPrinciplesKey(Principles.PrinciplesType.Transaction), new Galleon.Principles.TransactionPrinciples(1, "hi", "short", 1000, 1));
            var t1 = new Galleon.TransactionManager.Transaction(DataUtilities.Version, key1.PublicKeyS, key2.PublicKeyS, 15.5, 0, Guid.NewGuid().ToString().GetHashString(HashAlgorithms.SHA256));
            t1.GenerateSignture("key.dat");
            Assert.True(t1.IsSignatureVerified);
            key2.ExportPrivateKey("key.dat");
            t1.GenerateSignture("key.dat");
            Assert.False(t1.IsSignatureVerified);
            key1.Dispose();
            key2.Dispose();
        }
    }
}
