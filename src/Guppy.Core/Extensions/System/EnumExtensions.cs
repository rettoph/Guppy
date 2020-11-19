using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetFlags<T>(this T flags)
            where T : Enum
        {
            foreach(T flag in Enum.GetValues(typeof(T)))
                if (flags.HasFlag(flag))
                    yield return flag;
        }
    }
}
