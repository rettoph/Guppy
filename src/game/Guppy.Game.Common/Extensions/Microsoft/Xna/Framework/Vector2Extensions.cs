namespace Microsoft.Xna.Framework
{
    public static class Vector2Extensions
    {
        public static float Angle(this Vector2 vertex, Vector2 p1)
        {
            return MathHelper.WrapAngle(MathF.Atan2(p1.Y - vertex.Y, p1.X - vertex.X));
        }

        public static float Angle(this Vector2 vertex, Vector2 p1, Vector2 p2)
        {
            return MathHelper.WrapAngle(MathF.Atan2(p2.Y - vertex.Y, p2.X - vertex.X) - MathF.Atan2(p1.Y - vertex.Y, p1.X - vertex.X));
        }

        public static Vector2 Round(this Vector2 vector)
        {
            return new(MathF.Round(vector.X), MathF.Round(vector.Y));
        }

        public static Vector2 Rotate(this Vector2 v, float delta)
        {
            return v.RotateTo(MathF.Atan2(v.Y, v.X) + delta);
        }

        public static Vector2 RotateTo(this Vector2 v, float target)
        {
            float l = v.Length();

            return new Vector2(
                x: MathF.Cos(target) * l,
                y: MathF.Sin(target) * l);
        }

        public static string ToString(this Vector2 v, string format)
        {
            return $"({v.X.ToString(format)}, {v.Y.ToString(format)})";
        }

        public static Vector3 ToVector3(this Vector2 vector2, float z = 0)
        {
            return new(vector2.X, vector2.Y, z);
        }
    }
}