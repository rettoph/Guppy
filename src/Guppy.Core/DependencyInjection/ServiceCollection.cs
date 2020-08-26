using Guppy.DependencyInjection.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// The primary service collection instance, used to register services
    /// within service loaders.
    /// </summary>
    public sealed class ServiceCollection
    {
        #region Private Fields
        internal List<ServiceFactoryData> factories;
        internal List<ServiceDescriptorData> services;
        internal List<ServiceConfiguration> configurations;
        #endregion

        #region Constructors
        internal ServiceCollection()
        {
            this.factories = new List<ServiceFactoryData>();
            this.services = new List<ServiceDescriptorData>();
            this.configurations = new List<ServiceConfiguration>();
        }
        #endregion

        #region Base Methods
        /// <summary>
        /// Create a new factory method to create an instance of a specified service.
        /// </summary>
        /// <param name="type">The lookup & return type of the factory method.</param>
        /// <param name="factory">The main factory method to create a new instance.</param>
        /// <param name="priority">Only the highest priority level for each lookup type will be saved post initialization.</param>
        public void AddFactory(Type type, Func<ServiceProvider, Object> factory, Int32 priority = 10)
            => this.factories.Add(new ServiceFactoryData()
            {
                Type = type,
                Factory = factory,
                Priority = priority
            });

        public void AddFactory<T>(Func<ServiceProvider, T> factory, Int32 priority = 10)
            => this.AddFactory(typeof(T), p => factory.Invoke(p), priority);

        /// <summary>
        /// Create a new service with a unique name.
        /// </summary>
        /// <param name="name">The primary lookup name for this service.</param>
        /// <param name="factory">The factory lookup type to utilize when creating a new service instance.</param>
        /// <param name="lifetime">The service lifetime.</param>
        /// <param name="priority">Only the highest priority level for each lookup name will be saved post initialization.</param>
        /// <param name="cacheType">Optional: When the lifetime is singleton or scoped this allows for a custom lookup type. If none is defined the factory type will be used instead.</param>
        public void AddService(String name, Type factory, ServiceLifetime lifetime, Int32 priority = 10, Type cacheType = null)
            => this.services.Add(new ServiceDescriptorData()
            {
                Name = name,
                Factory = factory,
                Lifetime = lifetime,
                Priority = priority,
                CacheType = cacheType
            });

        /// <summary>
        /// Add a new custom configuration for a service that will
        /// automatically be excecuted when a new instance is pulled.
        /// </summary>
        /// <param name="service">The service name (or partial name) that this configuration should be applied to.</param>
        /// <param name="configuration">The method to run.</param>
        /// <param name="order">The order in which this particular configuration should run.</param>
        public void AddConfiguration(ServiceConfigurationKey key, Action<Object, ServiceProvider, ServiceDescriptor> configuration, Int32 order = 0)
            => this.configurations.Add(new ServiceConfiguration(key, configuration, order));
        #endregion

        #region Helper Methods
        /// <summary>
        /// Return a brand new service provider based on
        /// the current service collection
        /// </summary>
        /// <returns></returns>
        public ServiceProvider BuildServiceProvider()
            => new ServiceProvider(this);
        #endregion
    }
}
