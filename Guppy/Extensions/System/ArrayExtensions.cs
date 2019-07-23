using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class ArrayExtensions
    {
        public static T[] AddItems<T>(this T[] target, params T[] items)
        {
            var initialLength = target.Length;
            // Increase the array size...
            Array.Resize(ref target, target.Length + items.Length);

            for (var i = 0; i < items.Length; i++)
                target[initialLength + i] = items[i];

            return target;
        }
    }
}
