using Guppy.Network.Enums;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;

namespace Guppy.Network.Extensions.Identity
{
    public static class UserExtensions
    {
        public static UserAction CreateAction(this User user, UserActionTypes type, ClaimAccessibility accessibility)
        {
            return new UserAction()
            {
                Id = user.Id,
                Type = type,
                Claims = user.Where(x => x.Accessibility <= accessibility).ToArray(),
            };
        }
    }
}
