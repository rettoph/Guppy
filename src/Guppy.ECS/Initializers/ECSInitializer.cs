using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.ECS.Definitions;
using Guppy.ECS.Providers;
using Guppy.ECS.Services;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var systems = assemblies.GetTypes<ISystemDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in systems)
            {
                services.AddSystem(definition);
            }

            services.AddSingleton<IWorldProvider, WorldProvider>();
            services.AddScoped<World>(p => p.GetRequiredService<IWorldProvider>().Create(p));
        }
    }
}
