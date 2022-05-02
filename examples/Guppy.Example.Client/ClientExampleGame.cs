using Guppy.EntityComponent.Services;
using Guppy.Example.Library;
using Guppy.Gaming.Graphics;
using Guppy.Gaming.Services;
using Guppy.Network.Peers;
using Guppy.Network.Security.Structs;
using Guppy.Threading;
using LiteNetLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Client
{
    internal class ClientExampleGame : ExampleGame
    {
        private ClientPeer _client;

        public ClientExampleGame(
            Bus bus,
            ClientPeer client,
            ISceneService scenes,
            IEntityService entities) : base(bus, client, scenes, entities)
        {
            _client = client;

            _client.Start();
            _client.Connect("localhost", 1337, Claim.Public("display-name", "Rettoph"), Claim.Protected("username", "TonyG"));
        }
    }
}
