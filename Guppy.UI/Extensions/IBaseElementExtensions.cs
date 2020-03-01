using Guppy.UI.Components.Interfaces;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions
{
    public static class IBaseElementExtensions
    {
        #region Align Methods
        /// <summary>
        /// Aligns the given rectangle in the requested alignment method.
        /// 
        /// This will update the rectangles internal position.
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public static void Align(this IBaseElement element, ref Rectangle rectangle, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            // Default to Top Left alignment...
            Point position = useWorldCoordinates ? element.GetBounds().Location : Point.Zero;

            // Vertical Alignment...
            if ((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                position.Y += element.GetBounds().Height - rectangle.Height;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                position.Y += (element.GetBounds().Height - rectangle.Height) / 2;
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                position.X += element.GetBounds().Width - rectangle.Width;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                position.X += (element.GetBounds().Width - rectangle.Width) / 2;
            }

            // Update the recieved rectangles position.
            rectangle.Location = position;
        }

        /// <summary>
        /// Returns a rectangle aligned to the current element with the
        /// requested alignment type
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public static Rectangle Align(this IBaseElement element, Rectangle rectangle, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            element.Align(ref rectangle, alignment, useWorldCoordinates);
            return rectangle;
        }

        public static Vector2 Align(this IBaseElement element, Vector2 size, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            // Default to Top Left alignment...
            Vector2 position = useWorldCoordinates ? element.GetBounds().Location.ToVector2() : Vector2.Zero;

            // Vertical Alignment...
            if ((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                position.Y += (Int32)(element.GetBounds().Height - size.Y);
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                position.Y += (Int32)((element.GetBounds().Height - size.Y) / 2);
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                position.X += (Int32)(element.GetBounds().Width - size.X);
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                position.X += (Int32)((element.GetBounds().Width - size.X) / 2);
            }

            position.X = (Int32)position.X;
            position.Y = (Int32)position.Y;

            return position;
        }
        #endregion
    }
}
