using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Claims = user.Where(x => x.Accessiblity <= accessibility).ToArray()
            };
        }
    }
}
