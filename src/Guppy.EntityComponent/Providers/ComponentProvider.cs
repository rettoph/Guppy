using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    internal sealed class ComponentProvider : IComponentProvider
    {
        private Dictionary<Type, ComponentDescriptor[]> _configurations;

        public ComponentProvider(Dictionary<Type, ComponentDescriptor[]> configurations)
        {
            _configurations = configurations;
        }

        public IComponentService Create(Type entity, IServiceProvider provider)
        {
            Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();
            foreach(ComponentDescriptor descriptor in _configurations[entity])
            {
                components.Add(descriptor.ComponentType, descriptor.Factory(provider));
            }

            return new ComponentService(components);
        }
    }
}
