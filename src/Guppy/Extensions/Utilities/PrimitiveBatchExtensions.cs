using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Utilities
{
    public static class PrimitiveBatchExtensions
    {
        public static void Begin(this PrimitiveBatch primitiveBatch, Camera2D camera, BlendState blendstate = null)
        {
            primitiveBatch.Begin(camera.View, camera.Projection, blendstate);
        }

        public static void DrawRectangle(this PrimitiveBatch batch, Rectangle rectangle, Color color)
        {
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Top), color);
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
            batch.DrawLine(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Left, rectangle.Bottom), color);
            batch.DrawLine(new Vector2(rectangle.Right, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
        }

        public static void FillRectangle(this PrimitiveBatch batch, Rectangle rectangle, Color color)
        {
            batch.DrawTriangle(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
            batch.DrawTriangle(new Vector2(rectangle.Right, rectangle.Bottom), color, new Vector2(rectangle.Left, rectangle.Bottom), color, new Vector2(rectangle.Left, rectangle.Top), color);
        }
    }
}
