using Guppy.Network.Enums;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Dtos;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;

namespace Guppy.Network.Extensions.Identity
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
