using Guppy;
using Guppy.Configurations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Layers
{
    public class SimpleLayer : Layer
    {
        private SpriteBatch _spriteBatch;

        public SimpleLayer(SpriteBatch spriteBatch, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
        {
            _spriteBatch = spriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            this.entities.Draw(gameTime);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            this.entities.Update(gameTime);
        }
    }
}
