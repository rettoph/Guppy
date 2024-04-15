using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Identity.Dtos;

namespace Guppy.Core.Network.Messages
{
    internal sealed class UserAction
    {
        public UserActionTypes Type { get; init; }

        public required UserDto UserDto { get; init; }
    }
}
