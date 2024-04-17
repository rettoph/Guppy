using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common.Messages
{
    internal sealed class UserAction
    {
        public byte GroupId { get; init; }

        public UserActionTypes Type { get; init; }

        public required UserDto UserDto { get; init; }
    }
}
