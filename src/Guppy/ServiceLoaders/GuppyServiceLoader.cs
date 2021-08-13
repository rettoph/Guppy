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
        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<Synchronizer>(p => new Synchronizer());
            services.RegisterScoped<Synchronizer>();

            services.RegisterTypeFactory<IntervalInvoker>(p => new IntervalInvoker());
            services.RegisterScoped<IntervalInvoker>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
