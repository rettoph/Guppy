namespace Microsoft.Xna.Framework
{
    public static class SingleExtensions
    {
        public static Vector2 ToVector2(this float x, float y = 0)
        {
            return new(x, y);
        }
    }
}