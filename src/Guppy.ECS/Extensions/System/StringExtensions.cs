using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static uint xxHash(this string input)
        {
            var bytes = Encoding.Default.GetBytes(input);

            return K4os.Hash.xxHash.XXH32.DigestOf(bytes);
        }
    }
}
