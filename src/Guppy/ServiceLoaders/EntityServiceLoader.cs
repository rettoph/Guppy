using Guppy.Attributes;
using Guppy.Collections;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityServiceCollection : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<EntityCollection>((p) => new EntityCollection());
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
