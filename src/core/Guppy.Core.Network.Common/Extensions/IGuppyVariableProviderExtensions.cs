using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Core.Network.Common.Constants;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyVariableProviderExtensions
    {
        public static PeerTypeEnum GetPeerType(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.Get<GuppyNetworkVariables.Scope.PeerType>()?.Value ?? throw new KeyNotFoundException();
        }
    }
}
