using Guppy.Common.Attributes;
using Guppy.Filters;
using Guppy.Network.Enums;
using Guppy.Network.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Attributes
{
    public sealed class PeerTypeFilterAttribute : InitializableAttribute
    {
        public readonly PeerType PeerType;

        public PeerTypeFilterAttribute(PeerType peerType)
        {
            this.PeerType = peerType;
        }

        protected override void Initialize(IServiceCollection services, Type classType)
        {
            services.AddFilter(new PeerTypeFilter(this.PeerType, classType));
        }
    }
}
