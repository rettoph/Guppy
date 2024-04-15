using Guppy.Core.Network.Identity.Claims;

namespace Guppy.Core.Network.Messages
{
    internal class ConnectionRequestData
    {
        public required Claim[] Claims { get; init; }
    }
}
