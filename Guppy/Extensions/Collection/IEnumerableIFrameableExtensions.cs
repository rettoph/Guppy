using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Collection
{
    public static class IEnumerableIFrameableExtensions
    {
        public static void TryDrawAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : IFrameable
        {
            foreach (IFrameable item in source)
                item.TryDraw(gameTime);
        }

        public static void TryUpdateAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : IFrameable
        {
            foreach (IFrameable item in source)
                item.TryUpdate(gameTime);
        }
    }
}
