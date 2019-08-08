using Guppy.Configurations;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Utilities.Pools;
using Guppy.Collections;
using Guppy.Network.Security.Authentication.Authenticators;

namespace Guppy.Network.Extensions.DependencyInjection
{
    public static class IServiceProviderExtensions
    {
        #region Authenticator Methods
        public static IEnumerable<IAuthenticator> GetAuthenticators(this IServiceProvider provider)
        {
            return provider.GetServices<IAuthenticator>();
        }
        #endregion
    }
}
