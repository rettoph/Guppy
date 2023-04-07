using Guppy.GUI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Extended
{
    public static class RectangleFExtensions
    {
        public static RectangleF SetLocation(this RectangleF rectangle, Vector2 position)
        {
            rectangle.Position = position;

            return rectangle;
        }

        public static RectangleF Fit(this RectangleF rectangle, IStyle<Unit> width, IStyle<Unit> height)
        {
            if (width.TryGetValue(out var w))
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
