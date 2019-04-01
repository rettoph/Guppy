using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server.Scenes
{
    public class ServerLobbyScene : LobbyScene
    {
        private ServerPeer _server;
        private ServerGroup _group;

        public ServerLobbyScene(ServerPeer server, IServiceProvider provider) : base(server, provider)
        {
            _server = server;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _group = _server.Groups.GetById(Guid.Empty) as ServerGroup;
        }
    }
}
