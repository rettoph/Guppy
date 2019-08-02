using Guppy.Attributes;
using Guppy.Configurations;
using Guppy.Interfaces;
using Guppy.Utilities.Delegaters;
using Guppy.Utilities.Loggers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader(110)]
    public class GuppyServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddScoped<ScopeConfiguration>();
            services.AddTransient<EventDelegater>();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
