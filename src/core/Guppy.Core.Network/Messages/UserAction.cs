using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Messages
{
    internal sealed class UserAction
    {
        public byte GroupId { get; init; }

        public UserActionTypeEnum Type { get; init; }

        public required UserDto UserDto { get; init; }
    }
}