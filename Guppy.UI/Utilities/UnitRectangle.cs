using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    /// <summary>
    /// Rectangle class that utilizes the custom Unit UI class
    /// </summary>
    public class UnitRectangle
    {
        public Unit X { get; set; }
        public Unit Y { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        public Int32 Top { get; private set; }
        public Int32 Right { get; private set; }
        public Int32 Bottom { get; private set; }
        public Int32 Left { get; private set; }

        public Rectangle Output;

        public UnitRectangle(Unit x, Unit y, Unit width, Unit height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            Output = new Rectangle(x, y, width, height);
        }

        public void Update(Rectangle bounds)
        {
            this.X.Update(bounds.Width);
            this.Y.Update(bounds.Height);
            this.Width.Update(bounds.Width);
            this.Height.Update(bounds.Height);

            this.Top = this.Y;
            this.Right = this.X + this.Width;
            this.Bottom = this.Y + this.Height;
            this.Left = this.X;

            this.Output.X = this.X;
            this.Output.Y = this.Y;
            this.Output.Width = this.Width;
            this.Output.Height = this.Height;
        }

        public Boolean Contains(Vector2 value)
        {
            return Output.Contains(value);
        }

        #region Operators
        public static implicit operator UnitRectangle(Rectangle rect)
        {
            return new UnitRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        #endregion
    }
}
