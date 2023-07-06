using Guppy.Attributes;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;
using Guppy.Configurations;
using Guppy.Network.Enums;

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
            configuration.Builder.AddFilter(new ServiceFilter<PeerType>(classType, this.Flags));
        }
    }
}
