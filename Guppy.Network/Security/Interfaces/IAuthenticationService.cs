using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Guppy.Network.Security.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticateResult Authenticate(User user);
    }
}
