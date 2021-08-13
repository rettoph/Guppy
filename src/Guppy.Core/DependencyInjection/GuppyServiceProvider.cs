using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Events.Delegates;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Guppy.DependencyInjection
{
    public partial class GuppyServiceProvider : IDisposable
    {
        #region Private Fields
        private Boolean _disposing;

        private GuppyServiceProvider _parent;
        private List<GuppyServiceProvider> _children;

        private Dictionary<Type, ITypeFactory> _typeFactories;
        private Dictionary<ServiceConfigurationKey, IServiceConfiguration> _registeredServices;
        private Dictionary<ServiceConfigurationKey, IServiceManager> _activeServices;

        private Dictionary<ServiceConfigurationKey, ComponentConfiguration[]> _componentConfigurations;

        private Dictionary<UInt32, ServiceConfigurationKey> _serviceConfigurationKeys;
        #endregion

        #region Public Fields
        public readonly Boolean IsRoot;
        public readonly GuppyServiceProvider Root;
        #endregion

        #region Public Properties
        public Dictionary<Type, ITypeFactory> TypeFactories => _typeFactories;
        public Dictionary<ServiceConfigurationKey, IServiceConfiguration> RegisteredServices => _registeredServices;
        public Dictionary<ServiceConfigurationKey, IServiceManager> ActiveServices => _activeServices;
        public Dictionary<ServiceConfigurationKey, ComponentConfiguration[]> ComponentConfigurations => _componentConfigurations;
        public Dictionary<UInt32, ServiceConfigurationKey> ServiceConfigurationKeys => _serviceConfigurationKeys;
        #endregion

        #region Events
        public event OnEventDelegate<GuppyServiceProvider, IServiceConfiguration, Type[]> OnServiceActivated;
        #endregion

        #region Constructors
        internal GuppyServiceProvider(GuppyServiceCollection services)
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
                .ToDictionary(g => g.Key, g => g.First());

            // Convert the internal ComponentConfigurationDtos into live ComponentConfigurations
            var componentConfigurations = services.ComponentConfigurations
                .Select(c => c.CreateComponentConfiguration(_registeredServices, services.ComponentFilters));

            // Cache all relevant component configurations for each registered service
            _componentConfigurations = new Dictionary<ServiceConfigurationKey, ComponentConfiguration[]>();
            foreach (ServiceConfigurationKey key in _registeredServices.Keys.Where(k => k.Inherits(ServiceConfigurationKey.From<IEntity>())))
                _componentConfigurations[key] = componentConfigurations
                    .Where(c => key.Inherits(c.EntityServiceConfigurationKey))
                    .ToArray();

            // // Construct all ComponentConfiguration instances.
            // _componentConfigurations = services.ComponentConfigurations
            //     .Select(c => c.CreateComponentConfiguration(_registeredServices, services.ComponentFilters))
            //     .GroupBy(c => c.EntityServiceConfigurationKey)
            //     .ToDictionary(g => g.Key, g => g.ToArray());
            // 
            // // Add placeholders for every registered service type with no configurations
            // foreach (ServiceConfigurationKey key in _registeredServices.Keys.Where(k => k.Inherits(ServiceConfigurationKey.From<IEntity>())))
            //     if (!_componentConfigurations.ContainsKey(key))
            //         _componentConfigurations[key] = new ComponentConfiguration[0];

            _serviceConfigurationKeys = _registeredServices.Keys.ToDictionary(
                keySelector: key => key.Id,
                elementSelector: key => key);

            _activeServices = new Dictionary<ServiceConfigurationKey, IServiceManager>();

            this.IsRoot = true;
            this.Root = this;
            _children = new List<GuppyServiceProvider>();
        }
        internal GuppyServiceProvider(GuppyServiceProvider parent)
        {
            this.IsRoot = false;
            this.Root = parent.Root;
            _parent = parent;
            _children = new List<GuppyServiceProvider>();

            _typeFactories = parent._typeFactories;
            _registeredServices = parent._registeredServices;
            _componentConfigurations = parent._componentConfigurations;
            _serviceConfigurationKeys = parent._serviceConfigurationKeys;
            _activeServices = this.Root.ActiveServices
                .Where(kvp => kvp.Value.Configuration.Lifetime == ServiceLifetime.Singleton)
                .ToDictionary();
        }
        #endregion

        #region Helper Methods
        internal IServiceManager GetServiceManager(
            IServiceConfiguration configuration,
            Type[] generics)
        {
            var key = configuration.TypeFactory.MutateKey(configuration.Key, generics);

            IServiceManager manager;

            if (!_activeServices.TryGetValue(key, out manager))
                manager = this.ActivateService(configuration, generics);

            return manager;
        }

        public ITypeFactory GetTypeFactory(Type type)
            => _typeFactories[type];

        public GuppyServiceProvider CreateScope()
        {
            var child = new GuppyServiceProvider(this);
            _children.Add(child);

            return child;
        }

        private IServiceManager ActivateService(
            IServiceConfiguration configuration,
            Type[] generics)
        {
            IServiceManager manager = configuration.BuildServiceManager(this, generics);

            // Attempt to cache all configuration cache keys. These must succeed. If an overlap is detected an exception will be thrown.
            foreach (ServiceConfigurationKey cacheKey in configuration.TypeFactory.MutateKeys(configuration.DefaultCacheKeys, generics))
                _activeServices.Add(cacheKey, manager);

            // Invoke the listening event.
            this.OnServiceActivated?.Invoke(this, configuration, generics);

            return manager;
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            if (_disposing)
                return;

            _disposing = true;

            foreach (GuppyServiceProvider child in _children)
                child.Dispose();

            foreach (IServiceManager manager in _activeServices.Values)
                manager.Dispose();

            _parent = default;
            _children.Clear();
            _activeServices.Clear();
            _registeredServices.Clear();
            _activeServices.Clear();
            _componentConfigurations.Clear();
        }
        #endregion
    }
}
