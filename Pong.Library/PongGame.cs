using Guppy.Network;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Peers;
using Pong.Library.Layers;
using Guppy.Extensions;

namespace Pong.Library
{
    public abstract class PongGame : NetworkGame
    {
        protected NetPeerConfiguration config;
        protected Group Group;

        public PongGame(IServiceProvider provider) : base(provider)
        {
        }

        protected override void Boot()
        {
            base.Boot();
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
