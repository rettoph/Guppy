using Guppy.EntityComponent;
using Guppy.EntityComponent.Loaders.Descriptors;
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
        private Dictionary<Type, EntityComponentsDescriptor[]> _descriptors;

        public ComponentProvider(Dictionary<Type, EntityComponentsDescriptor[]> descriptors)
        {
            _descriptors = descriptors;
        }

        public IComponentService Create(IEntity entity, IServiceProvider provider)
        {
            EntityComponentsDescriptor[] descriptors = _descriptors[entity.GetType()];
            Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>(descriptors.Length);
            foreach(EntityComponentsDescriptor descriptor in descriptors)
            {
                if(descriptor.Filter(entity, provider))
                {
                    IComponent component = descriptor.ComponentDescriptor.Factory(provider, entity);

                    components.Add(descriptor.ComponentDescriptor.ComponentType, component);
                }
            }

            return new ComponentService(components);
        }
    }
}
