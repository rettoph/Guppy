using Guppy.Extensions.Collection;
using Guppy.UI.Components.Interfaces;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Guppy.UI.Components
{
    /// <summary>
    /// Defines the minimum templating required for a UI element.
    /// </summary>
    public abstract class Element : Configurable, IElement
    {
        #region Protected Attributes
        protected Pointer pointer { get; private set; }
        protected SpriteBatch spriteBatch { get; private set; }
        protected PrimitiveBatch primitiveBatch { get; private set; }
        #endregion

        #region Public Attributes
        /// <inheritdoc />
        public ElementBounds Bounds { get; private set; }
        /// <inheritdoc />
        public Boolean Hovered { get; private set; }
        /// <inheritdoc />
        public Boolean Active { get; private set; }
        /// <inheritdoc />
        public Pointer.Button Buttons { get; private set; }
        public IBaseElement Container { get; set; }
        public Boolean Dirty { get; set; }
        #endregion

        #region Events
        public event EventHandler<Boolean> OnHoveredChanged;
        public event EventHandler<Boolean> OnActiveChanged;
        public event EventHandler<Pointer.Button> OnButtonPressed;
        public event EventHandler<Pointer.Button> OnButtonReleased;
        public event EventHandler<Rectangle> OnBoundsChanged;
        #endregion

        #region Lifecycle Methods 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Bounds = new ElementBounds(this);

            this.pointer = provider.GetRequiredService<Pointer>();
            this.spriteBatch = provider.GetRequiredService<SpriteBatch>();
            this.primitiveBatch = provider.GetRequiredService<PrimitiveBatch>();
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.Enabled = true;
            this.Visible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Dirty = true;

            this.Bounds.OnPositionChanged += this.HandleBoundsChanged;
            this.Bounds.OnSizeChanged += this.HandleBoundsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Bounds.OnPositionChanged -= this.HandleBoundsChanged;
            this.Bounds.OnSizeChanged -= this.HandleBoundsChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // this.primitiveBatch.DrawRectangle(this.Bounds.Pixel, Color.Red);
        }

        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            this.TryClean();

            // Update the hovered value as needed..
            if ((this.Hovered || this.Container == null || this.Container.Hovered) && this.Hovered != (this.Hovered = this.GetHovered()))
                this.OnHoveredChanged?.Invoke(this, this.Hovered);

            // Update the current active state of the element.
            if (!this.Active && this.Hovered && (this.Buttons & this.pointer.Released & Pointer.Button.Left) != 0)
            {
                this.Active = true;
                this.OnActiveChanged?.Invoke(this, this.Active);
            } 
            else if (this.Active && !this.Hovered && (this.pointer.Pressed & Pointer.Button.Left) != 0)
            {
                this.Active = false;
                this.OnActiveChanged?.Invoke(this, this.Active);
            }
        }

        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            if (this.Hovered && this.pointer.Pressed != 0)
            { // If the element is hovered check for button presses...
                this.CheckButtonPressed(Pointer.Button.Left);
                this.CheckButtonPressed(Pointer.Button.Middle);
                this.CheckButtonPressed(Pointer.Button.Right);
            }

            if (this.Buttons != 0 && this.pointer.Released != 0)
            { // If there are any buttons currently pressed on the element...
                this.CheckButtonReleased(Pointer.Button.Left);
                this.CheckButtonReleased(Pointer.Button.Middle);
                this.CheckButtonReleased(Pointer.Button.Right);
            }
        }
        #endregion

        #region Check Event Methods
        private void CheckButtonPressed(Pointer.Button button)
        {
            if((this.pointer.Pressed & ~this.Buttons & button) != 0)
            {
                this.Buttons |= button;
                this.OnButtonPressed?.Invoke(this, button);
            }
        }

        private void CheckButtonReleased(Pointer.Button button)
        {
            if((this.pointer.Released & this.Buttons & button) != 0)
            {
                this.Buttons &= ~button;
                this.OnButtonReleased?.Invoke(this, button);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Calculate the current hovered state of the element
        /// </summary>
        /// <returns></returns>
        protected virtual Boolean GetHovered()
        {
            return this.Bounds.Pixel.Contains(this.pointer.Position);
        }

        public virtual Rectangle GetBounds()
        {
            return this.Bounds.Pixel;
        }
        #endregion

        #region Clean Methods
        ///<inheritdoc />
        public void TryClean(Boolean force = false)
        {
            if(this.Dirty || force)
            {
                this.Clean();
                this.Dirty = false;
            }
        }

        protected virtual void Clean()
        {
            var old = this.Bounds.Pixel;
            this.Bounds.Clean();
            if (old != this.Bounds.Pixel)
                this.OnBoundsChanged?.Invoke(this, this.Bounds.Pixel);
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsChanged(object sender, EventArgs e)
        {
            this.Dirty = true;
        }
        #endregion
    }
}
