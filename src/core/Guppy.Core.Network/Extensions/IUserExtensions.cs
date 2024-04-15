﻿using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Messages;

namespace Guppy.Core.Network.Common.Extensions.Identity
{
    public static class UserExtensions
    {
        internal static UserAction CreateAction(this IUser user, UserActionTypes type, ClaimAccessibility accessibility)
        {
            return new UserAction()
            {
                Type = type,
                UserDto = user.ToDto(accessibility)
            };
        }
    }
}
