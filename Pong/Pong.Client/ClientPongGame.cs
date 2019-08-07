using Guppy.Attributes;
using Guppy.Network.Peers;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client
{
    [IsGame]
    public class ClientPongGame : PongGame
    {
        private ClientPeer _client;

        public ClientPongGame(ClientPeer client) : base(client)
        {
            _client = client;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _client.Start();
            _client.TryConnect("127.0.0.1", 1337);
        }
    }
}
