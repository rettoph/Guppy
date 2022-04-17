using Guppy.EntityComponent.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Definitions
{
    public abstract class ComponentDefinition
    {
        public abstract Type EntityType { get; }
        public abstract Type ComponentType { get; }

        public abstract ComponentDescriptor BuildDescriptor();
    }

    public abstract class ComponentDefinition<TEntity, TComponent> : ComponentDefinition
        where TEntity : IEntity
        where TComponent : IComponent
    {
        public override Type EntityType => typeof(TEntity);

        public override Type ComponentType => typeof(TComponent);

        public abstract TComponent Factory(IServiceProvider provider, TEntity entity);

        public override ComponentDescriptor BuildDescriptor()
        {
            return ComponentDescriptor.Create<TEntity, TComponent>(this.Factory);
        }
    }
}
