using Guppy.UI.Entities.UI;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    public class ElementBounds : UnitRectangle
    {
        #region Private Fields
        private BaseElement _parent;
        #endregion

        #region Public Fields 
        public Rectangle Pixel;
        #endregion

        #region Constructor
        internal ElementBounds(BaseElement parent)
        {
            _parent = parent;
        }
        internal ElementBounds(BaseElement parent, Unit top, Unit left, Unit width, Unit height) : base(top, left, width, height)
        {
            _parent = parent;
        }
        #endregion

        #region Methods
        protected internal virtual void Clean()
        {
            // Generate a new pixel rectangle based on the parent element's parent's bounds.
            this.Pixel = this.ToPixel(_parent.GetParentBounds());
        }

        public Boolean Contains(Vector2 point)
        {
            return this.Pixel.Contains(point);
        }
        #endregion

        #region Operators
        public static implicit operator Rectangle(ElementBounds bounds)
        {
            return bounds.Pixel;
        }
        #endregion
    }
}
