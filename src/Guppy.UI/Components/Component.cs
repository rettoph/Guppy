using Guppy.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.UI.Entities;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Backgrounds;
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
        private ServiceProvider _provider;
        private GraphicsDevice _graphics;
        private PrimitiveBatch _primitiveBatch;
        private Cursor _cursor;
        private Boolean _active;
        private Boolean _hovered;
        private UnitRectangle _bounds;
        private Background _background;
        #endregion

        #region Protected Fields
        protected SpriteBatch spriteBatch;
        #endregion

        #region Public Atttributes
        public IBaseContainer Container { get; set; }
        public UnitRectangle Bounds
        {
            get => _bounds;
            private set => _bounds = value;
        }
        public Cursor.Button Buttons { get; private set; }
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
        public Background Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    _background.Setup(_provider, this);
                    this.OnBackgroundChanged?.Invoke(this, _background);
                }
            }
        }
        #endregion

        #region Events
        public event EventHandler<Boolean> OnActiveChanged;
        public event EventHandler<Boolean> OnHoveredChanged;
        public event EventHandler<Cursor.Button> OnButtonPressed;
        public event EventHandler<Cursor.Button> OnButtonReleased;
        public event EventHandler<Background> OnBackgroundChanged;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            provider.Service(out _graphics);
            provider.Service(out _primitiveBatch);
            provider.Service(out _cursor);
            provider.Service(out spriteBatch);

            this.Bounds = new UnitRectangle();
        }
        #endregion

        #region Frame Methods
        public override void TryDraw(GameTime gameTime)
        {
            if (this.Container.Bounds.Pixel.Intersects(this.Bounds.Pixel))
                base.TryDraw(gameTime);
        }

        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            this.Background?.Draw(gameTime);
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
            this.Bounds.TryClean(this.Container.GetContainerLocation(), this.Container.GetContainerSize());
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(this.Hovered || this.Container.Hovered) // Update the internal hovered value.
                this.Hovered = this.Bounds.Pixel.Contains(_cursor.Position);

            // Update the current active state
            if(!this.Active && this.Hovered && (this.Buttons & _cursor.Released & Cursor.Button.Left) != 0)
                this.Active = true;
            else if(this.Active && !this.Hovered && (_cursor.Pressed & Cursor.Button.Left) != 0)
                this.Active = false;
        }

        protected override void PostUpdate(GameTime gameTime)
        {
            base.PostUpdate(gameTime);

            // Update the cursor buttons
            if(this.Hovered && _cursor.Pressed != 0)
            { // If the element is hovered check for button pressed...
                this.CheckButtonPressed(Cursor.Button.Left);
                this.CheckButtonPressed(Cursor.Button.Middle);
                this.CheckButtonPressed(Cursor.Button.Right);
            }

            if(this.Buttons != 0 && _cursor.Released != 0)
            { // If there are any buttons being pressed...
                this.CheckButtonReleased(Cursor.Button.Left);
                this.CheckButtonReleased(Cursor.Button.Middle);
                this.CheckButtonReleased(Cursor.Button.Right);
            }
        }
        #endregion

        #region Check Event Methods
        private void CheckButtonPressed(Cursor.Button button)
        {
            if ((_cursor.Pressed & ~this.Buttons & button) != 0)
            {
                this.Buttons |= button;
                this.OnButtonPressed?.Invoke(this, button);
            }
        }

        private void CheckButtonReleased(Cursor.Button button)
        {
            if ((_cursor.Released & this.Buttons & button) != 0)
            {
                this.Buttons &= ~button;
                this.OnButtonReleased?.Invoke(this, button);
            }
        }
        #endregion
    }
}
