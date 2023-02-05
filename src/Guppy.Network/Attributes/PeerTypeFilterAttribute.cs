using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Attributes
{
    public sealed class PeerTypeFilterAttribute : InitializableAttribute
    {
        public readonly PeerType PeerType;

        public PeerTypeFilterAttribute(PeerType peerType)
        {
            this.PeerType = peerType;
        }

        protected override void Initialize(GuppyEngine engine, Type classType)
        {
            engine.Services.AddFilter(new PeerTypeFilter(this.PeerType, classType));
        }
    }
}
