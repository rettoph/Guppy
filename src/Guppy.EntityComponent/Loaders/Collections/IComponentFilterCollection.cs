using Guppy.EntityComponent.Loaders.Definitions;
using Guppy.EntityComponent.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Collections
{
    public interface IComponentFilterCollection : IList<ComponentFilterDescriptor>
    {
        IComponentFilterCollection Add<TComponent>(ComponentFilterDescriptor.EntityFilterDelegate entityFilter, ComponentFilterDescriptor.TypeFilterDelegate typeFilter)
             where TComponent : IComponent;

        IComponentFilterCollection Add<TDefinition>()
            where TDefinition : ComponentFilterDefinition;
    }
}
