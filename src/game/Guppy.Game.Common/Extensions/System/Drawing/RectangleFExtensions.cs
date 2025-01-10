using Microsoft.Xna.Framework;

namespace System.Drawing
{
    public static class RectangleFExtensions
    {
        public static bool Contains(this RectangleF rect, Matrix transformation) => rect.Left < transformation.M41 && transformation.M41 < rect.Right
                && rect.Top < transformation.M42 && transformation.M42 < rect.Bottom;
    }
}