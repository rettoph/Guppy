using Guppy;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        GuppyLoader guppy;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.guppy = new GuppyLoader();

           //this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;

            this.graphics.PreferredBackBufferHeight = 600;
            this.graphics.PreferredBackBufferWidth = 900;
        }

        protected override void Initialize()
        {
            base.Initialize();

            guppy.Initialize();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
