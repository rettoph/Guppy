using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions.ComponentFilters
{
    internal sealed class RuntimeComponentFilterDefinition : ComponentFilterDefinition
    {
        public override Type ComponentType { get; }

        private readonly Func<IServiceProvider, IEntity, ComponentDefinition, bool> _entityFilter;
        private readonly Func<Type, ComponentDefinition, bool> _typeFilter;

        public RuntimeComponentFilterDefinition(Type componentType, Func<IServiceProvider, IEntity, ComponentDefinition, bool> entityFilter, Func<Type, ComponentDefinition, bool> typeFilter)
        {
            _entityFilter = entityFilter;
            _typeFilter = typeFilter;

            this.ComponentType = componentType;
        }

        public override ComponentFilter BuildComponentFilter()
        {
            return new ComponentFilter(this.ComponentType, _entityFilter, _typeFilter);
        }
    }
}
