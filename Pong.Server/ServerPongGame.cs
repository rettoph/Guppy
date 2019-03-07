using Guppy.Network.Peers;
using Lidgren.Network;
using Pong.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Server
{
    public class ServerPongGame : PongGame
    {
        public ServerPongGame()
        {
        }

        protected override Peer PeerFactory(IServiceProvider arg)
        {
            return new Guppy.Network.Peers.Server(new NetPeerConfiguration("pong"), this.logger);
        }
    }
}
