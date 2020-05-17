using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Components
{
    public class StackContainer<TComponent> : Container<TComponent>
        where TComponent : IComponent
    {
        #region Private Fields
        private Boolean _dirty;
        #endregion

        #region Public Attributes
        public Direction Direction { get; set; }
        public Boolean Inline { get; set; } = false;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Children.OnAdded += this.HandleChildAdded;
            this.Children.OnRemoved += this.HandleChildRemoved;
        }
        #endregion

        #region Frame Methods
        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            this.TryClean();
        }
        #endregion

        #region Helper Methods
        protected void TryClean(Boolean force = false)
        {
            if(_dirty || force)
            {
                this.Clean();
                _dirty = false;
            }
        }

        protected virtual void Clean()
        {
            var pos = 0;
            switch (this.Direction)
            {
                case Direction.Horizontal:
                    this.Children.ForEach(c =>
                    {
                        c.Bounds.X = pos;
                        pos += c.Bounds.Pixel.Width;
                    });

                    this.Bounds.Width = pos;

                    if (this.Inline)
                        this.Bounds.Height = this.Children.Last().Bounds.Pixel.Bottom;

                    break;
                case Direction.Vertical:
                    this.Children.ForEach(c =>
                    {
                        c.Bounds.Y = pos;
                        pos += c.Bounds.Pixel.Height;
                    });

                    this.Bounds.Height = pos;

                    if (this.Inline)
                        this.Bounds.Width = this.Children.Max(c => c.Bounds.Pixel.Width);

                    break;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleChildAdded(object sender, TComponent e)
        {
            _dirty = true;
            e.Bounds.OnChanged += this.HandleChildBoundsChanged;
        }

        private void HandleChildRemoved(object sender, TComponent e)
        {
            _dirty = true;
            e.Bounds.OnChanged -= this.HandleChildBoundsChanged;
        }

        private void HandleChildBoundsChanged(object sender, Rectangle e)
        {
            _dirty = true;
        }
        #endregion

        #region IBaseContainer Overrides
        protected override Point GetContainerLocation()
            => base.GetContainerLocation();

        protected override Point GetContainerSize()
            => new Point()
                {
                    X = this.Direction == Direction.Horizontal ? this.Container.GetContainerSize().X : base.GetContainerSize().X,
                    Y = this.Direction == Direction.Vertical ? this.Container.GetContainerSize().Y : base.GetContainerSize().Y
                };
        #endregion
    }
}
