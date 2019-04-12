using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle Overlap(this Rectangle source, Rectangle target)
        {
            if (source.Contains(target))
                return target;
            else if (!source.Intersects(target))
                return Rectangle.Empty;

            var overlap = new Rectangle();
            overlap.X = source.X < target.X ? target.X : source.X;
            overlap.Y = source.Y < target.Y ? target.Y : source.Y;
            overlap.Width = (source.Right < target.Right ? source.Right : target.Width) - target.X;
            overlap.Height = (source.Top < target.Top ? source.Top : target.Height) - target.Y;

            if (overlap.Width < 0)
                overlap.Width = 0;

            if (overlap.Height < 0)
                overlap.Height = 0;

            return overlap;
        }
    }
}
