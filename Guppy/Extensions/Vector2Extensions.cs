using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    public static class Vector2Extensions
    {
        public static Boolean Within(this Vector2 vector, Rectangle target)
        {
            return (target.Left <= vector.X && target.Top <= vector.Y) &&
                   (target.Right >= vector.X && target.Top <= vector.Y) &&
                   (target.Right >= vector.X && target.Bottom >= vector.Y) &&
                   (target.Left <= vector.X && target.Bottom >= vector.Y);

        }
    }
}
