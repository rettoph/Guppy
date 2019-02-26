using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Demo
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics { get; private set; }
        protected DemoGame demoGame;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;

            this.Window.AllowUserResizing = true;

            this.Content.RootDirectory = "Content";

            this.demoGame = new DemoGame(this.Window, this.Content, this.graphics);
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.demoGame.Start();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.demoGame.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.demoGame.Update(gameTime);
        }
    }
}
