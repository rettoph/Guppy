using Guppy.Extensions.Collection;
using Guppy.UI.Entities.UI.Interfaces;
using Guppy.UI.Extensions;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Defines the minimum templating required for a UI element.
    /// </summary>
    public abstract class Element : Entity, IElement
    {
        #region Protected Fields
        protected virtual SpriteBatch spriteBatch {
            get => this.container.spriteBatch;
            set => this.container.spriteBatch = value;
        }
        protected virtual PrimitiveBatch primitiveBatch
        {
            get => this.container.primitiveBatch;
            set => this.container.primitiveBatch = value;
        }
        protected virtual Pointer pointer => this.container.pointer;
        protected internal Boolean dirty { get; set; }
        protected internal Element container { get; set; }
        #endregion

        #region Public Attributes
        /// <inheritdoc />
        public ElementBounds Bounds { get; private set; }
        /// <inheritdoc />
        public Boolean Hovered { get; private set; }
        /// <inheritdoc />
        public Pointer.Button Buttons { get; private set; }
        #endregion

        #region Events
        public event EventHandler<Boolean> OnHoveredChanged;
        public event EventHandler<Pointer.Button> OnButtonPressed;
        public event EventHandler<Pointer.Button> OnButtonReleased;
        #endregion

        #region Lifecycle Methods 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.Bounds = new ElementBounds(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.dirty = true;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.primitiveBatch.DrawRectangle(this.Bounds.Pixel, Color.Red);
        }

        protected override void PreUpdate(GameTime gameTIme)
        {
            base.PreUpdate(gameTIme);

            this.TryClean();

            // Update the hovered value as needed..
            if ((this.Hovered || this.container == null || this.container.Hovered) && this.Hovered != (this.Hovered = this.GetHovered()))
                this.OnHoveredChanged?.Invoke(this, this.Hovered);
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
        ///<inheritdoc />
        public virtual Rectangle GetContainerBounds()
        {
            return this.container.Bounds.Pixel;
        }

        /// <summary>
        /// Calculate the current hovered state of the element
        /// </summary>
        /// <returns></returns>
        protected virtual Boolean GetHovered()
        {
            return this.Bounds.Pixel.Contains(this.pointer.Position);
        }
        #endregion

        #region Clean Methods
        ///<inheritdoc />
        public void TryClean(Boolean force = false)
        {
            if(this.dirty || force)
            {
                this.Clean();
                this.dirty = false;
            }
        }

        protected virtual void Clean()
        {
            this.Bounds.Clean();
        }
        #endregion
    }
}
