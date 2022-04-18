using Guppy.EntityComponent.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    internal sealed class EntityComponentFilters
    {
        public readonly Type EntityType;
        public readonly ComponentDefinition ComponentDescriptor;
        public readonly ComponentFilter[] Filters;

        public EntityComponentFilters(Type entityType, ComponentDefinition component, ComponentFilter[] filters)
        {
            this.EntityType = entityType;
            this.ComponentDescriptor = component;
            this.Filters = filters;
        }

        public bool Filter(IServiceProvider provider, IEntity entity)
        {
            bool result = true;

            foreach(ComponentFilter filter in this.Filters)
            {
                result &= filter.EntityFilter(provider, entity, this.ComponentDescriptor);
            }

            return result;
        }
    }
}
