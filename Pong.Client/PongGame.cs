using Guppy;
using Guppy.Network.Peers;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    public class PongGame : Game
    {
        private ClientPeer _client;

        public PongGame(ClientPeer client)
        {
            _client = client;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.scenes.Create<PongScene>();

            _client.TryConnect("127.0.0.1", 1337);
        }
    }
}
