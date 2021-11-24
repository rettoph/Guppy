﻿using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using GuppyServiceCollection = Guppy.DependencyInjection.GuppyServiceCollection;
using GuppyServiceProvider = Guppy.DependencyInjection.GuppyServiceProvider;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class DependencyInjectionServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<GuppyServiceProvider>(p => p);
            services.RegisterScoped<GuppyServiceProvider>();

            services.RegisterTypeFactory<Settings>(p => new Settings());
            services.RegisterSingleton<Settings>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
