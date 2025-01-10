using XnaColor = Microsoft.Xna.Framework.Color;

namespace System.Drawing
{
    public static class ColorExtensions
    {
        public static XnaColor ToXnaColor(this Color color)
        {
            return new(color.R, color.G, color.B, color.A);
        }
    }
}