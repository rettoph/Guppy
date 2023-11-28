using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
            {
                return source;
            }
                

            return source.Remove(source.LastIndexOf(value));
        }
    }
}
