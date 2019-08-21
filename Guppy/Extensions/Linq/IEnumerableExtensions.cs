using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Linq
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);

            return source;
        }
    }
}
