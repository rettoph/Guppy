using Guppy;
using Guppy.Loggers;
using Guppy.Network.Extensions.Guppy;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Pong.Client
{
    class Game1 : Microsoft.Xna.Framework.Game
    {
        private GuppyLoader _guppy;
        private ClientPongGame _pongGame;
        private GraphicsDeviceManager _graphics;

        public Game1()
        {
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;

            _graphics = new GraphicsDeviceManager(this);
            _guppy = new GuppyLoader(new ConsoleLogger());
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Configure guppy for monogame...
            _guppy.ConfigureMonogame(_graphics, this.Window, this.Content);
            _guppy.ConfigureNetwork(this.PeerFactory);
            _guppy.Initialize();

            // Create a new gameinstance...
            _pongGame = _guppy.GameFactory.Create<ClientPongGame>();
            _pongGame.Start();
        }

        private Peer PeerFactory(IServiceProvider arg)
        {
            var config = new NetPeerConfiguration("pong");
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.Error);
            config.ConnectionTimeout = 10;
            config.AutoFlushSendQueue = false;

            return new ClientPeer(config, arg.GetService<ILogger>());
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

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Environment.Exit(0);
        }
    }
}
