using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Extensions.log4net;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.ServiceLoaders
{
    [AutoLoad]
    internal sealed class OutputServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            // Configure & add log4net services...
            services.AddFactory<ILog>(p => LogManager.GetLogger(typeof(GuppyLoader)));
            services.AddFactory<ILoggerRepository>(p => p.GetService<ILog>().Logger.Repository);
            services.AddFactory<Hierarchy>(p => (Hierarchy)p.GetService<ILoggerRepository>());

            services.AddSingleton<ILog>();
            services.AddSingleton<ILoggerRepository>();
            services.AddSingleton<Hierarchy>();

            services.AddConfiguration<ILog>((l, p, s) =>
            { // Mark as configured...
                ((Hierarchy)l.Logger.Repository).Configured = true;
            }, -10);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {

        }
    }
}
