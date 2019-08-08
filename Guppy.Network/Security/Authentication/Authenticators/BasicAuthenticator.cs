using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Authentication.Authenticators
{
    public sealed class BasicAuthenticator : IAuthenticator
    {
        public Boolean Validate(User user, NetIncomingMessage hail)
        {
            return true;
        }

        public String GetResponse(User user, NetIncomingMessage hail)
        {
            return String.Empty;
        }
    }
}
