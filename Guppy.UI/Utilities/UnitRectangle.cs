using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class UnitRectangle
    {
        private Unit _y;
        private Unit _x;
        private Unit _width;
        private Unit _height;

        public Unit Y
        {
            get => _y;
            set {
                _y = value;
                this.OnPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit X
        {
            get => _x;
            set
            {
                _x = value;
                this.OnPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit Width
        {
            get => _width;
            set
            {
                _width = value;
                this.OnSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Unit Height
        {
            get => _height;
            set
            {
                _height = value;
                this.OnSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnPositionChanged;
        public event EventHandler OnSizeChanged;

        public UnitRectangle()
        {
            _y = 0;
            _x = 0;
            _width = 1f;
            _height = 1f;
        }
        public UnitRectangle(Unit x, Unit y, Unit width, Unit height)
        {
            _y = y;
            _x = x;
            _width = width;
            _height = height;
        }
        public Rectangle ToPixel(Rectangle parent)
        {
            return new Rectangle(
                x: parent.X + this.X.ToPixel(parent.Width),
                y: parent.Y + this.Y.ToPixel(parent.Height),
                width: this.Width.ToPixel(parent.Width),
                height: this.Height.ToPixel(parent.Height));
        }

        public void Set(Unit x = null, Unit y = null, Unit width = null, Unit height = null)
        {
            if(y != null)
                this.Y = y;
            if(x != null)
                this.X = x;
            if(width != null)
                this.Width = width;
            if(height != null)
                this.Height = height;
        }

        #region Reference Getters
        public void GetX(ref Unit x)
        {
            x = this.GetX();
        }

        public void GetY(ref Unit y)
        {
            y = this.GetY();
        }

        public void GetWidth(ref Unit width)
        {
            width = this.GetWidth();
        }

        public void GetHeight(ref Unit height)
        {
            height = this.GetHeight();
        }

        public ref Unit GetX()
        {
            return ref _x;
        }

        public ref Unit GetY()
        {
            return ref _y;
        }

        public ref Unit GetWidth()
        {
            return ref _width;
        }

        public ref Unit GetHeight()
        {
            return ref _height;
        }
        #endregion
    }
}
