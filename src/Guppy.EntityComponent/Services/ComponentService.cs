using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class ComponentService : IComponentService
    {
        private readonly IEntity _entity;
        private readonly Dictionary<Type, IComponent> _components;

        public ComponentService(IEntity entity, int capacity)
        {
            _entity = entity;
            _components = new Dictionary<Type, IComponent>(capacity);
        }

        public void Dispose()
        {
            foreach(IComponent component in _components.Values)
            {
                component.Dispose();
            }
        }

        public void Add<T>(T component)
            where T : class, IComponent
        {
            if (_components.TryAdd(typeof(T), component))
            {
                component.Initialize(_entity);
            }
        }

        public void Add(Type type, IComponent component)
        {
            if (_components.TryAdd(type, component))
            {
                component.Initialize(_entity);
            }
        }

        public void Remove<T>()
            where T : class, IComponent
        {
            if(_components.Remove(typeof(T), out IComponent? instance))
            {
                instance.Uninitilaize();
            }
        }

        public T Get<T>() 
            where T : class, IComponent
        {
            return (T)_components[typeof(T)];
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

        public IEnumerator<IComponent> GetEnumerator()
        {
            return _components.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
