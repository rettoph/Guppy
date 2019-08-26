using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Collection
{
    public static class IEnumerableFrameableExtensions
    {
        public static void TryDrawAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : Frameable
        {
            foreach (Frameable item in source)
                item.TryDraw(gameTime);
        }

        public static void TryUpdateAll<T>(this IEnumerable<T> source, GameTime gameTime)
            where T : Frameable
        {
            foreach (Frameable item in source)
                item.TryUpdate(gameTime);
        }
    }
}
