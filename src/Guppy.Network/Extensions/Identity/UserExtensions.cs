using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;

namespace Guppy.Network.Extensions.Identity
{
    public static class UserExtensions
    {
        public static UserAction CreateAction(this User user, UserAction.Actions action, ClaimAccessibility accessibility)
        {
            return new UserAction()
            {
                Id = user.Id,
                Action = action,
                Claims = user.Where(x => x.Accessibility <= accessibility).ToArray(),
            };
        }
    }
}
