using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtensions
    {
        public static Rectangle Move(this Rectangle point, Point delta)
        {
            point.Location += delta;
            return point;
        }

        public static Rectangle Intersection(this Rectangle r1, Rectangle r2)
        {
            return new Rectangle()
            {
                X = Math.Max(r1.Left, r2.Left),
                Y = Math.Max(r1.Top, r2.Top),
                Width = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left),
                Height = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top)
            };
        }
    }
}
