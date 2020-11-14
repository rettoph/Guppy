using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Services;
using Guppy.Utilities;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class GuppyServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<Synchronizer>(p => new Synchronizer());
            services.AddScoped<Synchronizer>();

            services.AddFactory<DriverService>(p => new DriverService());
            services.AddSingleton<DriverService>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
