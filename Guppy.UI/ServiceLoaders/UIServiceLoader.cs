using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.UI.Utilities;

namespace Guppy.UI.ServiceLoaders
{
    [IsServiceLoader]
    public class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Pointer>("ui:pointer");
            services.AddScoped<PrimitiveBatch>();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
