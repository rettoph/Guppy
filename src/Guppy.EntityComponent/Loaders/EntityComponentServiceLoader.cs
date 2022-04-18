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
using Minnow.Providers;

namespace Guppy.EntityComponent.Loaders
{
    internal sealed class EntityComponentServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEntityService, EntityService>();

            services.AddSingleton<ITypeProvider<IEntity>>(p => p.GetRequiredService<IAssemblyProvider>().GetTypes<IEntity>(t => t.IsConcrete()));
        }
    }
}
