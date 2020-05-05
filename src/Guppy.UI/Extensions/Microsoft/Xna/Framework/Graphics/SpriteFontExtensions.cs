using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics
{
    public static class SpriteFontExtensions
    {
        public static Vector2 MeasureStringWithMin(this SpriteFont font, String value)
        {
            var size = font.MeasureString(value);
            size.Y = Math.Max(font.LineSpacing + 1, size.Y);
            return size;
        }
    }
}
