using Guppy.Network.Enums;
using Guppy.Network.Identity.Dtos;

namespace Guppy.Network.Messages
{
    internal sealed class UserAction
    {
        public UserActionTypes Type { get; init; }

        public required UserDto UserDto { get; init; }
    }
}
