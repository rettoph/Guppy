using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class PrimitiveBatchExtensions
    {
        public static void DrawRectangle(this PrimitiveBatch batch, Rectangle rectangle, Color color)
        {
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Top), color);
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Left, rectangle.Bottom), color);
            batch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
        } 
    }
}
