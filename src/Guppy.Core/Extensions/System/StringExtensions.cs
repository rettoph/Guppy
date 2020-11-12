using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Extensions.System
{
    public static class StringExtensions
    {
#if DEBUG
        private static Dictionary<UInt32, String> _cache = new Dictionary<uint, string>();
#endif

        public static UInt32 xxHash(this String value)
        {
#if DEBUG
            var hash = xxHashSharp.xxHash.CalculateHash(Encoding.UTF8.GetBytes(value));
            if (!_cache.ContainsKey(hash))
            {
                _cache[hash] = value;
                Console.WriteLine($"{hash} => {value}");
            }

            return hash;
#endif
            return xxHashSharp.xxHash.CalculateHash(Encoding.UTF8.GetBytes(value));
        }
    }
}
