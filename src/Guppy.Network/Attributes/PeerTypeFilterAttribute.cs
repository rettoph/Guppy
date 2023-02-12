using Guppy.Attributes;
using Guppy.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network.Attributes
{
    public sealed class PeerTypeFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly PeerType Flags;

        public PeerTypeFilterAttribute(PeerType flags)
        {
            this.Flags = flags;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            configuration.Services.AddFilter(new PeerTypeFilter(this.Flags, classType));
        }
    }
}
