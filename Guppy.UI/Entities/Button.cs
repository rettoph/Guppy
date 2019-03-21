using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Configurations;
using Guppy.UI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Entities
{
    public class Button : Element
    {
        private SpriteBatch _spriteBatch;
        private ButtonConfiguration _configuration;

        public Button(String text, Rectangle bounds, SpriteBatch spriteBatch, EntityConfiguration configuration, Scene scene, ILogger logger, Alignment alignment = Alignment.TopLeft) : base(bounds, configuration, scene, logger, alignment)
        {
            _spriteBatch = spriteBatch;
            _configuration = configuration.Data as ButtonConfiguration;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            switch (this.State)
            {
                case ElementState.Normal:
                    _spriteBatch.Draw(_configuration.Texture, this.Bounds, _configuration.Texture.Bounds, Color.Wheat);
                    break;
                case ElementState.Hovered:
                    _spriteBatch.Draw(_configuration.HoverTexture, this.Bounds, _configuration.Texture.Bounds, Color.Wheat);
                    break;
                case ElementState.Active:
                    _spriteBatch.Draw(_configuration.ActiveTexture, this.Bounds, _configuration.Texture.Bounds, Color.Wheat);
                    break;
            }
        }
    }
}
