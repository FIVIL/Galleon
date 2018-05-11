using System;
using System.Collections.Generic;
using System.Text;

namespace Galleon.Principles
{
    public interface IPrinciples
    {
        void Apply(byte version);
        bool Verify();

    }
}
