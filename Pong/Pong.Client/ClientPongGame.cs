using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Network.Peers;
using Guppy.Network.Security.Authentication;
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

        public ClientPongGame(EntityCollection entities, ClientPeer client) : base(client)
        {
            _client = client;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _client.Start();

            var user = _client.TryCreateUser(u =>
            {
                u.AddClaim("name", "Rettoph");
            });

            _client.TryConnect("127.0.0.1", 1337, user);
        }
    }
}
