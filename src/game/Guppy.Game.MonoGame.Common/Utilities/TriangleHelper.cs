using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Common.Helpers
{
    public struct Triangle
    {
        /// <summary>
        /// Angle A
        /// </summary>
        public float A;

        /// <summary>
        /// Angle B
        /// </summary>
        public float B;

        /// <summary>
        /// Angle C
        /// </summary>
        public float C;

        /// <summary>
        /// Side a
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public float a;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Side b
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public float b;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Side c
        /// </summary>
#pragma warning disable IDE1006 // Naming Styles
        public float c;
#pragma warning restore IDE1006 // Naming Styles
    }

    public static class TriangleHelper
    {
        /// <summary>
        /// Give the known triangle values and return
        /// the full solved triangle
        /// </summary>
        /// <param name="A">Angle A</param>
        /// <param name="B">Angle B</param>
        /// <param name="C">Angle C</param>
        /// <param name="a">Side a</param>
        /// <param name="b">Side b</param>
        /// <param name="c">Side c</param>
        public static Triangle Solve(float? A = null, float? B = null, float? C = null, float? a = null, float? b = null, float? c = null)
        {
            var triangle = new Triangle()
            {
                A = A ?? 0,
                B = B ?? 0,
                C = C ?? 0,
                a = a ?? 0,
                b = b ?? 0,
                c = c ?? 0
            };

            #region AAS
            if (a != null && A != null && B != null)
            {
                TriangleHelper.AAS(A.Value, B.Value, a.Value, out triangle.C, out triangle.b, out triangle.c);
            }
            else if (a != null && A != null && C != null)
            {
                TriangleHelper.AAS(A.Value, C.Value, a.Value, out triangle.B, out triangle.c, out triangle.b);
            }
            else if (b != null && B != null && C != null)
            {
                TriangleHelper.AAS(B.Value, C.Value, b.Value, out triangle.A, out triangle.c, out triangle.a);
            }
            else if (b != null && B != null && A != null)
            {
                TriangleHelper.AAS(B.Value, A.Value, b.Value, out triangle.C, out triangle.a, out triangle.c);
            }
            else if (c != null && C != null && A != null)
            {
                TriangleHelper.AAS(C.Value, A.Value, c.Value, out triangle.B, out triangle.a, out triangle.b);
            }
            else if (c != null && C != null && B != null)
            {
                TriangleHelper.AAS(C.Value, B.Value, c.Value, out triangle.A, out triangle.b, out triangle.c);
            }
            #endregion

            #region ASA
            else if (a != null && C != null && B != null)
            {
                TriangleHelper.ASA(C.Value, B.Value, a.Value, out triangle.A, out triangle.b, out triangle.c);
            }
            else if (b != null && A != null && C != null)
            {
                TriangleHelper.ASA(A.Value, C.Value, b.Value, out triangle.B, out triangle.c, out triangle.a);
            }
            else if (c != null && B != null && A != null)
            {
                TriangleHelper.ASA(B.Value, A.Value, c.Value, out triangle.A, out triangle.a, out triangle.b);
            }
            #endregion

            else
            {
                throw new Exception("Unsolvable triangle detected.");
            }

            return triangle;
        }

        public static void AAS(float a1, float a2, float s1, out float a3, out float s2, out float s3)
        {
            s2 = (float)(s1 * MathF.Sin(a2) / MathF.Sin(a1));

            a3 = MathHelper.Pi - a1 - a2;
            s3 = (s1 * MathF.Sin(a3) / MathF.Sin(a1));
        }

        public static void ASA(float a1, float s1, float a2, out float a3, out float s2, out float s3)
        {
            a3 = MathHelper.Pi - a1 - a2;

            s2 = (s1 / MathF.Sin(a3) * MathF.Sin(a1));
            s3 = (s1 / MathF.Sin(a3) * MathF.Sin(a2));
        }
    }
}