using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public sealed class UnitRectangle
    {
        #region Private Fields
        private Rectangle _container;
        private Rectangle _pixel;
        private Unit _x = 0;
        private Unit _y = 0;
        private Unit _width = 1f;
        private Unit _height = 1f;
        private Boolean _dirty;
        #endregion

        #region Public Attributes
        public Unit X
        {
            get => _x;
            set
            {
                _x = value;
                _dirty = true;
            }
        }
        public Unit Y
        {
            get => _y;
            set
            {
                _y = value;
                _dirty = true;
            }
        }
        public Unit Width
        {
            get => _width;
            set
            {
                _width = value;
                _dirty = true;
            }
        }
        public Unit Height
        {
            get => _height;
            set
            {
                _height = value;
                _dirty = true;
            }
        }

        public Rectangle Pixel
        {
            get => _pixel;
            set
            {
                if(_pixel != value)
                { // Only update the value if its new
                    _pixel = value;
                    this.OnChanged?.Invoke(this, _pixel);
                }
            }
        }

        public Unit Left
        {
            set => this.X = value;
        }
        public Unit Top
        {
            set => this.Y = value;
        }
        public Unit Right
        {
            set => this.Width = 1f - this.X - value;
        }
        public Unit Bottom
        {
            set => this.Height = 1f - this.Y - value;
        }
        #endregion

        #region Events
        public event EventHandler<Rectangle> OnChanged;
        #endregion

        #region Helper Methods
        public void TryClean(Rectangle container)
        {
            if(_container != container || _dirty)
            {
                this.Pixel = new Rectangle()
                {
                    X = container.X + this.X.ToPixel(container.Width),
                    Y = container.Y + this.Y.ToPixel(container.Height),
                    Width = this.Width.ToPixel(container.Width),
                    Height = this.Height.ToPixel(container.Height)
                };

                _container = container;
                _dirty = false;
            }
        }

        public void Set(Unit x, Unit y, Unit width, Unit height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }
        #endregion
    }
}
