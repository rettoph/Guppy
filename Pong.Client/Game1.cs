using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client
{
    class Game1 : Game
    {
        private ClientPongGame _pongGame;

        public Game1()
        {
            _pongGame = new ClientPongGame(new GraphicsDeviceManager(this), this.Window, this.Content);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _pongGame.Start();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _pongGame.Draw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _pongGame.Update(gameTime);
        }
    }
}
