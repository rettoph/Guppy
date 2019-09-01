using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    internal sealed class DriverServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DriverFactory>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
