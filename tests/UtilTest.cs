using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Galleon.Util;

namespace Galleon.tests
{
    public class UtilTest
    {
        [Fact]
        public void StringUtilTest()
        {
            var s1 = "xunit";
            var s2 = "ایکس یونیت";
            var s1ascii = s1.FromString(StringEncoding.ASCII);
            var s1utf8 = s1.FromString(StringEncoding.UTF8);
            Assert.Equal(s1, s1ascii.ToString(StringEncoding.ASCII));
            Assert.Equal(s1, s1utf8.ToString(StringEncoding.UTF8));
            s1 = s2;
            s1utf8 = s1.FromString(StringEncoding.UTF8);
            Assert.Equal(s1, s1utf8.ToString(StringEncoding.UTF8));
            byte[] cha = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var chs = cha.ToString(StringEncoding.Base64);
            Assert.Equal(cha, chs.FromString(StringEncoding.Base64));
        }
    }
}
