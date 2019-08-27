using Guppy.Factories;
using Guppy.Network.Utilitites.Options;
using Guppy.Pooling.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Factories
{
    internal class NetPeerFactory : Factory<NetPeer>
    {
        private NetworkOptions _options;

        public NetPeerFactory(NetworkOptions options, IPoolManager<NetPeer> pools, IServiceProvider provider) : base(pools, provider)
        {
            _options = options;
        }

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null)
        {
            if (_options.NetPeer == null)
                _options.NetPeer = pool.Pull(t => ActivatorUtilities.CreateInstance(provider, t)) as NetPeer;

            return _options.NetPeer as T;
        }
    }
}
