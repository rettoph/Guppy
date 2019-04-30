using Guppy.Network;
using Guppy.Network.Groups;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Peers;
using Pong.Library.Layers;
using Guppy.Extensions;
using Microsoft.Extensions.Logging;

namespace Pong.Library
{
    public abstract class PongGame : NetworkGame
    {
        protected NetPeerConfiguration config;
        protected Group Group;

        public PongGame(ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        protected override void Boot()
        {
            base.Boot();

            this.Group = this.provider.GetService<Peer>().Groups.GetOrCreateById(Guid.Empty);
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            this.Group.Update();

            base.Update(gameTime);
        }
    }
}
