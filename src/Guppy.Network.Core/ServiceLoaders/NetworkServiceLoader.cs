using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Network.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.ServiceLoaders
{
    internal sealed class NetworkServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<Peer>()
                .SetLifetime(ServiceLifetime.Singleton)
                .CreateTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Peer>();
                });

            var test = new MessageServiceBuilder();
            test.Register()
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
