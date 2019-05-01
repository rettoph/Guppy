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
    public abstract class PongGame : Guppy.Game
    {
        public PongGame(ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        protected override void Boot()
        {
            base.Boot();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
