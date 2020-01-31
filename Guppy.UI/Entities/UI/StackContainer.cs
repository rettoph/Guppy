using Guppy.Extensions.Collection;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Entities.UI
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
                    this.dirty = true;
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
                        x: this.children.Sum(c => c.Bounds.Width.ToPixel(this.GetContainerBounds().Width)),
                        y: this.children.Max(c => c.Bounds.Height.ToPixel(this.GetContainerBounds().Height)));
                    break;
                case StackMethod.Vertical:
                    size = new Point(
                        x: this.children.Max(c => c.Bounds.Width.ToPixel(this.GetContainerBounds().Width)),
                        y: this.children.Sum(c => c.Bounds.Height.ToPixel(this.GetContainerBounds().Height)));
                    break;
            }

            if(this.Alignment != Alignment.None)
            {
                var pos = this.container.Align(size.ToVector2(), this.Alignment).ToPoint();
                this.Bounds.X = pos.X;
                this.Bounds.Y = pos.Y;
            }

            this.Bounds.Width = size.X;
            this.Bounds.Height = size.Y;



            // Update the position of all internal children
            Int32 offset = 0;
            this.children.ForEach(c =>
            {
                switch (this.Method)
                {
                    case StackMethod.Horizontal:
                        c.Bounds.X = offset;
                        offset += c.Bounds.Width.ToPixel(this.GetContainerBounds().Width);
                        break;
                    case StackMethod.Vertical:
                        c.Bounds.Y = offset;
                        offset += c.Bounds.Height.ToPixel(this.GetContainerBounds().Height);
                        break;
                }
            });            
        }
        #endregion

        #region Helper Methods
        protected override Rectangle GetBounds()
        {
            return this.Bounds.Pixel;

            return new Rectangle(this.Bounds.Pixel.Location, this.GetContainerBounds().Size);
        }

        protected override TElement add(TElement child)
        {
            child.OnBoundsChanged += this.HandleBoundsChanged;

            return base.add(child);
        }

        protected override void remove(TElement child)
        {
            child.OnBoundsChanged -= this.HandleBoundsChanged;

            base.remove(child);
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsChanged(object sender, Rectangle e)
        {
            this.dirty = true;
        }
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.primitiveBatch.DrawRectangle(this.Bounds.Pixel, Color.Blue);
        }
    }

    public class StackContainer : StackContainer<Element>
    {

    }
}
