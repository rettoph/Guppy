using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
#pragma warning disable IDE1006 // Naming Styles
        public static uint xxHash(this string input)
#pragma warning restore IDE1006 // Naming Styles
        {
            var bytes = Encoding.Default.GetBytes(input);

            return K4os.Hash.xxHash.XXH32.DigestOf(bytes);
        }
    }
}
