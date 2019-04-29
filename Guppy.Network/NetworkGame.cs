using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Guppy.Network
{
    public abstract class NetworkGame : Game
    {
        #region Private Attributes
        private Peer _peer;
        #endregion

        public NetworkGame(ILogger logger, IServiceProvider provider) : base(logger, provider)
        {
        }

        #region Initialization Methods
        protected override void PostInitialize()
        {
            base.PostInitialize();

            // Load the peer
            _peer = this.provider.GetService<Peer>();
        }
        #endregion

        #region Frame Methods
        public override void Update(GameTime gameTime)
        {
            // Update the peer every frame
            _peer.Update();

            base.Update(gameTime);
        }
        #endregion
    }
}
