using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class IElementExtensions
    {
        #region Align Methods
        /// <summary>
        /// Reposition the inputed rectangle to align with the current
        /// IElement instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="size"></param>
        /// <param name="alignment"></param>
        /// <param name="useWorldCoordinates"></param>
        /// <returns></returns>
        public static void Align(this IElement element, ref Rectangle rectangle, Alignment alignment, Boolean useWorldCoordinates = true)
        {
            // Default to Top Left alignment...
            Point location = useWorldCoordinates ? element.Bounds.Pixel.Location : Point.Zero;

            // Vertical Alignment...
            if ((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                location.Y += element.Bounds.Pixel.Height - rectangle.Height;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                location.Y += (element.Bounds.Pixel.Height - rectangle.Height) / 2;
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                location.X += element.Bounds.Pixel.Width - rectangle.Width;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                location.X += (element.Bounds.Pixel.Width - rectangle.Width) / 2;
            }

            rectangle.Location = location;
        }
        #endregion
    }
}
