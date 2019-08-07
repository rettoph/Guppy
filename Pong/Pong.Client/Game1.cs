using Guppy;
using Guppy.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Network.Extensions;
using Game = Microsoft.Xna.Framework.Game;
using Lidgren.Network;
using Pong.Library;

namespace Pong.Client
{
    public class Game1 : Game
    {
        private GuppyLoader _guppy;
        private GraphicsDeviceManager _graphics;
        private PongGame _game;

        public Game1()
        {
            _guppy = new GuppyLoader();
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();

            _guppy.ConfigureMonoGame(_graphics, this.Window, this.Content);
            _guppy.ConfigureClient(new NetPeerConfiguration("pong"));
            _guppy.Initialize();

            _game = _guppy.BuildGame<ClientPongGame>();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _game.TryUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _game.TryDraw(gameTime);
        }
    }
}
