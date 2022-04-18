using Guppy.EntityComponent.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public sealed class ComponentFilter
    {
        public readonly Type ComponentType;
        public readonly Func<IServiceProvider, IEntity, ComponentDefinition, bool> EntityFilter;
        public readonly Func<Type, ComponentDefinition, bool> TypeFilter;

        public ComponentFilter(Type componentType, Func<IServiceProvider, IEntity, ComponentDefinition, bool> entityFilter, Func<Type, ComponentDefinition, bool> typeFilter)
        {
            this.ComponentType = componentType;
            this.EntityFilter = entityFilter;
            this.TypeFilter = typeFilter;
        }
    }
}
