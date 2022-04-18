using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions
{
    public abstract class ComponentFilterDefinition
    {
        public abstract Type ComponentType { get; }

        internal ComponentFilterDefinition()
        {

        }

        public abstract ComponentFilter BuildComponentFilter();
    }

    public abstract class ComponentFilterDefinition<TComponent> : ComponentFilterDefinition
        where TComponent : IComponent
    {
        public override Type ComponentType => typeof(TComponent);

        public abstract bool EntityFilter(IServiceProvider provider, IEntity entity, ComponentDefinition component);
        public abstract bool TypeFilter(Type entity, ComponentDefinition component);

        public override ComponentFilter BuildComponentFilter()
        {
            return new ComponentFilter(this.ComponentType, this.EntityFilter, this.TypeFilter);
        }
    }
}
