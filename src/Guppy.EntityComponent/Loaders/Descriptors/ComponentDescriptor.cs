using Guppy.EntityComponent;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Descriptors
{
    public sealed class ComponentDescriptor
    {
        public readonly Type EntityType;
        public readonly Type ComponentType;
        public readonly Func<IServiceProvider, IComponent> Factory;

        private ComponentDescriptor(
            Type entityType, 
            Type componentType, 
            Func<IServiceProvider, IComponent> factory)
        {
            this.EntityType = entityType;
            this.ComponentType = componentType;
            this.Factory = factory;
        }

        public static ComponentDescriptor Create<TEntity, TComponent>(
            Func<IServiceProvider, TComponent> factory)
                where TEntity : IEntity
                where TComponent : IComponent
        {
            return new ComponentDescriptor(
                typeof(TEntity),
                typeof(TComponent),
                p => factory(p));
        }

        public static ComponentDescriptor Create(
            Type entityType,
            Type componentType,
            Func<IServiceProvider, IComponent> factory)
        {
            typeof(IEntity).ValidateAssignableFrom(entityType);
            typeof(IComponent).ValidateAssignableFrom(componentType);

            return new ComponentDescriptor(
                entityType,
                componentType,
                factory);
        }
    }
}
