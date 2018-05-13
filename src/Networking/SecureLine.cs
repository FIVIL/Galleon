using System;
using System.Collections.Generic;
using System.Text;

namespace Galleon.Networking
{
    class SecureLine : ILine
    {
        public Line ConnectionLine { get; private set; }
    }
}
