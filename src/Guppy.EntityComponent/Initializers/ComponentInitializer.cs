using Guppy;
using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Initializers.Collections;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Loaders.Collections;
using Guppy.EntityComponent.Loaders.Definitions;
using Guppy.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Initializers
{
    internal sealed class ComponentInitializer : GuppyInitializer<IComponentLoader>
    {
        protected override void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<IComponentLoader> loaders)
        {
            var componentDescriptors = assemblies.Types.GetTypesWithAttribute<ComponentDefinition, AutoLoadAttribute>()
                .Select(x => Activator.CreateInstance(x) as ComponentDefinition)
                .Select(x => x.BuildDescriptor());

            var componentFilterDescriptors = assemblies.Types.GetTypesWithAttribute<ComponentFilterDefinition, AutoLoadAttribute>()
                .Select(x => Activator.CreateInstance(x) as ComponentFilterDefinition)
                .Select(x => x.BuildDescriptor());

            ComponentCollection components = new ComponentCollection(componentDescriptors);
            ComponentFilterCollection filters = new ComponentFilterCollection(componentFilterDescriptors);

            foreach(IComponentLoader loader in loaders)
            {
                loader.ConfigureComponents(components, filters);
            }

            IEnumerable<Type> entities = assemblies.Types.GetTypesAssignableFrom<IEntity>().Where(t => t.IsConcrete());
            services.AddSingleton(components.BuildProvider(entities, filters));
        }
    }
}
