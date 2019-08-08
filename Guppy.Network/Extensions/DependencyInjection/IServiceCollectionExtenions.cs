using Guppy.Attributes;
using Guppy.Configurations;
using Guppy.Interfaces;
using Guppy.Network.Security.Authentication.Authenticators;
using Guppy.Utilities.Loaders;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtenions
    {
        #region Authenticator Methods
        public static void AddAuthenticator<TAuthenticator>(this IServiceCollection services)
            where TAuthenticator : class, IAuthenticator
        {
            services.AddScoped<IAuthenticator, TAuthenticator>();
        }
        #endregion
    }
}
