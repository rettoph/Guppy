using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Guppy.Utilities
{
    public struct Triangle
    {
        /// <summary>
        /// Angle A
        /// </summary>
        public Single A;

        /// <summary>
        /// Angle B
        /// </summary>
        public Single B;

        /// <summary>
        /// Angle C
        /// </summary>
        public Single C;

        /// <summary>
        /// Side a
        /// </summary>
        public Single a;

        /// <summary>
        /// Side b
        /// </summary>
        public Single b;

        /// <summary>
        /// Side c
        /// </summary>
        public Single c;
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
        public static Triangle Solve(Single? A = null, Single? B = null, Single? C = null, Single? a = null, Single? b = null, Single? c = null)
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
                TriangleHelper.AAS(A.Value, B.Value, a.Value, out triangle.C, out triangle.b, out triangle.c);
            else if (a != null && A != null && C != null)
                TriangleHelper.AAS(A.Value, C.Value, a.Value, out triangle.B, out triangle.c, out triangle.b);

            else if (b != null && B != null && C != null)
                TriangleHelper.AAS(B.Value, C.Value, b.Value, out triangle.A, out triangle.c, out triangle.a);
            else if (b != null && B != null && A != null)
                TriangleHelper.AAS(B.Value, A.Value, b.Value, out triangle.C, out triangle.a, out triangle.c);

            else if (c != null && C != null && A != null)
                TriangleHelper.AAS(C.Value, A.Value, c.Value, out triangle.B, out triangle.a, out triangle.b);
            else if (c != null && C != null && B != null)
                TriangleHelper.AAS(C.Value, B.Value, c.Value, out triangle.A, out triangle.b, out triangle.c);
            #endregion

            else
                throw new Exception("Unsolvable triangle detected.");

            return triangle;
        }

        public static void AAS(Single a1, Single a2, Single s1, out Single a3, out Single s2, out Single s3)
        {
            s2 = (Single)((s1 * Math.Sin(a2)) / Math.Sin(a1));

            a3 = MathHelper.Pi - a1 - a2;
            s3 = (Single)((s1 * Math.Sin(a3)) / Math.Sin(a1));
        }
    }
}
