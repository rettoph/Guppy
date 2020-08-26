using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Extensions.System
{
    public static class StringExtensions
    {
        public static UInt32 xxHash(this String value)
            => xxHashSharp.xxHash.CalculateHash(Encoding.UTF8.GetBytes(value));
    }
}
