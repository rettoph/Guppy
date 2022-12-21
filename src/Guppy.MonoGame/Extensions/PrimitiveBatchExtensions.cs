using Guppy.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using RectangleF = System.Drawing.RectangleF;

namespace Guppy.MonoGame
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
        public static void Begin<TVertexType, TEffect>(this PrimitiveBatch<TVertexType, TEffect> primitiveBatch, Camera2D camera, BlendState blendstate = null)
            where TVertexType : struct, IVertexType
            where TEffect : Effect, IEffectMatrices
                => primitiveBatch.Begin(camera.View, camera.Projection, blendstate);
        #endregion

        #region PrimitiveBatch<VertexPositionColor> Extensions
        #region Draw Triangle Methods
        public static void DrawTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, float x1, float y1, float z1,
            Color c2, float x2, float y2, float z2,
            Color c3, float x3, float y3, float z3)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawTriangle(
                        new VertexPositionColor(new Vector3(x1, y1, z1), c1),
                        new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                        new VertexPositionColor(new Vector3(x3, y3, z3), c3));

        public static void DrawTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2, Vector3 p3)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawTriangle(
                        new VertexPositionColor(p1, color),
                        new VertexPositionColor(p2, color),
                        new VertexPositionColor(p3, color));

        public static void DrawTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector3 p1,
            Color c2, Vector3 p2,
            Color c3, Vector3 p3)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawTriangle(
                        new VertexPositionColor(p1, c1),
                        new VertexPositionColor(p2, c2),
                        new VertexPositionColor(p3, c3));

        public static void DrawTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector2 p1, Vector2 p2, Vector2 p3, float y = 0)
                where TEffect : Effect, IEffectMatrices
                     => primitiveBatch.DrawTriangle(
                        new VertexPositionColor(new Vector3(p1, y), color),
                        new VertexPositionColor(new Vector3(p2, y), color),
                        new VertexPositionColor(new Vector3(p3, y), color));

        public static void DrawTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector2 p1,
            Color c2, Vector2 p2,
            Color c3, Vector2 p3,
            float y = 0)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawTriangle(
                        new VertexPositionColor(new Vector3(p1, y), c1),
                        new VertexPositionColor(new Vector3(p2, y), c2),
                        new VertexPositionColor(new Vector3(p3, y), c3));
        #endregion

        #region DrawLine Methods
        public static void DrawLine<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, float x1, float y1, float z1,
            Color c2, float x2, float y2, float z2)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawLine(
                        new VertexPositionColor(new Vector3(x1, y1, z1), c1),
                        new VertexPositionColor(new Vector3(x2, y2, z2), c2));

        public static void DrawLine<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawLine(
                        new VertexPositionColor(p1, color),
                        new VertexPositionColor(p2, color));
        public static void DrawLine<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector3 p1,
            Color c2, Vector3 p2)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawLine(
                        new VertexPositionColor(p1, c1),
                        new VertexPositionColor(p2, c2));

        public static void DrawLine<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector2 p1, Vector2 p2, float y = 0)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawLine(
                        new VertexPositionColor(new Vector3(p1, y), color),
                        new VertexPositionColor(new Vector3(p2, y), color));

        public static void DrawLine<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector2 p1,
            Color c2, Vector2 p2,
            float y = 0)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.DrawLine(
                        new VertexPositionColor(new Vector3(p1, y), c1),
                        new VertexPositionColor(new Vector3(p2, y), c2));
        #endregion

        #region Draw Triangle Methods
        public static void TraceTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, float x1, float y1, float z1,
            Color c2, float x2, float y2, float z2,
            Color c3, float x3, float y3, float z3)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.TraceTriangle(
                        new VertexPositionColor(new Vector3(x1, y1, z1), c1),
                        new VertexPositionColor(new Vector3(x2, y2, z2), c2),
                        new VertexPositionColor(new Vector3(x3, y3, z3), c3));

        public static void TraceTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector3 p1, Vector3 p2, Vector3 p3)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.TraceTriangle(
                        new VertexPositionColor(p1, color),
                        new VertexPositionColor(p2, color),
                        new VertexPositionColor(p3, color));

        public static void TraceTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector3 p1,
            Color c2, Vector3 p2,
            Color c3, Vector3 p3)
                where TEffect : Effect, IEffectMatrices
                     => primitiveBatch.TraceTriangle(
                        new VertexPositionColor(p1, c1),
                        new VertexPositionColor(p2, c2),
                        new VertexPositionColor(p3, c3));

        public static void TraceTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Vector2 p1, Vector2 p2, Vector2 p3, Single y = 0)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.TraceTriangle(
                        new VertexPositionColor(new Vector3(p1, y), color),
                        new VertexPositionColor(new Vector3(p2, y), color),
                        new VertexPositionColor(new Vector3(p3, y), color));

        public static void TraceTriangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color c1, Vector2 p1,
            Color c2, Vector2 p2,
            Color c3, Vector2 p3,
            Single y = 0)
                where TEffect : Effect, IEffectMatrices
                    => primitiveBatch.TraceTriangle(
                        new VertexPositionColor(new Vector3(p1, y), c1),
                        new VertexPositionColor(new Vector3(p2, y), c2),
                        new VertexPositionColor(new Vector3(p3, y), c3));
        #endregion

        #region TraceRectangle Methods
        public static void TraceRectangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Rectangle rectangle)
                where TEffect : Effect, IEffectMatrices
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

        public static void TraceRectangleF<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, RectangleF rectangle)
                where TEffect : Effect, IEffectMatrices
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
        public static void DrawRectangle<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, Rectangle rectangle)
                where TEffect : Effect, IEffectMatrices
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

        public static void DrawRectangleF<TEffect>(
            this PrimitiveBatch<VertexPositionColor, TEffect> primitiveBatch,
            Color color, RectangleF rectangle)
                where TEffect : Effect, IEffectMatrices
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
        #endregion
    }
}
