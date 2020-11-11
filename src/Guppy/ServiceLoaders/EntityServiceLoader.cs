using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class EntityServiceCollection : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<EntityList>(p => new EntityList());
            services.AddScoped<EntityList>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
