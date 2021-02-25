using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

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
        private Boolean _disposing;
        private List<ServiceProvider> _children;
        #endregion

        #region Internal Properties
        internal ILog logger { get; private set; }
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

            this.logger = this.GetService<ILog>();
        }
        internal ServiceProvider(ServiceProvider parent)
        {
            _factories = parent._factories;
            _services = parent._services;

            _singletonInstances = parent._singletonInstances;
            _scopedInstances = new Dictionary<Type, Object>();
            _lookups = new Dictionary<UInt32, ServiceConfiguration>(parent._lookups);

            this.logger = this.GetService<ILog>();
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

            if(lookupType.IsInterface)
            { // Recersively add types until the interface is no longer implemented...
                while (type.GetInterfaces().Contains(lookupType))
                {
                    this.AddLookup(type, target);
                    type = type.BaseType;
                }

                // Add the recived interface...
                this.AddLookup(lookupType, target);
            }
            else
            { // Add types until the base type is hit...
                while (type != lookupType)
                {
                    this.AddLookup(type, target);
                    type = type.BaseType;
                }

                // Add the final lookup type...
                this.AddLookup(type, target);
            }
        }

        public void AddLookupRecursive<TLookup>(ServiceConfiguration target)
            => this.AddLookupRecursive(typeof(TLookup), target);
        #endregion
        #endregion

        #region IServiceProvider Implementation
        Object IServiceProvider.GetService(Type serviceType)
            => this.GetService(serviceType);
        #endregion

        #region IDisposable Implementation
        public void Dispose(Boolean singletons)
        {
            if (_disposing)
                return;

            _disposing = true;

            _scopedInstances.Values.ForEach(s =>
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
    }
}
