using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Extensions;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Messages;

namespace Guppy.Core.Network.Extensions.Identity
{
    public static class UserExtensions
    {
        internal static UserAction CreateAction(this IUser user, byte groupId, UserActionTypes type, ClaimAccessibility accessibility)
        {
            return new UserAction()
            {
                GroupId = groupId,
                Type = type,
                UserDto = user.ToDto(accessibility)
            };
        }
    }
}
