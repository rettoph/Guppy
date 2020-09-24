using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Collections
{
    public static class IEnumerableFrameableExtensions
    {
        public static void TryDrawAll(this IEnumerable<IFrameable> list, GameTime gameTime)
            => list.ForEach(f => f.TryDraw(gameTime));

        public static void TryUpdateAll(this IEnumerable<IFrameable> list, GameTime gameTime)
            => list.ForEach(f => f.TryUpdate(gameTime));
    }
}
