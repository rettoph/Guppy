using Guppy.EntityComponent;
using Guppy.EntityComponent.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Definitions
{
    public abstract class ComponentFilterDefinition
    {
        public abstract Type ComponentType { get; }

        public abstract bool EntityFilter(IEntity entity, IServiceProvider provider, ComponentDescriptor descriptor);
        public abstract bool TypeFilter(Type entity, ComponentDescriptor descriptor);

        public abstract ComponentFilterDescriptor BuildDescriptor();
    }

    public abstract class ComponentFilterDefinition<TComponent> : ComponentFilterDefinition
        where TComponent : IComponent
    {
        public override Type ComponentType => typeof(TComponent);

        public override ComponentFilterDescriptor BuildDescriptor()
        {
            return ComponentFilterDescriptor.Create<TComponent>(this.EntityFilter, this.TypeFilter);
        }
    }
}
