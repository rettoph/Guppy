using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using log4net;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class log4netServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            // Configure & add log4net services...
            services.RegisterTypeFactory<ILog>(p => LogManager.GetLogger(typeof(GuppyLoader)));
            services.RegisterTypeFactory<ILoggerRepository>(p => p.GetService<ILog>(Guppy.Core.Constants.ServiceConfigurationKeys.ILog).Logger.Repository);
            services.RegisterTypeFactory<Hierarchy>(p => (Hierarchy)p.GetService<ILoggerRepository>());

            services.RegisterService<ILog>()
                .SetLifetime(ServiceLifetime.Singleton);

            services.RegisterService<ILoggerRepository>()
                .SetLifetime(ServiceLifetime.Singleton);

            services.RegisterService<Hierarchy>()
                .SetLifetime(ServiceLifetime.Singleton);

            services.RegisterSetup<ILog>((l, p, s) =>
            { // Mark as configured...
                ((Hierarchy)l.Logger.Repository).Configured = true;
            }, -10);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
