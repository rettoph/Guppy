using Guppy.Attributes;
using Guppy.Collections;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityServiceCollection : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<EntityCollection>(p => new EntityCollection());
            services.AddScoped<EntityCollection>();
            // Add custom configuration to auto add the entity into the global entity collection post initialization
            services.AddConfiguration<Entity>((e, p, f) => p.GetService<EntityCollection>().TryAdd(e), Int32.MaxValue);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
