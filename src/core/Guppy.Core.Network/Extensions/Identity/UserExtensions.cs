using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Identity;
using Guppy.Core.Network.Identity.Dtos;
using Guppy.Core.Network.Identity.Enums;
using Guppy.Core.Network.Messages;

namespace Guppy.Core.Network.Extensions.Identity
{
    public static class UserExtensions
    {
        internal static UserAction CreateAction(this User user, UserActionTypes type, ClaimAccessibility accessibility)
        {
            return new UserAction()
            {
                Type = type,
                UserDto = user.ToDto(accessibility)
            };
        }

        public static UserDto ToDto(this User user, ClaimAccessibility accessibility = ClaimAccessibility.Public)
        {
            return new UserDto()
            {
                Id = user.Id,
                Claims = user.Where(x => x.Accessibility <= accessibility).ToArray(),
            };
        }
    }
}
