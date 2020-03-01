using Guppy.Extensions.Collection;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Components
{
    /// <summary>
    /// An container designed to hold multiple children and automatically
    /// update their position to for a uniform 
    /// horizontal line
    /// </summary>
    public class StackContainer<TElement> : Container<TElement>
        where TElement : Element
    {
        #region Private Fields
        private StackMethod _method = StackMethod.Vertical;
        #endregion

        #region Public Attributes
        public StackMethod Method
        {
            get => _method;
            set
            {
                if(value != _method)
                {
                    _method = value;
                    this.Dirty = true;
                }
            }
        }
        public Alignment Alignment { get; set; } = Alignment.None;
        #endregion

        #region Clean Methods
        protected override void Clean()
        {
            // Clean all the children
            base.Clean();

            Point size = Point.Zero;
            switch (this.Method)
            {
                case StackMethod.Horizontal:
                    size = new Point(
                        x: this.children.Sum(c => c.Bounds.Width.ToPixel(this.Container.GetBounds().Width)),
                        y: this.children.Max(c => c.Bounds.Height.ToPixel(this.Container.GetBounds().Height)));
                    break;
                case StackMethod.Vertical:
                    size = new Point(
                        x: this.children.Max(c => c.Bounds.Width.ToPixel(this.Container.GetBounds().Width)),
                        y: this.children.Sum(c => c.Bounds.Height.ToPixel(this.Container.GetBounds().Height)));
                    break;
            }

            if(this.Alignment != Alignment.None)
            {
                var pos = this.Container.Align(size.ToVector2(), this.Alignment).ToPoint();
                this.Bounds.X = pos.X;
                this.Bounds.Y = pos.Y;
            }

            this.Bounds.Width = size.X;
            this.Bounds.Height = size.Y;
            this.Bounds.Clean();

            // Update the position of all internal children
            Int32 offset = 0;
            this.children.ForEach(c =>
            {
                switch (this.Method)
                {
                    case StackMethod.Horizontal:
                        c.Bounds.X = offset;
                        offset += c.Bounds.Pixel.Width;
                        break;
                    case StackMethod.Vertical:
                        c.Bounds.Y = offset;
                        offset += c.Bounds.Pixel.Height;
                        break;
                }
            });
        }
        #endregion
    }

    public class StackContainer : StackContainer<Element>
    {

    }
}
