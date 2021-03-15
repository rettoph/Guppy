using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System.Collections
{
    public static class ListExtensions
    {
        public static void TryAddRange<T>(this List<T> list, IEnumerable<T> collection)
        {
            if (collection?.Any() ?? false)
                list.AddRange(collection);
        }
    }
}
