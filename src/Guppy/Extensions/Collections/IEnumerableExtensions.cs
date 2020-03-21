using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Collections
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);

            return source;
        }
    }
}
