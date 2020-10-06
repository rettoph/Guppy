using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Microsoft.Xna.Framework
{
    public static class Vector2Extensions
    {
        public static Single Angle(this Vector2 p1, Vector2 p2)
            => MathHelper.WrapAngle((Single)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));

        public static Single Angle(this Vector2 vertex, Vector2 p1, Vector2 p2)
            => MathHelper.WrapAngle((Single)Math.Atan2(p2.Y - vertex.Y, p2.X - vertex.X) - (Single)Math.Atan2(p1.Y - vertex.Y, p1.X - vertex.X));

        public static Vector2 Rotate(this Vector2 v, Single radians)
            => Vector2.Transform(v, Matrix.CreateRotationZ(radians));
    }
}
