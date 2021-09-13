using K4os.Hash.xxHash;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class StringExtensions
    {
#if DEBUG
        public static Dictionary<String, UInt32> _lookup = new Dictionary<String, UInt32>();
#endif
        public static UInt32 xxHash(this String value)
        {
#if DEBUG
            if(!_lookup.ContainsKey(value))
            {
                UInt32 hash = XXH32.DigestOf(Encoding.UTF8.GetBytes(value));

                _lookup.Add(value, hash);

                Console.WriteLine($"{hash} => {value}");

                return hash;
            }
#endif
            return XXH32.DigestOf(Encoding.UTF8.GetBytes(value));
        }

        public static String AddLeft(this String value, Char add, Int32 count = 1)
            => value.AddLeft(new String(add, count));
        public static String AddLeft(this String value, String add)
            => add + value;

        public static String AddRight(this String value, Char add, Int32 count = 1)
            => value.AddRight(new String(add, count));

        public static String AddRight(this String value, String add)
            => value + add;

        public static String Duplicate(this String input, Int32 count)
        {
            String output = "";
            for (Int32 i = 0; i < count; i++)
                output += input;

            return output;
        }
    }
}
