using Guppy;
using Guppy.Configurations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Library.Layers
{
    public class SimpleLayer : Layer
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDevice _graphicsDevice;

        public SimpleLayer(Scene scene, LayerConfiguration configuration, SpriteBatch spriteBatch = null, GameWindow window = null, GraphicsDevice graphicsDevice = null) : base(scene, configuration, window, graphicsDevice)
        {
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicsDevice;

            this.Debug = false;
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            this.entities.Draw(gameTime);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            this.entities.Update(gameTime);
        }
    }
}
