﻿using Guppy.Attributes;
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
            services.RegisterTypeFactory<MessageBus>()
                .SetDefaultConstructor<MessageBus>();

            services.RegisterService<MessageBus>(Constants.ServiceNames.GameMessageBus)
                .SetLifetime(ServiceLifetime.Singleton)
                .SetFactoryType<MessageBus>();

            services.RegisterService<MessageBus>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetFactoryType<MessageBus>();

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
