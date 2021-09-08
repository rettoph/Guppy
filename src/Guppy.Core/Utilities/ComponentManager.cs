using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities
{
    public class ComponentManager : Service
    {
        #region Private Fields
        private Dictionary<ServiceConfigurationKey, IComponent> _components;
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
            _components = components.ToDictionary(
                keySelector: c => c.ServiceConfiguration.Key,
                elementSelector: c => c);
        }
        #endregion

        #region Get Methods
        public TComponent Get<TComponent>()
            where TComponent : class, IComponent
        {
            return this.Get<TComponent>(ServiceConfigurationKey.From<TComponent>());
        }
        public TComponent Get<TComponent>(ServiceConfigurationKey key)
            where TComponent : class, IComponent
        {
            if (_components.TryGetValue(key, out IComponent component) && component is TComponent casted)
            {
                return casted;
            }

            return default;
        }
        #endregion
    }
}
