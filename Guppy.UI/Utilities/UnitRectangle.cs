using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class UnitRectangle : IEquatable<UnitRectangle>
    {
        #region Private Fields
        private Rectangle _output;
        #endregion

        #region Public Attributes
        public Unit X { get; set; }
        public Unit Y { get; set; }
        public Unit Width { get; set; }
        public Unit Height { get; set; }

        public Rectangle Bounds { get { return _output; } }
        #endregion


        public UnitRectangle(Unit x, Unit y, Unit width, Unit height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            _output = new Rectangle(
                this.X.Value, 
                this.Y.Value, 
                this.Width.Value, 
                this.Height.Value);
        }

        public void UpdateBounds(Rectangle container)
        {
            this.X.UpdateValue(container.Width);
            this.Y.UpdateValue(container.Height);
            this.Width.UpdateValue(container.Width);
            this.Height.UpdateValue(container.Height);

            _output.X = container.X + this.X.Value;
            _output.Y = container.Y + this.Y.Value;
            _output.Width = this.Width.Value;
            _output.Height = this.Height.Value;
        }
        public void UpdateBounds(UnitRectangle container)
        {
            this.UpdateBounds(container.Bounds);
        }

        public bool Equals(UnitRectangle other)
        {
            return _output.Equals(other.Bounds);
        }
    }
}
