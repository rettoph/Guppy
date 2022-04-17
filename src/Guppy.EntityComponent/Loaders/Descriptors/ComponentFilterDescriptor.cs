using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Descriptors
{
    public sealed class ComponentFilterDescriptor
    {
        public delegate bool EntityFilterDelegate(IEntity entity, IServiceProvider provider, ComponentDescriptor descriptor);
        public delegate bool TypeFilterDelegate(Type entity, ComponentDescriptor descriptor);


        public readonly Type AssignableComponentType;
        public readonly EntityFilterDelegate EntityFilter;
        public readonly TypeFilterDelegate TypeFilter;

        private ComponentFilterDescriptor(
            Type assignableComponentType, 
            EntityFilterDelegate entityFilter, 
            TypeFilterDelegate typeFilter)
        {
            this.AssignableComponentType = assignableComponentType;
            this.EntityFilter = entityFilter;
            this.TypeFilter = typeFilter;
        }

        public static ComponentFilterDescriptor Create<TComponent>(EntityFilterDelegate entityFilter, TypeFilterDelegate typeFilter)
             where TComponent : IComponent
        {
            return new ComponentFilterDescriptor(typeof(TComponent), entityFilter, typeFilter);
        }
    }
}
