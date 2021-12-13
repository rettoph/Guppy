using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Threading.Utilities;
using Guppy.DependencyInjection.Builders;
using DotNetUtils.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
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
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<IntervalInvoker>();
                });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
