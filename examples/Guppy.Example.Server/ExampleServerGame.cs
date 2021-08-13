using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Example.Server
{
    public class ExampleServerGame : ExampleGame
    {
        #region Private Fields
        private ServerPeer _server;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _server);

            _server.CurrentUser.AddClaim(new Claim("name", "Server"));

            _server.Start();
        }
        #endregion
    }
}
