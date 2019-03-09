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

        protected override void Boot()
        {
            base.Boot();

            this.config.Port = 1337;
        }

        protected override Peer PeerFactory(IServiceProvider arg)
        {
            return new ServerPeer(this.config, this.logger);
        }
    }
}
