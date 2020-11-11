using Guppy.DependencyInjection.Descriptors;
using Guppy.Extensions.Collections;
using Guppy.Extensions.System;
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
        private Dictionary<UInt32, ServiceConfiguration> _services;
        private Dictionary<Type, Object> _singletonInstances;
        private Dictionary<Type, Object> _scopedInstances;
        #endregion

        #region Constructors
        internal ServiceProvider(ServiceCollection services)
        {
            _singletonInstances = new Dictionary<Type, Object>();
            _scopedInstances = new Dictionary<Type, Object>();

            // First, convert all ServiceDescriptors into valid ServiceFactories & ServiceConfigurations
            services.Descriptors.ForEach(sd =>
            {
                // Define a default factory for the service...
                services.Add(new ServiceFactoryDescriptor(
                    type: sd.ServiceType, 
                    factory: p => sd.ImplementationFactory(p) ?? ActivatorUtilities.CreateInstance(p, sd.ImplementationType ?? sd.ServiceType),
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
            var factories = services.Factories.GroupBy(f => f.Type)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => new ServiceFactory(g.OrderBy(fd => fd.Priority).Last()));

            // Finally, sort the defined configuration descriptors and create configuration instances...
            _services = services.Configurations.GroupBy(cd => cd.Id)
                .ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.OrderBy(cd => cd.Priority).Last().As<ServiceConfigurationDescriptor, ServiceConfiguration>(cd =>
                    {
                        var factory = factories[cd.Factory];

                        return new ServiceConfiguration(
                            descriptor: cd,
                            factory: factory,
                            actions: services.Actions.Where(sa => sa.Key.Includes(factory.ImplementationType, cd.Name)));
                    }));
        }
        internal ServiceProvider(ServiceProvider parent)
        {
            _services = parent._services;
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
        #endregion

        #region IServiceProvider Implementation
        Object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
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
