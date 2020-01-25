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
        private Element _parent;
        #endregion

        #region Protected Fields 
        protected Rectangle pixel;
        #endregion

        #region Constructor
        internal ElementBounds(Element parent)
        {
            _parent = parent;
        }
        internal ElementBounds(Element parent, Unit top, Unit left, Unit width, Unit height) : base(top, left, width, height)
        {
            _parent = parent;
        }
        #endregion

        #region Methods
        protected internal virtual void Clean()
        {
            // Generate a new pixel rectangle based on the parent element's parent's bounds.
            this.pixel = this.ToPixel(_parent is Stage ? (_parent as Stage).ViewportBounds : _parent.Parent.Bounds);
        }
        #endregion

        #region Operators
        public static implicit operator Rectangle(ElementBounds bounds)
        {
            return bounds.pixel;
        }
        #endregion
    }
}
