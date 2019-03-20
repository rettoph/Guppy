using Guppy.Network;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Library.Scenes
{
    public class LobbyScene : NetworkScene
    {
        protected Group group;

        public LobbyScene(Peer peer, IServiceProvider provider) : base(provider)
        {
            group = peer.Groups.GetOrCreateById(Guid.Empty);
        }
    }
}
