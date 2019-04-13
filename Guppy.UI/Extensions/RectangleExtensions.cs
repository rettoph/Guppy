using Guppy.UI.Enums;
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

        public static Vector2 Align(this Rectangle bounds, Vector2 size, Alignment alignment = Alignment.Center)
        {
            Vector2 position = Vector2.Zero;

            // Horizontal alignment of text here...
            if ((alignment & Alignment.Left) != 0)
            { // Left alignment...
                position.X = bounds.Left;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // Center alignment...
                position.X = bounds.Center.ToVector2().X - (size.X / 2);
            }
            else if ((alignment & Alignment.Right) != 0)
            { // Right alignment...
                position.X = bounds.Right - size.X;
            }

            // Horizontal Vertical of text here...
            if ((alignment & Alignment.Top) != 0)
            { // Top alignment...
                position.Y = bounds.Top;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // Center alignment...
                position.Y = bounds.Center.ToVector2().Y - (size.Y / 2);
            }
            else if ((alignment & Alignment.Bottom) != 0)
            { // Bottom alignment...
                position.Y = bounds.Bottom - size.Y;
            }

            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);

            return position;
        }
    }
}
