using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Collections;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities
{
    public class Stage : Entity, IContainer
    {
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphics;
        private SpriteBatch _defaultSpriteBatch;
        #endregion

        #region Public Attributes
        public SpriteBatch SpriteBatch { get; set; }
        public ComponentCollection Children { get; private set; }
        public UnitRectangle Bounds { get; private set; }
        public Boolean Hovered => true;
        public Boolean Active => true;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _defaultSpriteBatch);
            provider.Service(out _window);
            provider.Service(out _graphics);

            this.Children = provider.GetService<ComponentCollection>();
            this.Bounds = new UnitRectangle()
            {
                X = 0,
                Y = 0,
                Width = 1f,
                Height = 1f
            };
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Children.Parent = this;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.SpriteBatch = _defaultSpriteBatch;
            this.Children.ForEach(c => c.TryDraw(gameTime));
        }

        protected override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);

            // Clean the internal bounds if needed...
            this.Bounds.TryClean(_graphics.Viewport.Bounds);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Children.ForEach(c => c.TryUpdate(gameTime));
        }
        #endregion
    }
}
