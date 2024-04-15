using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Messages
{
    internal sealed class ConnectionRequestResponse
    {
        public required ConnectionRequestResponseType Type { get; init; }

        public required UserDto? SystemUser { get; init; }
        public required UserDto? CurrentUser { get; init; }
    }
}
