using Guppy.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.UI.Entities;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class Component : Frameable, IComponent
    {
        #region Private Fields
        private PrimitiveBatch _primitiveBatch;
        private Indicator _indicator;
        private Boolean _active;
        private Boolean _hovered;
        #endregion

        #region Public Atttributes
        public IElement Container { get; set; }
        public UnitRectangle Bounds { get; private set; }
        public virtual Boolean Active
        {
            get => _active;
            set
            {
                if(value != _active)
                {
                    _active = value;
                    this.OnActiveChanged?.Invoke(this, _active);
                }
            }
        }
        public virtual Boolean Hovered
        {
            get => _hovered;
            set
            {
                if (value != _hovered)
                {
                    _hovered = value;
                    this.OnHoveredChanged?.Invoke(this, _hovered);
                }
            }
        }
        public SpriteBatch SpriteBatch { get => this.Container.SpriteBatch; }
        #endregion

        #region Events
        public event EventHandler<Boolean> OnActiveChanged;
        public event EventHandler<Boolean> OnHoveredChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _primitiveBatch);
            provider.Service(out _indicator);

            this.Bounds = new UnitRectangle();
        }
        #endregion

        #region Frame Methods
        public override void TryDraw(GameTime gameTime)
        {
            if (this.Container.Bounds.Pixel.Intersects(this.Bounds.Pixel))
                base.TryDraw(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _primitiveBatch.DrawRectangle(this.Bounds.Pixel, this.Hovered ? Color.Blue : Color.Red);
        }

        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            // Clean the internal bounds if needed...
            this.Bounds.TryClean(this.Container.Bounds.Pixel);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(this.Hovered || this.Container.Hovered) // Update the internal hovered value.
                this.Hovered = this.Bounds.Pixel.Contains(_indicator.Position);
        }
        #endregion
    }
}
