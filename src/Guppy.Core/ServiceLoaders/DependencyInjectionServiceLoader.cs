using DotNetUtils.DependencyInjection;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class DependencyInjectionServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<GuppyServiceProvider>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetTypeFactory(factory =>
                {
                    factory.SetMethod(p => p);
                });

            services.RegisterService<Settings>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<Settings>();
                });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
