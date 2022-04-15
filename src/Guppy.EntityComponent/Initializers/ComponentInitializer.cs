using Guppy;
using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Initializers.Collections;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Loaders.Collections;
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
            IComponentCollection components = new ComponentCollection();

            foreach(IComponentLoader loader in loaders)
            {
                loader.ConfigureComponents(components);
            }

            IEnumerable<Type> entities = assemblies.Types.GetTypesAssignableFrom<IEntity>().Where(t => t.IsConcrete());
            services.AddSingleton(components.BuildProvider(entities));
        }
    }
}
