using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.ECS.Services;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Initializers
{
    internal sealed class ECSInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var systems = assemblies.GetTypes<ISystem>().WithAttribute<AutoLoadAttribute>(false)
                .OrderBy(x => x.GetCustomAttribute<AutoLoadAttribute>()!.Order);

            foreach (Type system in systems)
            {
                services.AddSystem(system);
            }

            services.AddScoped<IWorldService, WorldService>();
            services.AddScoped<World>(p => p.GetRequiredService<IWorldService>().Instance);
        }
    }
}
