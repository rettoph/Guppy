using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class GuppyScopeFilterBuilderExtensions
    {
        public static GuppyScopeFilterBuilder RequirePeerType(this GuppyScopeFilterBuilder builder, PeerTypeEnum peerType)
        {
            return builder.RequireScopeVariable(GuppyNetworkVariables.Scope.PeerType.Create(peerType));
        }
    }
}
