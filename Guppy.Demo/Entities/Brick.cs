using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Demo.Configurations;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Demo.Entities
{
    public class Brick : Entity
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;
        private Texture2D _texture;
        private Color _color;
        private BrickConfiguration _config;
        private Vector2 _position;

        public Brick(Vector2 position, ColorLoader colorLoader, ContentLoader contentLoader, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            _config = this.Configuration.Data as BrickConfiguration;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;

            _texture = contentLoader.Get<Texture2D>("texture:brick");
            _color = colorLoader.GetValue(_config.ColorHandle);
            _position = position * new Vector2(60, 30);
        }

        public Brick(Guid id, EntityConfiguration configuration, Scene scene, ILogger logger) : base(id, configuration, scene, logger)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Draw(_texture, _position, _color);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
