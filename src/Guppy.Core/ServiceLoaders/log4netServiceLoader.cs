using Guppy.EntityComponent.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.Attributes;
using Guppy.Interfaces;
using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class log4netServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<ILog>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => LogManager.GetLogger(typeof(GuppyLoader)));
                });

            services.RegisterService<ILoggerRepository>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => p.GetService<ILog>().Logger.Repository);
                });

            services.RegisterService<Hierarchy>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => (Hierarchy)p.GetService<ILoggerRepository>());
                });

            services.RegisterSetup<ILog>()
                .SetOrder(-10)
                .SetMethod((l, p, s) =>
                { // Mark as configured...
                    ((Hierarchy)l.Logger.Repository).Configured = true;
                });
        }
    }
}
