using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class ServiceProvider : IServiceProvider, IDisposable
    {
        #region Private Fields
        private Dictionary<Type, ServiceFactory> _factories;
        private Dictionary<UInt32, ServiceConfiguration> _services;
        private Dictionary<UInt32, ServiceConfiguration> _lookups;
        private Dictionary<Type, Object> _singletonInstances;
        private Dictionary<Type, Object> _scopedInstances;
        #endregion

        #region Constructors
        internal ServiceProvider(ServiceCollection services)
        {
            _lookups = new Dictionary<UInt32, ServiceConfiguration>();
            _singletonInstances = new Dictionary<Type, Object>();
            _scopedInstances = new Dictionary<Type, Object>();

            // First, convert all ServiceDescriptors into valid ServiceFactories & ServiceConfigurations
            services.Descriptors.ForEach(sd =>
            {
                // Define a default factory for the service...
                services.Add(new ServiceFactoryDescriptor(
                    type: sd.ServiceType, 
                    factory: (p, t) => sd.ImplementationFactory(p) ?? ActivatorUtilities.CreateInstance(p, t),
                    implementationType: sd.ImplementationType));

                // Define a default configuration for the service...
                services.Add(new ServiceConfigurationDescriptor(
                    name: sd.ServiceType.FullName, 
                    lifetime: sd.Lifetime, 
                    factory: sd.ServiceType));

                // Save the implimentation instance if valid...
                if (sd.Lifetime == ServiceLifetime.Singleton && sd.ImplementationInstance != default)
                    this.CacheSingletonInstance(sd.ServiceType, sd.ImplementationInstance);
            });

            // Next, sort the defined factories descriptors and create factory instances...
            _factories = services.Factories.GroupBy(f => f.Type)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => new ServiceFactory(g.OrderBy(fd => fd.Priority).Last()));

            // Finally, sort the defined configuration descriptors and create configuration instances...
            _services = services.Configurations.GroupBy(cd => cd.Id)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.OrderBy(cd => cd.Priority).Last().As<ServiceConfigurationDescriptor, ServiceConfiguration>(cd =>
                    {
                        var factory = _factories[cd.Factory];

                        return new ServiceConfiguration(
                            descriptor: cd,
                            factory: factory,
                            actions: services.Actions.Where(sa => sa.Key.Includes(factory.ImplementationType, cd.Name)).OrderBy(sa => sa.Order));
                    }));
        }
        internal ServiceProvider(ServiceProvider parent)
        {
            _factories = parent._factories;
            _services = parent._services;
            _lookups = new Dictionary<UInt32, ServiceConfiguration>();
            _singletonInstances = parent._singletonInstances;
            _scopedInstances = new Dictionary<Type, Object>();
        }
        #endregion

        #region Helper Methods
        internal void CacheScopedInstance(Type type, Object instance)
            => _scopedInstances.Add(type, instance);

        internal void CacheSingletonInstance(Type type, Object instance)
            => _singletonInstances.Add(type, instance);

        internal Boolean TryGetScopedInstance(Type type, out Object instance)
            => _scopedInstances.TryGetValue(type, out instance);

        internal Boolean TryGetSingletonInstance(Type type, out Object instance)
            => _singletonInstances.TryGetValue(type, out instance);

        public ServiceFactory GetServiceFactory(Type type)
            => _factories[type];

        public ServiceConfiguration GetServiceConfiguration(UInt32 id)
            => _services[id];
        public ServiceConfiguration GetServiceConfiguration(String name)
            => _services[ServiceConfiguration.GetId(name)];
        public ServiceConfiguration GetServiceConfiguration(Type type)
            => _services[ServiceConfiguration.GetId(type)];
        public ServiceConfiguration GetServiceConfiguration<T>()
            => _services[ServiceConfiguration.GetId<T>()];

        #region AddLookup Methods
        /// <summary>
        /// Create a virtual lookup target so that when a specific
        /// service is queried then an alternative may be 
        /// returned.
        /// </summary>
        /// <param name="lookupId"></param>
        /// <param name="target"></param>
        public void AddLookup(UInt32 lookupId, ServiceConfiguration target)
            => _lookups.Add(lookupId, target);

        /// <summary>
        /// Create a virtual lookup target so that when a specific
        /// service is queried then an alternative may be 
        /// returned.
        /// </summary>
        /// <param name="lookupName"></param>
        /// <param name="target"></param>
        public void AddLookup(String lookupName, ServiceConfiguration target)
            => this.AddLookup(ServiceConfiguration.GetId(lookupName), target);

        /// <summary>
        /// Create a virtual lookup target so that when a specific
        /// service is queried then an alternative may be 
        /// returned.
        /// </summary>
        /// <param name="lookupType"></param>
        /// <param name="target"></param>
        public void AddLookup(Type lookupType, ServiceConfiguration target)
            => this.AddLookup(ServiceConfiguration.GetId(lookupType), target);

        /// <summary>
        /// Create a virtual lookup target so that when a specific
        /// service is queried then an alternative may be 
        /// returned.
        /// </summary>
        /// <typeparam name="TLookup"></typeparam>
        /// <param name="target"></param>
        public void AddLookup<TLookup>(ServiceConfiguration target)
            => this.AddLookup(ServiceConfiguration.GetId<TLookup>(), target);

        public void AddLookupRecursive(Type lookupType, ServiceConfiguration target)
        {
            var type = target.Factory.ImplementationType;
            ExceptionHelper.ValidateAssignableFrom(lookupType, type);

            while(type != lookupType)
            {
                this.AddLookup(type, target);
                type = type.BaseType;
            }

            this.AddLookup(type, target);
        }

        public void AddLookupRecursive<TLookup>(ServiceConfiguration target)
            => this.AddLookupRecursive(typeof(TLookup), target);
        #endregion
        #endregion

        #region IServiceProvider Implementation
        Object IServiceProvider.GetService(Type serviceType)
        {
            // Implement some backwards compatibility with Microsoft's DI 
            // For more info visit: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs
            // Line 95
            return this.GetService(serviceType) ??
                this.TryCreateGeneric(serviceType) ??
                this.TryCreateEnumerable(serviceType);
        }

        private Object TryCreateGeneric(Type serviceType)
        {
            ServiceConfiguration configuration;
            if (serviceType.IsConstructedGenericType 
                && _services.TryGetValue(ServiceConfiguration.GetId(serviceType.GetGenericTypeDefinition()), out configuration))
            {
                return configuration.BuildInstance(provider: this, type: serviceType);
            }

            return null;
        }

        private Object TryCreateEnumerable(Type serviceType)
        {
            if (serviceType.IsConstructedGenericType &&
                serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return _services.Values
                    .Where(sd => sd.Factory.Type.IsAssignableFrom(serviceType.GenericTypeArguments[0]))
                    .Select(sd => sd.BuildInstance(this));

            return null;
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            _scopedInstances.Clear();
        }
        #endregion
    }
}
