using Guppy.Attributes;
using Guppy.Common.Providers;
using Guppy.ECS.Definitions;
using Guppy.ECS.Providers;
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
            var definitions = assemblies.GetTypes<ISystemDefinition>().WithAttribute<AutoLoadAttribute>(false);

            foreach(Type definition in definitions)
            {
                services.AddSystem(definition);
            }

            services.AddSingleton<IWorldProvider, WorldProvider>();
            services.AddScoped<World>(p => p.GetRequiredService<IWorldProvider>().Create(p));
        }
    }
}
