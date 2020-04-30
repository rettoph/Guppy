using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class ITextElementExtensions
    {
        /// <summary>
        /// Calculate the positioning of the current text value.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Vector2 GetTextPosition(this ITextElement element)
        {
            var bounds = new Rectangle(Point.Zero, element.Font.MeasureString(element.Text).ToPoint());
            element.Align(ref bounds, element.TextAlignment);
            return bounds.Location.ToVector2();
        }
    }
}
