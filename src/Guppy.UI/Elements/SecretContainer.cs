using Guppy.DependencyInjection;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Elements
{
    public abstract class SecretContainer<TElement, TContainer> : Element, IContainer
        where TElement : class, IElement
        where TContainer : class, IElement, IContainer<TElement>
    {
        #region Private Fields
        private TContainer _inner;
        #endregion

        #region Protected Fields
        protected TContainer inner => _inner;
        protected InlineType inline
        {
            get => _inner.Inline;
            set => _inner.Inline = value;
        }
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

            _inner = this.BuildContainer(provider);
            _inner.Container = this;
        }

        protected override void Release()
        {
            base.Release();

            _inner?.TryRelease();
            _inner = default;
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

            _inner.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _inner.TryCleanHovered();
            _inner.TryUpdate(gameTime);
        }
        #endregion

        #region IContainer Implementation
        Rectangle IContainer.GetInnerBoundsForChildren()
            => this.GetInnerBoundsForChildren();

        protected virtual Rectangle GetInnerBoundsForChildren()
        {
            if (this.inline != 0)
            { // Calculate bounds based on padding...
                var container = this.Container.GetInnerBoundsForChildren();
                var width = (this.inline & InlineType.Horizontal) == 0 ? this.InnerBounds.Width : container.Width - this.Padding.Left.ToPixel(container.Width) - this.Padding.Right.ToPixel(container.Width);
                var height = (this.inline & InlineType.Vertical) == 0 ? this.InnerBounds.Height : container.Height - this.Padding.Top.ToPixel(container.Height) - this.Padding.Bottom.ToPixel(container.Height);

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
        #endregion

        #region Helper Methods
        protected virtual TContainer BuildContainer(ServiceProvider provider)
            => provider.GetService<TContainer>();
        #endregion

        #region Event Handlers
        private void HandleBoundsCleaned(Element sender)
        {
            _inner.TryCleanBounds();

            if ((this.inline & InlineType.Vertical) != 0)
                this.Bounds.Height = new MultiUnit(_inner.Bounds.Height, this.Padding.Top, this.Padding.Bottom);
            if ((this.inline & InlineType.Horizontal) != 0)
                this.Bounds.Width = new MultiUnit(_inner.Bounds.Width, this.Padding.Left, this.Padding.Right);
        }
        #endregion
    }

    public abstract class SecretContainer<TChildren> : SecretContainer<TChildren, Container<TChildren>>
        where TChildren : class, IElement
    { }
}
