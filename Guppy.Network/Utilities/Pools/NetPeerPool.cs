using Guppy.Extensions.DependencyInjection;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities.Pools
{
    public class NetPeerPool<TNetPeer> : Pool<TNetPeer>
        where TNetPeer : NetPeer
    {
        private NetPeerConfiguration _configuration;

        public NetPeerPool(NetPeerConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override TNetPeer Create(IServiceProvider provider)
        {
            return ActivatorUtilities.CreateInstance<TNetPeer>(provider.CreateScopeWithConfiguration(), _configuration);
        }
    }
}
