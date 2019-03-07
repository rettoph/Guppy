using Guppy.Network.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security
{
    class DefaultAuthenticationService : IAuthenticationService
    {
        public DefaultAuthenticationService()
        {
        }

        public AuthenticateResult Authenticate(User user)
        {
            return AuthenticateResult.Success("Welcome");
        }
    }
}
