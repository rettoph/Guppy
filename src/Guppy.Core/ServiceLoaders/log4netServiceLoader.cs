using Guppy.Attributes;
using Guppy.DependencyInjection;
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
        public void RegisterServices(ServiceCollection services)
        {
            // Configure & add log4net services...
            services.AddFactory<ILog>(p => LogManager.GetLogger(typeof(GuppyLoader)));
            services.AddFactory<ILoggerRepository>(p => p.GetService<ILog>().Logger.Repository);
            services.AddFactory<Hierarchy>(p => (Hierarchy)p.GetService<ILoggerRepository>());

            services.AddSingleton<ILog>();
            services.AddSingleton<ILoggerRepository>();
            services.AddSingleton<Hierarchy>();

            services.AddSetup<ILog>((l, p, s) =>
            { // Mark as configured...
                ((Hierarchy)l.Logger.Repository).Configured = true;
            }, -10);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
