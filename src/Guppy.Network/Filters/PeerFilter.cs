using Guppy.Common.Filters;
using Guppy.Common.Implementations;
using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Filters
{
    public class PeerFilter<TPeer, TImplementation> : SimpleFilter
        where TPeer : Peer
    {
        public PeerFilter() : base(typeof(TImplementation))
        {
        }

        public override bool Invoke(IServiceProvider provider, Type implementationType)
        {
            var instance = provider.GetRequiredService<ServiceActivator<Peer>>();
            return instance.Type?.IsAssignableTo(typeof(TPeer)) ?? false;
        }
    }
}
