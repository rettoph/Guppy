using Guppy.EntityComponent.Services;
using Guppy.Example.Library;
using Guppy.Gaming.Services;
using Guppy.Network.Peers;
using Guppy.Threading;
using LiteNetLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Server
{
    internal class ServerExampleGame : ExampleGame
    {
        private ServerPeer _server;

        public ServerExampleGame(
            Bus bus, 
            ServerPeer server,
            ISceneService scenes, 
            IEntityService entities) : base(bus, server, scenes, entities)
        {
            _server = server;

            _server.Start(1337);
        }
    }
}
