using Guppy.Network.Peers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.Scenes
{
    public class ClientLobbyScene : LobbyScene
    {
        public ClientLobbyScene(Peer peer, IServiceProvider provider) : base(peer, provider)
        {
        }
    }
}
