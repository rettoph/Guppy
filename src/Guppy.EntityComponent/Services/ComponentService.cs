using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class ComponentService : IComponentService
    {
        private Dictionary<Type, IComponent> _components;

        public ComponentService(Dictionary<Type, IComponent> components)
        {
            _components = components;
        }

        public void Dispose()
        {
            foreach(IComponent component in _components.Values)
            {
                component.Dispose();
            }
        }

        public T? Get<T>() 
            where T : class, IComponent
        {
            return _components[typeof(T)] as T;
        }

        public bool Has<T>() 
            where T : class, IComponent
        {
            return _components.ContainsKey(typeof(T));
        }

        public bool TryGet<T>(out T? component)
            where T : class, IComponent
        {
            if(_components.TryGetValue(typeof(T), out IComponent? value))
            {
                component = value as T;
                return true;
            }

            component = default;
            return false;
        }
    }
}
