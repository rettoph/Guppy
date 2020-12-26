using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum Alignment
    {
        Top = 1,
        VerticalCenter = 2,
        Bottom = 4,
        Left = 8,
        HorizontalCenter = 16,
        Right = 32,

        /*
         * Shorthands
         */
        TopLeft = Alignment.Top | Alignment.Left,
        TopCenter = Alignment.Top | Alignment.HorizontalCenter,
        TopRight = Alignment.Top | Alignment.Right,
        CenterLeft = Alignment.VerticalCenter | Alignment.Left,
        CenterCenter = Alignment.VerticalCenter | Alignment.HorizontalCenter,
        CenterRight = Alignment.VerticalCenter | Alignment.Right,
        BottomLeft = Alignment.Bottom | Alignment.Left,
        BottomCenter = Alignment.Bottom | Alignment.HorizontalCenter,
        BottomRight = Alignment.Bottom | Alignment.Right,
    }

    public static class AlignmentExtensions
    {
        /// <summary>
        /// Return the position to place a recieved item based on the recieved container
        /// </summary>
        /// <param name="alignment"></param>
        /// <param name="item">The "size" if theitem to align.</param>
        /// <param name="container">The bounds within which to align the item.</param>
        /// <returns></returns>
        public static Vector2 Align(this Alignment alignment, Vector2 item, Rectangle container)
        {
            Vector2 position = Vector2.Zero;

            /*
             * Horizontal Alignment Logic
             */
            if(item.X > container.Width)
            { // If the bounds overextend we hsould right align
                position.X = container.Left + container.Width - item.X;
            }
            else if ((alignment & Alignment.Left) != 0)
            {
                position.X = container.Left;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            {
                position.X = container.Left + (container.Width - item.X) / 2;
            }
            else if ((alignment & Alignment.Right) != 0)
            {
                position.X = container.Left + container.Width - item.X;
            }

            /*
             * Vertical Alignment Logic
             */
            if ((alignment & Alignment.Top) != 0)
            {
                position.Y = container.Top;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            {
                position.Y = container.Top + (container.Height - item.Y) / 2;
            }
            else if ((alignment & Alignment.Bottom) != 0)
            {
                position.Y = container.Top + container.Height - item.Y;
            }

            position.Round();
            return position;
        }
    }
}
