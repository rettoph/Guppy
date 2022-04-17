using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Descriptors
{
    internal sealed class EntityComponentsDescriptor
    {
        public readonly Type Entity;
        public readonly ComponentDescriptor ComponentDescriptor;
        public readonly ComponentFilterDescriptor[] Filters;

        public EntityComponentsDescriptor(Type entity, ComponentDescriptor componentDescriptor, ComponentFilterDescriptor[] filters)
        {
            this.Entity = entity;
            this.ComponentDescriptor = componentDescriptor;
            this.Filters = filters;
        }

        public bool Filter(IEntity entity, IServiceProvider provider)
        {
            bool result = true;

            foreach(ComponentFilterDescriptor filter in this.Filters)
            {
                result &= filter.EntityFilter(entity, provider, this.ComponentDescriptor);
            }

            return result;
        }
    }
}
