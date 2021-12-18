using Guppy.Example.Library;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Example.Server
{
    public class ExampleServerGame : ExampleGame
    {
        #region Private Fields
        private ServerPeer _server;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _server);
        }

        protected override void PostInitialize(ServiceProvider provider)
        {
            base.PostInitialize(provider);


            _server.TryStartAsync(1337);
        }
        #endregion
    }
}
