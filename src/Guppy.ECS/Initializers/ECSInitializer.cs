using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.ECS.Definitions;
using Guppy.ECS.Filters;
using Guppy.ECS.Providers;
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
            var components = assemblies.GetTypes<IComponentDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach (Type definition in components)
            {
                services.AddComponent(definition);
            }

            services.AddScoped<IEntityService, EntityService>();

            var systemsDefinitions = assemblies.GetTypes<ISystemDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in systemsDefinitions)
            {
                services.AddSystem(definition);
            }

            var systems = assemblies.GetTypes<ISystem>().WithAttribute<AutoLoadAttribute>(false);

            foreach (Type system in systems)
            {
                services.AddSystem(system, system.GetCustomAttribute<AutoLoadAttribute>()!.Order);
            }

            services.AddSingleton<ISystemProvider, SystemProvider>();
            services.AddScoped<IEnumerable<ISystem>>(p => p.GetRequiredService<ISystemProvider>().Create(p));
            services.AddScoped<IWorldService, WorldService>();
            services.AddScoped<World>(p => p.GetRequiredService<IWorldService>().Instance);
        }
    }
}
