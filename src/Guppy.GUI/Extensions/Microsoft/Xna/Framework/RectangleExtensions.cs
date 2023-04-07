using Guppy.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtensions
    {
        public static Rectangle SetLocation(this Rectangle rectangle, Point location)
        {
            rectangle.Location = location;

            return rectangle;
        }

        public static Rectangle Fit(this Rectangle rectangle, IStyle<Unit> width, IStyle<Unit> height)
        {
            if(width.TryGetValue(out var w))
            {
                rectangle.Width = w.Calculate(rectangle.Width);
            }

            if (height.TryGetValue(out var h))
            {
                rectangle.Height = h.Calculate(rectangle.Height);
            }

            return rectangle;
        }

        public static Rectangle Resize(this Rectangle rectangle, int deltaWidth, int deltaHeight)
        {
            rectangle.Width += deltaWidth;
            rectangle.Height += deltaHeight;

            return rectangle;
        }

        public static Rectangle Resize(this Rectangle rectangle, Point delta)
        {
            return rectangle.Resize(delta.X, delta.Y);
        }
    }
}
