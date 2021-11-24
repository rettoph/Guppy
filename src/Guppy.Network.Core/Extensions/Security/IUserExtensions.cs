using Guppy.Network.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Security;

namespace Guppy.Network.Extensions.Security
{
    public static class IUserExtensions
    {
        public static void TryWrite(this IUser user, NetOutgoingMessage om)
        {
            om.Write(user.Id);

            om.Write(user.Claims.Values, (claim, om) =>
            {
                om.Write(claim.Key);
                om.Write(claim.Value);
            });
        }

        public static void TryRead(this IUser user, NetIncomingMessage im)
        {
            im.ReadEnumerable<Claim>(im => new Claim(im.ReadString(), im.ReadString())).ForEach(claim =>
            {
                user.SetClaim(claim);
            });
        }
    }
}
