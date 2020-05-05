using Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class ITextComponentExtensions
    {
        /// <summary>
        /// Calculate the positioning of the current text value.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Vector2 GetTextPosition(this ITextComponent element)
        {
            var bounds = new Rectangle(Point.Zero, element.Font.MeasureStringWithMin(element.Text).ToPoint());
            element.Align(ref bounds, element.TextAlignment);

            // Right align text on overflow
            if (bounds.Width > element.Bounds.Pixel.Width)
                bounds.X -= bounds.Width - element.Bounds.Pixel.Width;
            return bounds.Location.ToVector2();
        }
    }
}
