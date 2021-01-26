
using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Lists;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Elements
{
    public class Container<TChildren> : Element, IContainer<TChildren>
        where TChildren : class, IElement
    {
        #region Public Properties
        public ElementList<TChildren> Children { get; private set; }
        public InlineType Inline { get; set; }
        #endregion

        #region Events
        private event OnEventDelegate<IElement, GameTime> OnUpdateChild;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.OnBoundsCleaned += this.HandleBoundsCleaned;
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            // Create & setup a new children list.
            this.Children = provider.GetService<ElementList<TChildren>>();
            this.Children.OnAdded += this.HandleChildAdded;
            this.Children.OnRemoved += this.HandleChildRemoved;
            this.Children.OnCreated += this.HandleChildCreated;

            this.OnUpdateChild += this.UpdateChild;
            this.OnState[ElementState.Hovered] += this.HandleHoveredStateChanged;
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);
        }

        protected override void Release()
        {
            base.Release();

            // Release all internal children.
            while (this.Children.Any())
                this.Children.First().TryRelease();

            this.Children.OnAdded -= this.HandleChildAdded;
            this.Children.OnRemoved -= this.HandleChildRemoved;
            this.Children.OnCreated -= this.HandleChildCreated;
            this.Children.TryRelease();

            this.OnUpdateChild -= this.UpdateChild;
            this.OnState[ElementState.Hovered] -= this.HandleHoveredStateChanged;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnBoundsCleaned -= this.HandleBoundsCleaned;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.GetActiveChildren().TryDrawAll(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (TChildren child in this.GetActiveChildren())
                this.OnUpdateChild.Invoke(child, gameTime);
        }
        #endregion

        #region Methods
        protected override bool TryCleanInnerBounds()
        {
            if(base.TryCleanInnerBounds() || this.Inline != InlineType.None)
            {
                // Recursively clean all children.
                foreach (TChildren child in this.GetActiveChildren())
                    child.TryCleanBounds();

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        Rectangle IContainer.GetInnerBoundsForChildren()
            => this.GetInnerBoundsForChildren();

        /// <summary>
        /// Retrieve the bounds to pass into the current containers
        /// children when recalculating child bounds.
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetInnerBoundsForChildren()
        {
            if(this.Inline != 0)
            { // Calculate bounds based on padding...
                var container = this.Container?.GetInnerBoundsForChildren() ?? new Rectangle();
                var width = ((this.Inline & InlineType.Horizontal) == 0) ? this.InnerBounds.Width : container.Width - this.Padding.Left.ToPixel(container.Width) - this.Padding.Right.ToPixel(container.Width);
                var height = ((this.Inline & InlineType.Vertical) == 0) ? this.InnerBounds.Width : container.Height - this.Padding.Top.ToPixel(container.Height) - this.Padding.Bottom.ToPixel(container.Height);

                return new Rectangle(
                    x: this.InnerBounds.X,
                    y: this.InnerBounds.Y,
                    width: width,
                    height: height);
            }
            else
            {
                return this.InnerBounds;
            }
        }

        /// <summary>
        /// If <see cref="Inline"/> is true, this will resize the
        /// current element based on the size of the contained 
        /// children.
        /// </summary>
        protected void TryCleanInline()
        {
            if ((this.Inline & InlineType.Vertical) != 0 && this.Children.Any())
                this.Bounds.Height = new PixelUnit(this.Children.Max(c => c.OuterBounds.Bottom) - this.Children.Min(c => c.OuterBounds.Top), this.Padding.Top, this.Padding.Bottom);
            if((this.Inline & InlineType.Horizontal) != 0 && this.Children.Any())
                this.Bounds.Width = new PixelUnit(this.Children.Max(c => c.OuterBounds.Right) - this.Children.Min(c => c.OuterBounds.Left), this.Padding.Left, this.Padding.Right);
        }

        /// <summary>
        /// Return an IEnumerable of <see cref="TChildren"/> that are currently
        /// active. This is used when updating, drawing, or resizing embedded children.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<TChildren> GetActiveChildren()
            => this.Children;

        /// <inheritdoc />
        protected override void Refresh()
        {
            base.Refresh();

            this.Children.ForEach(c => c.Refresh());
            this.TryCleanInline();
        }
        #endregion

        #region Child Update Methods
        private void UpdateChild(IElement sender, GameTime arg)
            => sender.TryUpdate(arg);

        private void TryCleanChildHovered(IElement sender, GameTime arg)
            => sender.TryCleanHovered();
        #endregion

        #region Event Handlers
        private void HandleChildCreated(TChildren child)
        {
            child.Container = this;
        }

        private void HandleChildAdded(IServiceList<TChildren> sender, TChildren child)
        {
            if (child.Container != null && child.Container != this)
                throw new Exception($"Unable to add child, already resides within another container.");

            child.Container = this;
            child.TryCleanBounds();
            child.Bounds.OnChanged += this.HandleChildBoundsChanged;

            this.TryCleanInline();
        }

        private void HandleChildRemoved(IServiceList<TChildren> sender, TChildren child)
        {
            child.Container = null;
            child.Bounds.OnChanged -= this.HandleChildBoundsChanged;

            this.TryCleanInline();
        }

        private void HandleChildBoundsChanged(IElement child, Bounds bounds)
        {
            child.TryCleanBounds();
            this.TryCleanInline();
        }

        private void HandleHoveredStateChanged(IElement sender, ElementState which, bool value)
        {
            if (value)
                this.OnUpdateChild += this.TryCleanChildHovered;
            else
            {
                // Update all children hovered states one last time.
                foreach (TChildren child in this.Children)
                    child.TryCleanHovered();

                this.OnUpdateChild -= this.TryCleanChildHovered;
            }
        }

        /// <summary>
        /// Primary event to resize the element when size is inline.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleBoundsCleaned(Element sender)
            => this.TryCleanInline();
        #endregion
    }

    public class Container : Container<IElement>
    { 
    }
}
