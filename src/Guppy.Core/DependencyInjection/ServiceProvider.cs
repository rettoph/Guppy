using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Events.Delegates;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guppy.DependencyInjection
{
    public partial class ServiceProvider : IDisposable
    {
        #region Private Fields
        private Boolean _disposing;

        private Dictionary<Type, ITypeFactory> _typeFactories;
        private Dictionary<ServiceConfigurationKey, IServiceConfiguration[]> _registeredServices;
        private Dictionary<ServiceConfigurationKey, IServiceManager> _primaryActiveService;
        private Dictionary<ServiceConfigurationServiceConfigurationKey, IServiceManager> _activeServices;

        private Dictionary<ServiceConfigurationKey, ComponentConfiguration[]> _componentConfigurations;
        #endregion

        #region Public Fields
        public readonly Boolean IsRoot;
        public readonly ServiceProvider Root;
        #endregion

        #region Public Properties
        public Dictionary<Type, ITypeFactory> TypeFactories => _typeFactories;
        public Dictionary<ServiceConfigurationKey, IServiceConfiguration[]> RegisteredServices => _registeredServices;
        public Dictionary<ServiceConfigurationKey, IServiceManager> PrimaryActiveServices => _primaryActiveService;
        public Dictionary<ServiceConfigurationServiceConfigurationKey, IServiceManager> ActiveServices => _activeServices;
        public Dictionary<ServiceConfigurationKey, ComponentConfiguration[]> ComponentConfigurations => _componentConfigurations;
        #endregion

        #region Constructors
        internal ServiceProvider(ServiceCollection services)
        {
            // Construct all relevant ITypeFactory instances.
            _typeFactories = services.TypeFactories
                .GroupBy(tf => tf.Type)
                .Select(g => g.OrderByDescending(tf => tf.Priority).First())
                .Select(tf => tf.CreateTypeFactory(services.BuilderActions))
                .ToDictionary(tf => tf.Type, tf => tf);

            // Construct all IServiceConfiguration instances.
            _registeredServices = services.ServiceConfigurations
                .OrderByDescending(sc => sc.Priority)
                .Select(sc => sc.CreateServiceConfiguration(_typeFactories, services.SetupActions))
                .GroupBy(sc => sc.Key)
                .ToDictionary(g => g.Key, g => g.ToArray());

            // Construct all ComponentConfiguration instances.
            _componentConfigurations = services.ComponentConfigurations
                .Select(c => c.CreateComponentConfiguration(_registeredServices, services.ComponentFilters))
                .GroupBy(c => c.EntityServiceConfigurationKey)
                .ToDictionary(g => g.Key, g => g.ToArray());

            // Add placeholders for every registered service type with no configurations
            foreach(ServiceConfigurationKey key in _registeredServices.Keys.Where(k => k.Inherits(ServiceConfigurationKey.From<IEntity>())))
                if (!_componentConfigurations.ContainsKey(key))
                    _componentConfigurations[key] = new ComponentConfiguration[0];

            _primaryActiveService = new Dictionary<ServiceConfigurationKey, IServiceManager>();
            _activeServices = new Dictionary<ServiceConfigurationServiceConfigurationKey, IServiceManager>();


            this.IsRoot = true;
            this.Root = this;
        }
        internal ServiceProvider(ServiceProvider parent)
        {
            this.IsRoot = false;
            this.Root = parent.Root;

            _typeFactories = parent._typeFactories;
            _registeredServices = parent._registeredServices;
            _componentConfigurations = parent._componentConfigurations;

            _primaryActiveService = new Dictionary<ServiceConfigurationKey, IServiceManager>();
            _activeServices = new Dictionary<ServiceConfigurationServiceConfigurationKey, IServiceManager>();
        }
        #endregion

        #region Helper Methods
        internal IServiceManager GetServiceManager(
            IServiceConfiguration configuration,
            Type[] generics)
        {
            var key = configuration.TypeFactory.MutateKey(configuration.Key, generics);
            var compKey = new ServiceConfigurationServiceConfigurationKey(configuration, key);

            IServiceManager manager;

            if (!_activeServices.TryGetValue(compKey, out manager))
            {
                manager = configuration.BuildServiceManager(this, generics);

                _activeServices.Add(compKey, manager);

                // Cache the primary service key if neccessary
                if(!_primaryActiveService.ContainsKey(key))
                    _primaryActiveService.Add(key, manager);
                    
                // Attempt to cache all configuration cache keys. These must succeed. If an overlap is detected an exception will be thrown.
                foreach(ServiceConfigurationKey cacheKey in configuration.TypeFactory.MutateKeys(configuration.DefaultCacheKeys, generics))
                    _primaryActiveService.Add(cacheKey, manager);
            }

            return manager;
        }

        public ITypeFactory GetTypeFactory(Type type)
            => _typeFactories[type];
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            if (_disposing)
                return;

            _disposing = true;

            foreach (IServiceManager manager in _activeServices.Values)
                manager.Dispose();

            _primaryActiveService.Clear();
            _registeredServices.Clear();
            _activeServices.Clear();
            _componentConfigurations.Clear();
        }
        #endregion
    }
}
