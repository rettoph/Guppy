using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions;
using Guppy.UI.Configurations;
using Guppy.UI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Entities
{
    public class Button : Element
    {
        private SpriteBatch _spriteBatch;
        private ButtonConfiguration _configuration;
        private String _text;

        private Vector2 _textPosition;

        public Button(String text, Rectangle bounds, InputManager inputManager, SpriteBatch spriteBatch, EntityConfiguration configuration, Scene scene, ILogger logger, Alignment alignment = Alignment.TopLeft) : base(bounds, inputManager, configuration, scene, logger, alignment)
        {
            _text = text;
            _spriteBatch = spriteBatch;
            _configuration = configuration.Data as ButtonConfiguration;

            this.ConfigureString();
        }

        /// <summary>
        /// Configure the string mapping based on current
        /// bounds and text value
        /// </summary>
        private void ConfigureString()
        {
            var stringBounds = _configuration.Font.MeasureString(_text);

            _textPosition = new Vector2(this.Bounds.X + ((this.Bounds.Width / 2) - (stringBounds.X / 2)), this.Bounds.Y + ((this.Bounds.Height / 2) - (stringBounds.Y / 2)));
        }

        public override void Draw(GameTime gameTime)
        {
            switch (this.State)
            {
                case ElementState.Normal:
                    _spriteBatch.Draw(_configuration.Texture, this.Bounds, _configuration.Texture.Bounds, Color.White);
                    break;
                case ElementState.Hovered:
                    _spriteBatch.Draw(_configuration.HoverTexture, this.Bounds, _configuration.Texture.Bounds, Color.White);
                    break;
                case ElementState.Active:
                    _spriteBatch.Draw(_configuration.ActiveTexture, this.Bounds, _configuration.Texture.Bounds, Color.White);
                    break;
            }

            // Draw the text afterwards
            _spriteBatch.DrawString(_configuration.Font, _text, _textPosition, Color.Yellow);
        }
    }
}
