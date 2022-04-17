using Guppy.EntityComponent.Loaders.Collections;
using Guppy.EntityComponent.Loaders.Definitions;
using Guppy.EntityComponent.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Initializers.Collections
{
    internal sealed class ComponentFilterCollection : List<ComponentFilterDescriptor>, IComponentFilterCollection
    {
        public ComponentFilterCollection(IEnumerable<ComponentFilterDescriptor> collection) : base(collection)
        {
        }

        public IComponentFilterCollection Add<TComponent>(
            ComponentFilterDescriptor.EntityFilterDelegate entityFilter, 
            ComponentFilterDescriptor.TypeFilterDelegate typeFilter)
                where TComponent : IComponent
        {
            this.Add(ComponentFilterDescriptor.Create<TComponent>(entityFilter, typeFilter));

            return this;
        }

        public IComponentFilterCollection Add<TDefinition>()
            where TDefinition : ComponentFilterDefinition
        {
            var definition = Activator.CreateInstance<TDefinition>();
            this.Add(definition.BuildDescriptor());

            return this;
        }
    }
}
