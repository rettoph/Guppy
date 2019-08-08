using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Authentication.Authenticators
{
    public interface IAuthenticator
    {
        Boolean Validate(User user, NetIncomingMessage hail);

        String GetResponse(User user, NetIncomingMessage hail);
    }
}
