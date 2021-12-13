using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class log4netServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<ILog>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetMethod(p => LogManager.GetLogger(typeof(GuppyLoader)));
                });

            services.RegisterService<ILoggerRepository>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetMethod(p => p.GetService<ILog>().Logger.Repository);
                });

            services.RegisterService<Hierarchy>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
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

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
