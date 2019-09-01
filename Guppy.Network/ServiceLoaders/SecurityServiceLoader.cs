using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Interfaces;
using Guppy.Network.Factories;
using Guppy.Network.Security;
using Guppy.Network.Security.Collections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    [IsServiceLoader]
    internal sealed class SecurityServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UserFactory>();
            services.AddSingleton<UserCollection>();
            services.AddTransient<CreatableCollection<User>>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
