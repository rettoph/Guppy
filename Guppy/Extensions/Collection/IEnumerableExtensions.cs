using Guppy.Implementations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Collection
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);

            return source;
        }

        public static void TryDrawAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : Frameable
        {
            foreach (var item in source)
                item.TryDraw(gameTime);
        }

        public static void TryUpdateAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : Frameable
        {
            foreach (var item in source)
                item.TryUpdate(gameTime);
        }
    }
}
