using Guppy.Core.Common.Implementations;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Constants
{
    public static class GuppyNetworkVariables
    {
        public static class Scope
        {
            public class PeerType(PeerTypeEnum value) : ScopeVariable<PeerType, PeerTypeEnum>(value)
            {

            }
        }
    }
}
