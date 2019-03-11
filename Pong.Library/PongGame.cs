using Guppy.Network;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Peers;

namespace Pong.Library
{
    public abstract class PongGame : NetworkGame
    {
        protected NetPeerConfiguration config;
        protected Group Group;

        protected override void Boot()
        {
            base.Boot();

            this.config = new NetPeerConfiguration("pong");
            this.config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            this.config.ConnectionTimeout = 10;
            this.config.AutoFlushSendQueue = false;
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            this.Group = this.provider.GetService<Peer>().Groups.GetOrCreateById(Guid.Empty);
        }

        public override void Update(GameTime gameTime)
        {
            this.Group.Update();

            base.Update(gameTime);
        }
    }
}
