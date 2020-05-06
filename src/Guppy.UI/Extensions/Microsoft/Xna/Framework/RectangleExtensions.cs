using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.Microsoft.Xna.Framework
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
            Int32 x, y, width, height;
            if(r1.X < r2.X)
            {
                x = r2.X;
                width = r1.Right - r2.X;
            }
            else
            {
                x = r1.X;
                width = r2.Right - r1.X;
            }

            if (r1.Y < r2.Y)
            {
                y = r2.Y;
                height = r1.Bottom - r2.Y;
            }
            else
            {
                y = r1.Y;
                height = r2.Bottom - r1.Y;
            }

            return new Rectangle()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height
            };
        }
    }
}
