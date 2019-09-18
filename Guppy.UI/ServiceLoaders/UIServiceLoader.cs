using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.UI.Entities;
using Guppy.UI.Utilities.Options;
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
            services.AddScoped<UIScopeOptions>();
            services.AddTransient<Pointer>(p =>
            {
                var options = p.GetRequiredService<UIScopeOptions>();

                if (options.Pointer == default(Pointer) || options.Pointer.Disposed)
                    options.Pointer = p.GetRequiredService<EntityCollection>().Create<Pointer>("ui:pointer");

                return options.Pointer;
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            var entities = provider.GetRequiredService<EntityLoader>();

            entities.TryRegister<Pointer>("ui:pointer");
        }
    }
}
