using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Guppy.Extensions.Utilities
{
    public static class PrimitiveBatchExtensions
    {
        public static void Begin(this PrimitiveBatch primitiveBatch, Camera2D camera, BlendState blendstate = null)
        {
            primitiveBatch.Begin(camera.View, camera.Projection, blendstate);
        }

        public static void DrawRectangle(this PrimitiveBatch batch, Color color, Rectangle rectangle)
        {
            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Right, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);
        }

        public static void FillRectangle(this PrimitiveBatch batch, Color color, Rectangle rectangle)
        {
            batch.DrawTriangle(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0,
                c3: color, x3: rectangle.Right, y3: rectangle.Bottom, z3: 0);

            batch.DrawTriangle(
                c1: color, x1: rectangle.Right, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0,
                c3: color, x3: rectangle.Left, y3: rectangle.Top, z3: 0);
        }

        public static void DrawRectangleF(this PrimitiveBatch batch, Color color, RectangleF rectangle)
        {
            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0);

            batch.DrawLine(
                c1: color, x1: rectangle.Right, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);
        }

        public static void FillRectangleF(this PrimitiveBatch batch, Color color, RectangleF rectangle)
        {
            batch.DrawTriangle(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0,
                c3: color, x3: rectangle.Right, y3: rectangle.Bottom, z3: 0);

            batch.DrawTriangle(
                c1: color, x1: rectangle.Right, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0,
                c3: color, x3: rectangle.Left, y3: rectangle.Top, z3: 0);
        }
    }
}
