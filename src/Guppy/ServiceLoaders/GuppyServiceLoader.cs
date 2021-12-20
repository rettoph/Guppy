using Guppy.Attributes;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Threading.Utilities;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<ThreadQueue>()
                .SetDefaultConstructor<ThreadQueue>();

            services.RegisterService<ThreadQueue>(Constants.ServiceNames.GameUpdateThreadQueue)
                .SetLifetime(ServiceLifetime.Singleton)
                .SetFactoryType<ThreadQueue>();

            services.RegisterService<ThreadQueue>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetFactoryType<ThreadQueue>();

            services.RegisterService<IntervalInvoker>()
                .SetLifetime(ServiceLifetime.Scoped)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<IntervalInvoker>();
                });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
