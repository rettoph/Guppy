﻿using Guppy.Attributes;
using Guppy.Loaders;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    [AutoLoad]
    internal sealed class ServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(ActivatedServiceProvider<>));
        }
    }
}