using Guppy;
using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Initializers
{
    internal sealed class ComponentInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var componentFilters = assemblies.GetAttributes<ComponentFilterDefinition, AutoLoadAttribute>().Types;

            foreach (Type componentFilter in componentFilters)
            {
                services.AddComponentFilter(componentFilter);
            }

            var components = assemblies.GetAttributes<ComponentDefinition, AutoLoadAttribute>().Types;

            foreach (Type component in components)
            {
                services.AddComponent(component);
            }

            services.AddSingleton<IComponentProvider, ComponentProvider>();
        }
    }
}
