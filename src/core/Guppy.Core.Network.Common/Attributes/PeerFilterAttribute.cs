using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Attributes
{
    [Obsolete]
    public class PeerFilterAttribute(PeerType requiredPeerType) : GuppyConfigurationAttribute
    {
        public readonly PeerType RequiredPeerType = requiredPeerType;

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterPeerTypeFilter(classType, this.RequiredPeerType);
        }
    }
}
