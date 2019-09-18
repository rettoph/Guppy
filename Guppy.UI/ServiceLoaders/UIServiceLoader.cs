using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.ServiceLoaders
{
    [IsServiceLoader]
    public class UIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Pointer>(p => p.GetRequiredService<EntityCollection>().Create<Pointer>("ui:pointer"));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            var entities = provider.GetRequiredService<EntityLoader>();

            entities.TryRegister<Pointer>("ui:pointer");
        }
    }
}
