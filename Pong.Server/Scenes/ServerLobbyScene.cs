using Guppy.Network.Peers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server.Scenes
{
    public class ServerLobbyScene : LobbyScene
    {
        public ServerLobbyScene(Peer peer, IServiceProvider provider) : base(peer, provider)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
