using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands.Extensions
{
    internal static class StringExtensions
    {
        public static string LowerCaseFirstLetter(this string input)
        {
            return char.ToLower(input[0]) + input.Substring(1);
        }
    }
}
