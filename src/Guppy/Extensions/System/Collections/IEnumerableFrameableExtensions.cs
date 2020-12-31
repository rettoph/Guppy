using Guppy.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.System.Collections
{
    public static class IEnumerableFrameableExtensions
    {
        public static void TryDrawAll(this IEnumerable<IFrameable> list, GameTime gameTime)
        {
            foreach (IFrameable frameable in list)
                frameable.TryDraw(gameTime);
        }

        public static void TryUpdateAll(this IEnumerable<IFrameable> list, GameTime gameTime)
        {
            foreach (IFrameable frameable in list)
                frameable.TryUpdate(gameTime);
        }
    }
}
