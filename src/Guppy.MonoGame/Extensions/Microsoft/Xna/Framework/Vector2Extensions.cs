using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Xna.Framework
{
    public static class Vector2Extensions
    {
        public static Single Angle(this Vector2 p2, Vector2 p1)
            => MathHelper.WrapAngle((Single)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));

        public static Single Angle(this Vector2 vertex, Vector2 p1, Vector2 p2)
            => MathHelper.WrapAngle((Single)Math.Atan2(p2.Y - vertex.Y, p2.X - vertex.X) - (Single)Math.Atan2(p1.Y - vertex.Y, p1.X - vertex.X));

        public static Vector2 Round(this Vector2 vector)
            => new Vector2((Single)Math.Round(vector.X), (Single)Math.Round(vector.Y));

        public static Vector2 Rotate(this Vector2 v, Single delta)
            => v.RotateTo((Single)Math.Atan2(v.Y, v.X) + delta);

        public static Vector2 RotateTo(this Vector2 v, Single target)
        {
            var l = v.Length();

            return new Vector2(
                x: (Single)Math.Cos(target) * l,
                y: (Single)Math.Sin(target) * l);
        }

        public static String ToString(this Vector2 v, String format)
            => $"({v.X.ToString(format)}, {v.Y.ToString(format)})";

        public static Vector3 ToVector3(this Vector2 vector2, Single z = 0)
            => new Vector3(vector2.X, vector2.Y, z);
    }
}
