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
        public readonly Func<IServiceProvider, IEntity, IComponent> Factory;

        private ComponentDescriptor(
            Type entityType, 
            Type componentType, 
            Func<IServiceProvider, IEntity, IComponent> factory)
        {
            this.EntityType = entityType;
            this.ComponentType = componentType;
            this.Factory = factory;
        }

        public static ComponentDescriptor Create<TEntity, TComponent>(
            Func<IServiceProvider, TEntity, TComponent> factory)
                where TEntity : IEntity
                where TComponent : IComponent
        {
            return new ComponentDescriptor(
                typeof(TEntity),
                typeof(TComponent),
                (p, e) => factory(p, (TEntity)e));
        }
    }
}
