using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Demo.Layers
{
    public class BrickLayer : Layer
    {
        private SpriteBatch _spriteBatch;

        public BrickLayer(SpriteBatch spriteBatch, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
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
            // throw new NotImplementedException();
        }
    }
}
