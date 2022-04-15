using Guppy.EntityComponent;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Providers;
using Guppy.EntityComponent;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minnow.System.Helpers;
using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.EntityComponent.Loaders.Collections;

namespace Guppy.EntityComponent.Initializers.Collections
{
    internal sealed class ComponentCollection : List<ComponentDescriptor>, IComponentCollection
    {
        public IComponentCollection Add<TEntity, TComponent>(Func<IServiceProvider, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            this.Add(ComponentDescriptor.Create<TEntity, TComponent>(factory));

            return this;
        }

        public IComponentCollection Add<TEntity, TComponent>()
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            this.Add(ComponentDescriptor.Create(typeof(TEntity), typeof(TComponent), ActivatorUtilitiesHelper.BuildFactory<TComponent>()));

            return this;
        }

        public IComponentProvider BuildProvider(IEnumerable<Type> entities)
        {
            Dictionary<Type, ComponentDescriptor[]> configurations = entities.ToDictionary(
                keySelector: t => t,
                elementSelector: t => this.Where(cd => cd.EntityType.IsAssignableFrom(t)).ToArray());

            return new ComponentProvider(configurations);
        }
    }
}
