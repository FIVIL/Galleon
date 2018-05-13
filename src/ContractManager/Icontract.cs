using System;
using System.Collections.Generic;
using System.Text;
using Galleon.Crypto;
namespace Galleon.ContractManager
{
    public interface IContract
    {
        void GenerateSignture(string privateKey);
        string GetHashString();
        bool IsSignatureVerified
        {
            get;
        }
    }
}
