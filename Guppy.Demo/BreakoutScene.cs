using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Demo.Layers;
using Guppy.Extensions;
using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Demo
{
    public class BreakoutScene : Scene
    {
        private GraphicsDevice _graphicsDevice;
        private RenderTarget2D _target;
        private SpriteBatch _spriteBatch;
        private GameWindow _window;

        public BreakoutScene(GameWindow window, SpriteBatch spriteBatch, GraphicsDevice graphicDevice, IServiceProvider provider) : base(provider)
        {
            _window = window;
            _spriteBatch = spriteBatch;
            _graphicsDevice = graphicDevice;
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.layers.Create<BrickLayer>();

            _target = new RenderTarget2D(_graphicsDevice, 1200, 900);
        }
        protected override void Initialize()
        {
            base.Initialize();

            for (Int32 x=0; x<20; x++)
            {
                for(Int32 y=0; y<20; y++)
                {
                    this.entities.Create("entity:brick:red", new Vector2(x, y));
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.SetRenderTarget(_target);
            _graphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            _graphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_target, new Rectangle(0, 0, _window.ClientBounds.Width, _window.ClientBounds.Height), Color.White);
            _spriteBatch.End();
        }
    }
}
