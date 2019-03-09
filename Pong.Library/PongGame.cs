using Guppy.Network;
using Lidgren.Network;
using System;

namespace Pong.Library
{
    public abstract class PongGame : NetworkGame
    {
        protected NetPeerConfiguration config;

        protected override void Boot()
        {
            base.Boot();

            this.config = new NetPeerConfiguration("pong");
            this.config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            this.config.ConnectionTimeout = 1000;
            this.config.AutoFlushSendQueue = false;
        }
    }
}
