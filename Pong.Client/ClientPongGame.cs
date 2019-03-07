using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client
{
    class ClientPongGame : PongGame
    {
        private GraphicsDeviceManager _graphics;
        private GameWindow _window;
        private ContentManager _content;

        public ClientPongGame(GraphicsDeviceManager graphics, GameWindow window, ContentManager content)
        {
            _graphics = graphics;
            _window = window;
            _content = content;
        }

        protected override void Boot()
        {
            base.Boot();

            // Add the client services...
            this.services.AddSingleton<GraphicsDeviceManager>(_graphics);
            this.services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            this.services.AddSingleton<GameWindow>(_window);
            this.services.AddSingleton<ContentManager>(_content);
            this.services.AddSingleton<SpriteBatch>(new SpriteBatch(_graphics.GraphicsDevice));
        }

        protected override Peer PeerFactory(IServiceProvider arg)
        {
            return new Guppy.Network.Peers.Client(new NetPeerConfiguration("pong"), this.logger);
        }
    }
}
