using Guppy.Network.Enums;
using Guppy.Network.Identity.Dtos;

namespace Guppy.Network.Messages
{
    internal sealed class ConnectionRequestResponse
    {
        public required ConnectionRequestResponseType Type { get; init; }

        public required UserDto? SystemUser { get; init; }
        public required UserDto? CurrentUser { get; init; }
    }
}
