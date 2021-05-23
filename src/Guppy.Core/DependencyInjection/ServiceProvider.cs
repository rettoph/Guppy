using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.System;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceProvider : IServiceProvider, IDisposable
    {
        #region Private Fields
        private Dictionary<Type, TypeFactory> _typeFactories;
        private Dictionary<ServiceConfigurationKey, ServiceConfiguration> _serviceConfigurationsSource;
        private Dictionary<ServiceConfigurationKey, ServiceConfiguration> _serviceConfigurations;
        private Dictionary<ServiceConfigurationKey, ComponentConfiguration[]> _componentConfigurations;

        private Dictionary<ServiceConfigurationKey, Object> _singletonInstances;
        private Dictionary<ServiceConfigurationKey, Object> _scopedInstances;

        private Boolean _disposing;
        private ServiceConfiguration _configuration;
        #endregion

        #region Public Properties
        public IReadOnlyDictionary<ServiceConfigurationKey, ServiceConfiguration> ServiceConfigurations => _serviceConfigurations;
        public IReadOnlyDictionary<ServiceConfigurationKey, Object> SingletonInstances => _singletonInstances;

        public IReadOnlyDictionary<ServiceConfigurationKey, Object> ScopedInstances => _scopedInstances;
        public IReadOnlyDictionary<ServiceConfigurationKey, ComponentConfiguration[]> ComponentConfigurations => _componentConfigurations;
        
        public IReadOnlyDictionary<UInt32, ServiceConfigurationKey> ServiceConfigurationKeys { get; private set; }
        #endregion

        #region Constructor
        internal ServiceProvider(ServiceCollection services)
        {
            // First, convert all ServiceDescriptors into valid TypeFactories & ServiceConfigurations
            services.ServiceDescriptors.ForEach(sd =>
            {
                // Define a default factory for the service...
                services.Add(new TypeFactoryDescriptor(
                    type: sd.ServiceType,
                    method: (p, t) => sd.ImplementationFactory?.Invoke(p) ?? sd.ImplementationInstance ?? ActivatorUtilities.CreateInstance(p, t)));

                // Define a default configuration for the service...
                services.Add(new ServiceConfigurationDescriptor(
                    key: ServiceConfigurationKey.From(sd.ServiceType),
                    lifetime: sd.Lifetime));
            });

            // Next, sort the defined factories descriptors and create factory instances...
            _typeFactories = services.TypeFactories.GroupBy(f => f.Type)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => new TypeFactory(g.OrderBy(fd => fd.Priority).Last()));

            // Then, sort the defined configuration descriptors and create configuration instances...
            _serviceConfigurationsSource = services.ServiceConfigurations.GroupBy(cd => cd.Key)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.OrderBy(cd => cd.Priority).Last().As<ServiceConfigurationDescriptor, ServiceConfiguration>(cd =>
                    {
                        var typeFactory = _typeFactories[cd.TypeFactory];

                        return new ServiceConfiguration(
                            descriptor: cd,
                            typeFactory: typeFactory,
                            actions: services.ServiceActions.Where(sa => cd.Key.Inherits(sa.Key)).OrderBy(sa => sa.Order));
                    }));

            // Finally, create a map of all valid ComponentConfigurations based on applicable IEntity service keys.
            _componentConfigurations = _serviceConfigurationsSource.Keys
                .Where(k => typeof(IEntity).IsAssignableFrom(k.Type))
                .ToDictionary(
                    key => key,
                    key => services.ComponentConfigurationDescriptors
                            .Where(desc => key.Inherits(desc.EntityServiceConfigurationKey))
                            .Select(desc =>
                            {
                                var configuration = _serviceConfigurationsSource[desc.ComponentServiceConfigurationKey];
                                var filters = services.ComponentFilters.Where(filter =>
                                {
                                    return configuration.Key.Inherits(filter.ComponentServiceConfigurationKey)
                                        && filter.Validator(configuration, _serviceConfigurationsSource[key]);
                                });
                                return new ComponentConfiguration(desc, configuration, filters);
                            })
                            .ToArray()
                    );

            _scopedInstances = new Dictionary<ServiceConfigurationKey, Object>();
            _singletonInstances = new Dictionary<ServiceConfigurationKey, Object>();
            _serviceConfigurations = new Dictionary<ServiceConfigurationKey, ServiceConfiguration>(_serviceConfigurationsSource);

            this.ServiceConfigurationKeys = this._serviceConfigurationsSource.ToDictionary(
                keySelector: kvp => kvp.Key.Id,
                elementSelector: kvp => kvp.Key);
        }

        internal ServiceProvider(ServiceProvider parent)
        {
            _typeFactories = parent._typeFactories;
            _serviceConfigurationsSource = parent._serviceConfigurationsSource;
            _componentConfigurations = parent._componentConfigurations;
            this.ServiceConfigurationKeys = parent.ServiceConfigurationKeys;

            _singletonInstances = parent._singletonInstances;
            _scopedInstances = new Dictionary<ServiceConfigurationKey, Object>();
            //_serviceConfigurations = new Dictionary<ServiceConfigurationKey, ServiceConfiguration>(_serviceConfigurationsSource);


            // TODO: Find solution that allows for the precense of singleton lookups created in alternate scopes.

            _serviceConfigurations = new Dictionary<ServiceConfigurationKey, ServiceConfiguration>(
                parent._serviceConfigurations.Where(sc => sc.Value.Lifetime == ServiceLifetime.Singleton)
                    .Union(_serviceConfigurationsSource)
                    .GroupBy(kvp => kvp.Key)
                    .Select(g => g.First())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
            );
        }
        #endregion

        #region Methods
        public Object GetService(
            ServiceConfigurationKey key,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
        {
            // Implement some backwards compatibility with Microsoft's DI 
            // For more info visit: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L95

            if (!_serviceConfigurations.TryGetValue(key, out _configuration))
                if (!_serviceConfigurations.TryGetValue(key.TryGetGenericKey(out generics), out _configuration))
                     return this.TryCreateEnumerable(key, setup, setupOrder);

            return _configuration.GetInstance(this, setup, setupOrder, generics);
        }


        /// <summary>
        /// Loosly based on Microsoft's DI.
        /// See original here: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L130
        /// </summary>
        /// <param name="key"></param>
        /// <param name="setup"></param>
        /// <param name="setupOrder"></param>
        /// <returns></returns>
        private IEnumerable<Object> TryCreateEnumerable(
            ServiceConfigurationKey key, 
            Action<Object, ServiceProvider, ServiceConfiguration> setup = null, 
            Int32 setupOrder = 0)
        {
            if (key.Type.IsConstructedGenericType &&
                key.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return _serviceConfigurations.Values
                    .Where(sd => sd.TypeFactory.Type.IsAssignableFrom(key.Type.GenericTypeArguments[0]))
                    .Select(sd => sd.GetInstance(this, setup, setupOrder));

            return default;
        }
        #endregion

        #region IServiceProvider Implementation
        Object IServiceProvider.GetService(Type serviceType)
            => this.GetService(ServiceConfigurationKey.From(serviceType));
        #endregion

        #region IDisposable Implementation
        public void Dispose(Boolean singletons)
        {
            if (_disposing)
                return;

            _disposing = true;

            _scopedInstances.Values.ToArray().ForEach(s =>
            {
                // Auto dispose all scoped instances.
                if (s is IDisposable d)
                    d.Dispose();
            });

            if (singletons)
            {
                _singletonInstances.Values.ForEach(s =>
                {
                    // Auto dispose all scoped instances.
                    if (s is IDisposable d)
                        d.Dispose();
                });
                _singletonInstances.Clear();
            }
            _scopedInstances.Clear();
            _disposing = false;
        }
        public void Dispose()
            => this.Dispose(false);
        #endregion

        #region Helper Methods
        internal void CacheScopedInstance(ServiceConfiguration configuration, Object instance, IEnumerable<ServiceConfigurationKey> lookups)
        {
            _scopedInstances.Add(configuration.Key, instance);

            foreach(ServiceConfigurationKey lookup in lookups.Where(key => key != configuration.Key))
                _serviceConfigurations.Add(lookup, configuration);
        }

        internal void CacheSingletonInstance(ServiceConfiguration configuration, Object instance, IEnumerable<ServiceConfigurationKey> lookups)
        {
            _singletonInstances.Add(configuration.Key, instance);

            foreach (ServiceConfigurationKey lookup in lookups.Where(key => key != configuration.Key))
                _serviceConfigurations.Add(lookup, configuration);
        }

        internal Boolean TryGetScopedInstance(ServiceConfigurationKey key, out Object instance)
            => _scopedInstances.TryGetValue(key, out instance);

        internal Boolean TryGetSingletonInstance(ServiceConfigurationKey key, out Object instance)
            => _singletonInstances.TryGetValue(key, out instance);

        public TypeFactory GetTypeFactory(Type type)
            => _typeFactories[type];
        #endregion
    }
}
