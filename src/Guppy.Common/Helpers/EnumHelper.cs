using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Helpers
{
    public static class EnumHelper
    {
        public static IDictionary<T, TValue> ToDictionary<T, TValue>()
            where T : struct, Enum
            where TValue : new()
        {
            return Enum.GetValues<T>().ToDictionary(
                keySelector: x => x,
                elementSelector: x => new TValue());
        }

        public static IDictionary<T, TValue> ToDictionary<T, TValue>(Func<T, TValue> factory)
            where T : struct, Enum
        {
            return Enum.GetValues<T>().ToDictionary(
                keySelector: x => x,
                elementSelector: x => factory(x));
        }
    }
}
