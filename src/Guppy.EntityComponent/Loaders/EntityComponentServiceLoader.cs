using Guppy.Attributes;
using Guppy;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Loaders;

namespace Guppy.EntityComponent.Loaders
{
    internal sealed class EntityComponentServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEntityService, EntityService>();
        }
    }
}
