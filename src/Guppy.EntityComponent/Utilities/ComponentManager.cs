using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.Utilities
{
    public class ComponentManager : Service
    {
        #region Private Fields
        private Dictionary<String, IComponent> _components;
        #endregion

        #region Lifecycle Methods
        protected override void Release()
        {
            base.Release();

            _components.Values.ForEach(c => c.TryRelease());
            _components = default;
        }
        #endregion

        #region Helper Methods
        internal void BuildDictionary(IEnumerable<IComponent> components)
        {
            // throw new NotImplementedException();
            _components = new Dictionary<String, IComponent>(
                components.SelectMany(static component =>
                {
                    return component.ServiceConfiguration.CacheNames
                        .Select(name => new KeyValuePair<String, IComponent>(name, component));
                })
            );
        }

        internal void Do(Action<IComponent> action)
        {
            foreach (IComponent component in _components.Values.Distinct())
                action(component);
        }
        #endregion

        #region Get Methods
        public TComponent Get<TComponent>()
            where TComponent : class, IComponent
        {
            return this.Get<TComponent>(typeof(TComponent).FullName);
        }
        public TComponent Get<TComponent>(String componentName)
            where TComponent : class, IComponent
        {
            if (_components.TryGetValue(componentName, out IComponent component) && component is TComponent casted)
            {
                return casted;
            }

            return default;
        }
        #endregion
    }
}
