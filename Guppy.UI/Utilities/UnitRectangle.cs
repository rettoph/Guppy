using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class UnitRectangle
    {
        private Unit _top;
        private Unit _left;
        private Unit _width;
        private Unit _height;

        public Unit Top
        {
            get => _top;
            set {
                _top = value;
                this.OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit Left
        {
            get => _left;
            set
            {
                _left = value;
                this.OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit Width
        {
            get => _width;
            set
            {
                _width = value;
                this.OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit Height
        {
            get => _height;
            set
            {
                _height = value;
                this.OnChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Unit Right
        {
            get => new NestedUnit(this.Left, this.Width);
            set => this.Left = new NestedUnit(value, this.Width.Flip());
        }
        public Unit Bottom
        {
            get => new NestedUnit(this.Top, this.Height);
            set => this.Top = new NestedUnit(value, this.Height.Flip());
        }

        public event EventHandler OnChanged;

        public UnitRectangle()
        {
            _top = 0;
            _left = 0;
            _width = 1f;
            _height = 1f;
        }
        public UnitRectangle(Unit top, Unit left, Unit width, Unit height)
        {
            _top = top;
            _left = left;
            _width = width;
            _height = height;
        }
        public Rectangle ToPixel(Rectangle parent)
        {
            return new Rectangle(
                x: parent.X + this.Left.ToPixel(parent.Width),
                y: parent.Y + this.Top.ToPixel(parent.Height),
                width: this.Width.ToPixel(parent.Width),
                height: this.Height.ToPixel(parent.Height));
        }
    }
}
