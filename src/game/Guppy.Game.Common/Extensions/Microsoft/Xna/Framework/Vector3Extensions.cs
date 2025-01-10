namespace Microsoft.Xna.Framework
{
    public static class Vector3Extensions
    {
        public static Vector2 ToVector2(this Vector3 vector3)
            => new(vector3.X, vector3.Y);
    }
}