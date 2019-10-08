using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.Collection
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Int32 total = source.Count();

            for (Int32 i = 0; i < total; i++)
                action(source.ElementAt(i));

            return source;
        }
    }
}
