using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.UI.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Image : StaticElement
    {
        private Texture2D _source;
        private RenderTarget2D _texture;

        public Image(String textureHandle, IServiceProvider provider, Rectangle bounds, InputManager inputManager, SpriteBatch spriteBatch, GameWindow window, GraphicsDevice graphiceDevice, EntityConfiguration configuration, Scene scene, ILogger logger) : base(bounds, inputManager, spriteBatch, window, graphiceDevice, configuration, scene, logger, "")
        {
            _source = provider.GetLoader<ContentLoader>().Get<Texture2D>(textureHandle);
        }

        public Image(String textureHandle, IServiceProvider provider, UnitRectangle bounds, InputManager inputManager, SpriteBatch spriteBatch, GameWindow window, GraphicsDevice graphiceDevice, EntityConfiguration configuration, Scene scene, ILogger logger) : base(bounds, inputManager, spriteBatch, window, graphiceDevice, configuration, scene, logger, "")
        {
            _source = provider.GetLoader<ContentLoader>().Get<Texture2D>(textureHandle);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.spriteBatch.Draw(_texture, this.Bounds.Output, Color.White);
        }

        protected override void GenerateTextures()
        {
            base.GenerateTextures();

            // Destroy the old one...
            _texture?.Dispose();

            // Save current render targets...
            var originalRenderTargets = this.graphicsDevice.GetRenderTargets();
            var targetBounds = new Rectangle(0, 0, this.Bounds.Width, this.Bounds.Height);

            _texture = new RenderTarget2D(this.graphicsDevice, this.Bounds.Width, this.Bounds.Height);
            this.graphicsDevice.SetRenderTarget(_texture);

            // Begin drawing the overlay
            this.internalSpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
            this.internalSpriteBatch.Draw(_source, targetBounds, targetBounds, Color.White);
            this.internalSpriteBatch.End();

            // reset the graphics device render targets
            this.graphicsDevice.SetRenderTargets(originalRenderTargets);
        }
    }
}
