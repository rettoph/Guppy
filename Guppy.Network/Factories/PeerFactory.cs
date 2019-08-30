using Guppy.Factories;
using Guppy.Network.Peers;
using Guppy.Network.Utilitites.Options;
using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Factories
{
    internal class PeerFactory : InitializableFactory<Peer>
    {
        internal NetworkOptions _options;

        #region Constructor
        public PeerFactory(NetworkOptions options ,IPoolManager<Peer> pools, IServiceProvider provider) : base(pools, provider)
        {
            _options = options;
        }
        #endregion

        protected override T Build<T>(IServiceProvider provider, IPool pool, Action<T> setup = null, Action<T> create = null)
        {
            if(_options.Peer == null)
                base.Build<T>(provider, pool, p =>
                {
                    _options.Peer = p;
                    setup?.Invoke(p);
                }, create);

            return _options.Peer as T;
        }
    }
}
