using Guppy.EntityComponent.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public static class IComponentDescriptorExtensions
    {
        internal static KeyValuePair<Type, IComponent> CreateKeyValuePair(this ComponentDescriptor descriptor, IServiceProvider provider)
        {
            IComponent component = descriptor.Factory(provider);

            return new KeyValuePair<Type, IComponent>(descriptor.ComponentType, component);
        }
    }
}
