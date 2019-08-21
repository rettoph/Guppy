using Guppy;
using Guppy.Attributes;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Client
{
    [IsGame]
    class PongGame : Guppy.Game
    {
        protected Scene scene;
        protected SpriteBatch spriteBatch;
        protected GraphicsDevice graphicsDevice;

        public PongGame(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.scene = this.scenes.Create<PongScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.scenes.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.graphicsDevice.Clear(Color.CornflowerBlue);


            this.scenes.TryDraw(gameTime);
        }
    }
}
