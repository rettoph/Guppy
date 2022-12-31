﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Helpers
{
    public static class EnumHelper
    {
        public static IDictionary<T, TValue> ToDictionary<T, TValue>(params T[] except)
            where T : struct, Enum
            where TValue : new()
        {
            return ToDictionary(x => new TValue(), except);
        }

        public static IDictionary<T, TValue> ToDictionary<T, TValue>(Func<T, TValue> factory, params T[] except)
            where T : struct, Enum
        {
            return Enum.GetValues<T>().Except(except).ToDictionary(
                keySelector: x => x,
                elementSelector: x => factory(x));
        }
    }
}
