using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Messages
{
    internal class ConnectionRequestData
    {
        public required Claim[] Claims { get; init; }
    }
}
