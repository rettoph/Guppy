using Guppy.Network.Enums;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public struct UserActionMessage
    {
        public readonly int Id;
        public readonly IEnumerable<Claim> Claims;
        public readonly UserActionType Type;

        public UserActionMessage(int id, IEnumerable<Claim> claims, UserActionType type)
        {
            this.Id = id;
            this.Claims = claims;
            this.Type = type;
        }

        private static UserActionMessage Create(User user, UserActionType type)
        {
            return new UserActionMessage(user.Id, user.Claims.Where(c => c.Type == ClaimType.Public), type);
        }

        public static UserActionMessage Joined(User user)
        {
            return UserActionMessage.Create(user, UserActionType.Joined);
        }

        public static UserActionMessage Left(User user)
        {
            return UserActionMessage.Create(user, UserActionType.Left);
        }
    }
}
