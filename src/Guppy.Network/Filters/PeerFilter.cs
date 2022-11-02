using Guppy.Common;
using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Filters
{
    public class PeerFilter<TPeer, TImplementation> : ServiceFilter<TImplementation>
        where TPeer : Peer
    {
        public override bool Invoke(IServiceProvider provider)
        {
            var instance = provider.GetRequiredService<Faceted<Peer>>();
            return instance.Type?.IsAssignableTo(typeof(TPeer)) ?? false;
        }
    }
}
