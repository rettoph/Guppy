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
using Guppy.EntityComponent.Loaders.Definitions;

namespace Guppy.EntityComponent.Initializers.Collections
{
    internal sealed class ComponentCollection : List<ComponentDescriptor>, IComponentCollection
    {
        public ComponentCollection(IEnumerable<ComponentDescriptor> collection) : base(collection)
        {
        }

        public IComponentCollection Add<TEntity, TComponent>(Func<IServiceProvider, TEntity, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            this.Add(ComponentDescriptor.Create<TEntity, TComponent>(factory));

            return this;
        }

        /// <summary>
        /// Attempt to utilize <see cref="ActivatorUtilities.CreateFactory(Type, Type[])"/>
        /// to use the given <typeparamref name="TEntity"/> in the constructor. This will break
        /// if there is no valid constructor.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public IComponentCollection Add<TEntity, TComponent>()
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            this.Add(ComponentDescriptor.Create(ActivatorUtilitiesHelper.BuildFactory<TEntity, TComponent>()));

            return this;
        }

        public IComponentCollection Add<TDefinition>()
            where TDefinition : ComponentDefinition
        {
            var definition = Activator.CreateInstance<TDefinition>();
            this.Add(definition.BuildDescriptor());

            return this;
        }

        public IComponentProvider BuildProvider(IEnumerable<Type> entities, IComponentFilterCollection filters)
        {
            Dictionary<Type, EntityComponentsDescriptor[]> entityComponentDescriptors = new Dictionary<Type, EntityComponentsDescriptor[]>(entities.Count());
            List<EntityComponentsDescriptor> entityComponentDescriptorsList = new List<EntityComponentsDescriptor>();

            foreach(Type entity in entities)
            {
                foreach(ComponentDescriptor componentDescriptor in this)
                {
                    if(componentDescriptor.EntityType.IsAssignableFrom(entity))
                    {
                        var entityComponentDescriptor = new EntityComponentsDescriptor(
                            entity,
                            componentDescriptor,
                            filters.Where(x => x.AssignableComponentType.IsAssignableFrom(componentDescriptor.ComponentType) && x.TypeFilter(entity, componentDescriptor)).ToArray());

                        entityComponentDescriptorsList.Add(entityComponentDescriptor);
                    }
                }

                entityComponentDescriptors.Add(entity, entityComponentDescriptorsList.ToArray());
                entityComponentDescriptorsList.Clear();
            }

            return new ComponentProvider(entityComponentDescriptors);
        }
    }
}
