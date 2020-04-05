using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
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

        public static void FillRectangle(this PrimitiveBatch batch, Rectangle rectangle, Color color)
        {
            batch.DrawTriangle(new Vector2(rectangle.Left, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Top), color, new Vector2(rectangle.Right, rectangle.Bottom), color);
            batch.DrawTriangle(new Vector2(rectangle.Right, rectangle.Bottom), color, new Vector2(rectangle.Left, rectangle.Bottom), color, new Vector2(rectangle.Left, rectangle.Top), color);
        }

        public static void FillCircle(this PrimitiveBatch batch, Vector2 position, Single radius, Color color, UInt32 segments = 16)
        {
            var rotation = Matrix.CreateRotationZ(0);
            var segment = Vector2.UnitX * radius;

            for (Single i = 0; i < MathHelper.TwoPi; i += (MathHelper.TwoPi / segments))
            {
                batch.DrawTriangle(
                    p1: position + Vector2.Transform(segment, rotation),
                    c1: color,
                    p2: position + Vector2.Transform(segment, (rotation = Matrix.CreateRotationZ(i))),
                    c2: color,
                    p3: position,
                    c3: color);
            }

            batch.DrawTriangle(
                p1: position + Vector2.Transform(segment, rotation),
                c1: color,
                p2: position + segment,
                c2: color,
                p3: position,
                c3: color);
        }

        public static void DrawCircle(this PrimitiveBatch batch, Vector2 position, Single radius, Color color, UInt32 segments = 16)
        {
            var rotation = Matrix.CreateRotationZ(0);
            var segment = Vector2.UnitX * radius;

            for (Single i = 0; i < MathHelper.TwoPi; i += (MathHelper.TwoPi / segments))
            {
                batch.DrawLine(
                    p1: position + Vector2.Transform(segment, rotation), 
                    c1: color, 
                    p2: position + Vector2.Transform(segment, (rotation = Matrix.CreateRotationZ(i))),
                    c2: color);
            }

            batch.DrawLine(
                p1: position + Vector2.Transform(segment, rotation),
                c1: color,
                p2: position + segment,
                c2: color);
        }
    }
}
