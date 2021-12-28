using Guppy.Attributes;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Threading.Utilities;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<MessageQueue>()
                .SetDefaultConstructor<MessageQueue>();

            services.RegisterService<MessageQueue>(Constants.ServiceNames.GameMessageQueue)
                .SetLifetime(ServiceLifetime.Singleton)
                .SetFactoryType<MessageQueue>();

            services.RegisterService<MessageQueue>(Constants.ServiceNames.SceneMessagueQueue)
                .SetLifetime(ServiceLifetime.Scoped)
                .SetFactoryType<MessageQueue>();

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
