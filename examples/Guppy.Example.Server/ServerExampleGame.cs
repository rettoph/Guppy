using Guppy.EntityComponent.Services;
using Guppy.Example.Library;
using Guppy.Gaming.Services;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Guppy.Providers;
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
        private ISettingProvider _settings;

        public ServerExampleGame(
            Bus bus, 
            ServerPeer server,
            ISceneService scenes, 
            IEntityService entities,
            ISettingProvider settings) : base(bus, server, scenes, entities)
        {
            _server = server;
            _settings = settings;

            _settings.Get<NetworkAuthorization>().Value = NetworkAuthorization.Master;
            _server.Start(1337);

            var value = new TestNetMessage("test", 10, 1, 3);
            _server.Room.Messages.CreateOutgoing<TestNetMessage>(in value).Send().Recycle();
        }
    }
}
