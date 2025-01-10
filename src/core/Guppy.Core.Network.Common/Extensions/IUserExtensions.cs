using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Identity.Enums;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IUserExtensions
    {
        public static UserDto ToDto(this IUser user, ClaimAccessibilityEnum accessibility = ClaimAccessibilityEnum.Public)
        {
            return new()
            {
                Id = user.Id,
                Claims = user.Where(x => x.Accessibility <= accessibility).ToArray(),
            };
        }
    }
}