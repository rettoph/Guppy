using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Guppy.Utilities.Primitives;
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
        #region Begin Methods
        /// <summary>
        /// Helper method to begin rendering at utilizing an
        /// input <paramref name="camera"/> value rather than
        /// world <see cref="Matrix"/> values.
        /// Camera point.
        /// </summary>
        /// <typeparam name="TVertexType"></typeparam>
        /// <param name="primitiveBatch"></param>
        /// <param name="camera"></param>
        /// <param name="blendstate"></param>
        public static void Begin<TVertexType>(this PrimitiveBatch<TVertexType> primitiveBatch, Camera2D camera, BlendState blendstate = null)
            where TVertexType : struct, IVertexType
                => primitiveBatch.Begin(camera.View, camera.Projection, blendstate);
        #endregion

        #region PrimitiveBatch<VertexPositionColor> Extensions
        #region Draw Triangle Methods
        public static void DrawTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color c1, Single x1, Single y1, Single z1,
            Color c2, Single x2, Single y2, Single z2,
            Color c3, Single x3, Single y3, Single z3)
                => primitiveBatch.DrawTriangle(
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2));

        public static void DrawTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2, Vector3 p3)
                => primitiveBatch.DrawTriangle(
                    new VertexPositionColor(p1, color),
                    new VertexPositionColor(p2, color),
                    new VertexPositionColor(p3, color));

        public static void DrawTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color c1, Vector3 p1,
            Color c2, Vector3 p2,
            Color c3, Vector3 p3)
                 => primitiveBatch.DrawTriangle(
                    new VertexPositionColor(p1, c1),
                    new VertexPositionColor(p2, c2),
                    new VertexPositionColor(p3, c3));

        public static void DrawTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color color, Vector2 p1, Vector2 p2, Vector2 p3, Single y = 0)
                 => primitiveBatch.DrawTriangle(
                    new VertexPositionColor(new Vector3(p1, y), color),
                    new VertexPositionColor(new Vector3(p2, y), color),
                    new VertexPositionColor(new Vector3(p3, y), color));

        public static void DrawTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color c1, Vector2 p1, 
            Color c2, Vector2 p2, 
            Color c3, Vector2 p3,
            Single y = 0)
                 => primitiveBatch.DrawTriangle(
                    new VertexPositionColor(new Vector3(p1, y), c1),
                    new VertexPositionColor(new Vector3(p2, y), c2),
                    new VertexPositionColor(new Vector3(p3, y), c3));
        #endregion

        #region DrawLine Methods
        public static void DrawLine(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color c1, Single x1, Single y1, Single z1,
            Color c2, Single x2, Single y2, Single z2)
                => primitiveBatch.DrawLine(
                    new VertexPositionColor(new Vector3(x1, y1, z1), c1),
                    new VertexPositionColor(new Vector3(x1, y1, z1), c1));

        public static void DrawLine(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2)
                => primitiveBatch.DrawLine(
                    new VertexPositionColor(p1, color),
                    new VertexPositionColor(p2, color));
        public static void DrawLine(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color c1, Vector3 p1, 
            Color c2, Vector3 p2)
                => primitiveBatch.DrawLine(
                    new VertexPositionColor(p1, c1),
                    new VertexPositionColor(p2, c2));

        public static void DrawLine(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color color, Vector2 p1, Vector2 p2, Single y = 0)
                => primitiveBatch.DrawLine(
                    new VertexPositionColor(new Vector3(p1, y), color),
                    new VertexPositionColor(new Vector3(p2, y), color));

        public static void DrawLine(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color c1, Vector2 p1, 
            Color c2, Vector2 p2,
            Single y = 0)
                => primitiveBatch.DrawLine(
                    new VertexPositionColor(new Vector3(p1, y), c1),
                    new VertexPositionColor(new Vector3(p2, y), c2));
        #endregion

        #region Draw Triangle Methods
        public static void TraceTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color c1, Single x1, Single y1, Single z1,
            Color c2, Single x2, Single y2, Single z2,
            Color c3, Single x3, Single y3, Single z3)
                => primitiveBatch.TraceTriangle(
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                    new VertexPositionColor(new Vector3(x2, y2, z2), c2));

        public static void TraceTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2, Vector3 p3)
                => primitiveBatch.TraceTriangle(
                    new VertexPositionColor(p1, color),
                    new VertexPositionColor(p2, color),
                    new VertexPositionColor(p3, color));

        public static void TraceTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color c1, Vector3 p1,
            Color c2, Vector3 p2,
            Color c3, Vector3 p3)
                 => primitiveBatch.TraceTriangle(
                    new VertexPositionColor(p1, c1),
                    new VertexPositionColor(p2, c2),
                    new VertexPositionColor(p3, c3));

        public static void TraceTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color color, Vector2 p1, Vector2 p2, Vector2 p3, Single y = 0)
                 => primitiveBatch.TraceTriangle(
                    new VertexPositionColor(new Vector3(p1, y), color),
                    new VertexPositionColor(new Vector3(p2, y), color),
                    new VertexPositionColor(new Vector3(p3, y), color));

        public static void TraceTriangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch,
            Color c1, Vector2 p1,
            Color c2, Vector2 p2,
            Color c3, Vector2 p3,
            Single y = 0)
                 => primitiveBatch.TraceTriangle(
                    new VertexPositionColor(new Vector3(p1, y), c1),
                    new VertexPositionColor(new Vector3(p2, y), c2),
                    new VertexPositionColor(new Vector3(p3, y), c3));
        #endregion

        #region TraceRectangle Methods
        public static void TraceRectangle(
            this PrimitiveBatch<VertexPositionColor> primitiveBatch, 
            Color color, Rectangle rectangle)
        {
            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Right, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);
        }

        public static void TraceRectangleF(this PrimitiveBatch<VertexPositionColor> primitiveBatch, Color color, RectangleF rectangle)
        {
            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0);

            primitiveBatch.DrawLine(
                c1: color, x1: rectangle.Right, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Bottom, z2: 0);
        }
        #endregion

        #region DrawRectangle Methods
        public static void DrawRectangle(this PrimitiveBatch<VertexPositionColor> primitiveBatch, Color color, Rectangle rectangle)
        {
            primitiveBatch.DrawTriangle(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0,
                c3: color, x3: rectangle.Right, y3: rectangle.Bottom, z3: 0);

            primitiveBatch.DrawTriangle(
                c1: color, x1: rectangle.Right, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0,
                c3: color, x3: rectangle.Left, y3: rectangle.Top, z3: 0);
        }

        public static void DrawRectangleF(this PrimitiveBatch<VertexPositionColor> primitiveBatch, Color color, RectangleF rectangle)
        {
            primitiveBatch.DrawTriangle(
                c1: color, x1: rectangle.Left, y1: rectangle.Top, z1: 0,
                c2: color, x2: rectangle.Right, y2: rectangle.Top, z2: 0,
                c3: color, x3: rectangle.Right, y3: rectangle.Bottom, z3: 0);

            primitiveBatch.DrawTriangle(
                c1: color, x1: rectangle.Right, y1: rectangle.Bottom, z1: 0,
                c2: color, x2: rectangle.Left, y2: rectangle.Bottom, z2: 0,
                c3: color, x3: rectangle.Left, y3: rectangle.Top, z3: 0);
        }
        #endregion

        #region DrawPrimitivePath Methods
        public static void DrawPrimitive(this PrimitiveBatch<VertexPositionColor> primitiveBatch, Primitive primitive, Color color)
            => primitiveBatch.DrawPrimitive(primitive, color, Matrix.Identity);
        public static void DrawPrimitive(this PrimitiveBatch<VertexPositionColor> primitiveBatch, Primitive primitive, Color color, Matrix transformation)
            => primitive.Draw(color, transformation, primitiveBatch);
        #endregion
        #endregion
    }
}
