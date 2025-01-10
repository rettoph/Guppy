using System.Drawing;

namespace Microsoft.Xna.Framework
{
    public static class BoundingFrustumExtensions
    {
        public static RectangleF ToBounds2D(this BoundingFrustum frustum, float padding = 0)
        {
            float left = (frustum.Left.D * frustum.Left.Normal.X * -1) - padding;
            float right = (frustum.Right.D * frustum.Right.Normal.X * -1) + padding;

            float top = (frustum.Top.D * frustum.Top.Normal.Y * -1) - padding;
            float bottom = (frustum.Bottom.D * frustum.Bottom.Normal.Y * -1) + padding;

            float width = right - left;
            float height = bottom - top;

            return new RectangleF(
                x: left,
                y: top,
                width: width,
                height: height);
        }
    }
}