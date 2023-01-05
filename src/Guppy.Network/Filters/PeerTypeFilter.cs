using Guppy.Common.DependencyInjection.Interfaces;
using Guppy.Common.Filters;
using Guppy.Common.Implementations;
using Guppy.Network.Enums;
using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Filters
{
    public class PeerTypeFilter : SimpleFilter
    {
        public readonly PeerType PeerType;

        public PeerTypeFilter(PeerType peerType, Type type) : base(type)
        {
            this.PeerType = peerType;
        }

        public override bool Invoke(IServiceProvider provider, object service)
        {
            var netScope = provider.GetRequiredService<NetScope>();
            var result = netScope.Peer?.Type == this.PeerType;

            return result;
        }
    }
}
