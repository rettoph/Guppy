using Guppy.EntityComponent.DependencyInjection;
using Minnow.General;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Network.Builders;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.Network.GuppyInitializers
{
    [AutoLoad]
    internal sealed class NetworkServiceLoaderGuppyInitializer : IGuppyInitializer
    {
        public void PreInitialize(AssemblyHelper assemblies, ServiceProviderBuilder services, IEnumerable<IGuppyLoader> loaders)
        {
            using (NetworkProviderBuilder networkBuilder = new NetworkProviderBuilder())
            {
                foreach (IServiceLoader serviceLoader in loaders)
                {
                    if (serviceLoader is INetworkServiceLoader networkServiceLoader)
                    {
                        networkServiceLoader.ConfigureNetwork(networkBuilder);
                    }
                }

                NetworkProvider network = networkBuilder.Build();
                services.RegisterService<NetworkProvider>()
                    .SetLifetime(ServiceLifetime.Singleton)
                    .SetTypeFactory(builder =>
                    {
                        builder.SetMethod(p => network);
                    });
            }
        }

        public void PostInitialize(ServiceProvider provider, IEnumerable<IGuppyLoader> loaders)
        {
            // throw new NotImplementedException();
        }
    }
}
