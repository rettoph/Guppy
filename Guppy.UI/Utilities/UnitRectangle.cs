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

        public Rectangle Output { get { return _output; } }
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

        public void UpdateOutput(Rectangle bounds)
        {
            this.X.UpdateValue(bounds.Width);
            this.Y.UpdateValue(bounds.Height);
            this.Width.UpdateValue(bounds.Width);
            this.Height.UpdateValue(bounds.Height);

            _output.X = this.X.Value;
            _output.Y = this.Y.Value;
            _output.Width = this.Width.Value;
            _output.Height = this.Height.Value;
        }
        public void UpdateOutput(UnitRectangle bounds)
        {
            this.UpdateOutput(bounds.Output);
        }

        public bool Equals(UnitRectangle other)
        {
            return _output.Equals(other.Output);
        }
    }
}
