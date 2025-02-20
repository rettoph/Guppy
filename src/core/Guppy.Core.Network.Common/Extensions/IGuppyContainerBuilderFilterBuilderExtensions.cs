using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyContainerBuilderFilterBuilderExtensions
    {
        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequirePeerType(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> builder,
            PeerTypeEnum peerType)
        {
            return builder.RequireScopeVariable(GuppyNetworkVariables.Scope.PeerType.Create(peerType));
        }
    }
}
