using Guppy.UI.Components;
using Guppy.UI.Components.Interfaces;
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
        private IElement _parent;
        #endregion

        #region Public Fields 
        public Rectangle Pixel;
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
        public void TryUpdate(GameTime gameTime)
        {
            this.Pixel.Location = new Point(this.X.ToPixel(_parent.Container.GetBounds().Width), this.Y.ToPixel(_parent.Container.GetBounds().Height));
        }

        protected internal virtual void Clean()
        {
            // Generate a new pixel rectangle based on the parent element's parent's bounds.
            this.Pixel = this.ToPixel(_parent.Container.GetBounds());
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
