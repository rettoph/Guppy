using Guppy.Core.Network.Common.Claims;

namespace Guppy.Core.Network.Messages
{
    internal class ConnectionRequestData
    {
        public required Claim[] Claims { get; init; }
    }
}