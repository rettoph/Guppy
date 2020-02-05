using Guppy.Extensions.Collection;
using Guppy.UI.Entities.UI.Interfaces;
using Guppy.UI.Enums;
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
        public Boolean Active { get; private set; }
        /// <inheritdoc />
        public Pointer.Button Buttons { get; private set; }

        public Boolean Hidden { get; set; }
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
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.SetEnabled(false);
            this.SetVisible(false);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.dirty = true;

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
        public override void TryDraw(GameTime gameTime)
        {
            if(!this.Hidden)
                base.TryDraw(gameTime);
        }

        public override void TryUpdate(GameTime gameTime)
        {
            if(!this.Hidden)
                base.TryUpdate(gameTime);
        }

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
            if ((this.Hovered || this.container == null || this.container.Hovered) && this.Hovered != (this.Hovered = this.GetHovered()))
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
        /// Internal method used to grab the current elements bounds.
        /// 
        /// These bounds will be used within the element's children
        /// to represent the container bounds.
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle GetBounds()
        {
            return this.Bounds.Pixel;
        }

        ///<inheritdoc />
        public virtual Rectangle GetContainerBounds()
        {
            return this.container.GetBounds();
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
            var old = this.GetBounds();
            this.Bounds.Clean();
            if (old != this.GetBounds())
                this.OnBoundsChanged?.Invoke(this, this.GetBounds());
        }
        #endregion

        #region Align Methods
        /// <summary>
        /// Aligns the given rectangle in the requested alignment method.
        /// 
        /// This will update the rectangles internal position.
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public void Align(ref Rectangle rectangle, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            // Default to Top Left alignment...
            Point position = useWorldCoordinates ? this.Bounds.Pixel.Location : Point.Zero;

            // Vertical Alignment...
            if ((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                position.Y += this.Bounds.Pixel.Height - rectangle.Height;
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                position.Y += (this.Bounds.Pixel.Height - rectangle.Height) / 2;
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                position.X += this.Bounds.Pixel.Width - rectangle.Width;
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                position.X += (this.Bounds.Pixel.Width - rectangle.Width) / 2;
            }

            // Update the recieved rectangles position.
            rectangle.Location = position;
        }
        /// <summary>
        /// Returns a rectangle aligned to the current element with the
        /// requested alignment type
        /// </summary>
        /// <param name="rectangle">The rectangle to align.</param>
        /// <param name="alignment">The requested alignment type. Default is Top Left</param>
        /// <param name="useWorldCoordinates">Whether or not the response should be in local or world coords.</param>
        public Rectangle Align(Rectangle rectangle, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            this.Align(ref rectangle, alignment, useWorldCoordinates);
            return rectangle;
        }

        public Vector2 Align(Vector2 size, Alignment alignment, Boolean useWorldCoordinates = false)
        {
            // Default to Top Left alignment...
            Vector2 position = useWorldCoordinates ? this.Bounds.Pixel.Location.ToVector2() : Vector2.Zero;

            // Vertical Alignment...
            if ((alignment & Alignment.Bottom) != 0)
            { // Bottom align...
                position.Y += (Int32)(this.Bounds.Pixel.Height - size.Y);
            }
            else if ((alignment & Alignment.VerticalCenter) != 0)
            { // VerticalCenter align...
                position.Y += (Int32)((this.Bounds.Pixel.Height - size.Y) / 2);
            }

            // Horizontal Alignment
            if ((alignment & Alignment.Right) != 0)
            { // Right align...
                position.X += (Int32)(this.Bounds.Pixel.Width - size.X);
            }
            else if ((alignment & Alignment.HorizontalCenter) != 0)
            { // HorizontalCenter align...
                position.X += (Int32)((this.Bounds.Pixel.Width - size.X) / 2);
            }

            position.X = (Int32)position.X;
            position.Y = (Int32)position.Y;

            return position;
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsChanged(object sender, EventArgs e)
        {
            this.dirty = true;
        }
        #endregion
    }
}
