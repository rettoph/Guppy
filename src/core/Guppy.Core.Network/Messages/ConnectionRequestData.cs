using Guppy.Core.Network.Common.Claims;

namespace Guppy.Core.Network.Common.Messages
{
    internal class ConnectionRequestData
    {
        public required Claim[] Claims { get; init; }
    }
}
