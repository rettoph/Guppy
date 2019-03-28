using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Client.Layers
{
    public class DebugLayer : Layer
    {
        private SpriteBatch _spriteBatch;

        public DebugLayer(SpriteBatch spriteBatch, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
        {
            _spriteBatch = spriteBatch;

            this.entities.Added += this.HandleEntityAdded;
        }

        private void HandleEntityAdded(object sender, Entity e)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            this.entities.Draw(gameTime);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
