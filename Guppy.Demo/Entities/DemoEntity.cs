using Guppy.Utilities.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Demo.Entities
{
    public class DemoEntity : Entity
    {
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private Random _random;

        public DemoEntity(SpriteBatch spriteBatch, ContentLoader content, Random random)
        {
            _spriteBatch = spriteBatch;
            _texture = content.Get<Texture2D>("texture:test");
            _random = random;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _position = new Vector2(_random.Next(0, 500), _random.Next(0, 500));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _position = new Vector2(_random.Next(0, 500), _random.Next(0, 500));
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}
