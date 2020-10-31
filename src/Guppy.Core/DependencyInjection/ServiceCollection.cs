using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Structs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// The primary service collection instance, used to register services
    /// within service loaders.
    /// </summary>
    public sealed partial class ServiceCollection : IServiceCollection
    {
        #region Private Fields
        private IList<ServiceDescriptor> _descriptors => this.factories.Select(fd => fd.Descriptor).ToList();
        #endregion

        #region Internal Fields
        internal List<ServiceFactoryData> factories;
        internal List<ServiceContextData> services;
        internal List<ServiceConfiguration> configurations;
        internal List<ServiceBuilder> builders;
        #endregion

        #region Constructors
        internal ServiceCollection()
        {
            this.factories = new List<ServiceFactoryData>();
            this.services = new List<ServiceContextData>();
            this.configurations = new List<ServiceConfiguration>();
            this.builders = new List<ServiceBuilder>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Create and add a brand new factory.
        /// This will auto generate a default
        /// ServiceContext.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="priority"></param>
        public void AddFactory(ServiceDescriptor item, Int32 priority)
        {
            this.factories.Add(new ServiceFactoryData()
            {
                Descriptor = item,
                Priority = priority
            });

            // Add a default context for this factory...
            this.AddContext(name: item.ServiceType.FullName, factory: item.ImplementationType ?? item.ServiceType, priority: priority);
        }

        /// <summary>
        /// Create a new ServiceContext with a unique name.
        /// </summary>
        /// <param name="name">The primary lookup name for this service.</param>
        /// <param name="factory">The factory implementation type to utilize when creating a new service instance.</param>
        /// <param name="lifetime">The service lifetime If null the factory lifetime will be used instead.</param>
        /// <param name="priority">Only the highest priority level for each lookup name will be saved post initialization.</param>
        /// <param name="serviceType">(Optional) When the lifetime is singleton or scoped this allows for a custom lookup type. If none is defined the factory type will be used instead.</param>
        /// <param name="autoBuild">(Optional) When true, scoped and singleton instances will automatically be created.</param>
        public void AddContext(String name, Type factory, ServiceLifetime? lifetime = null, Int32 priority = 10, Type serviceType = null, Boolean autoBuild = false)
            => this.services.Add(new ServiceContextData()
            {
                Name = name,
                Factory = factory,
                ServiceType = serviceType ?? factory,
                Lifetime = lifetime,
                Priority = priority,
                AutoBuild = autoBuild
            });

        /// <summary>
        /// Add a new custom configuration for a service that will
        /// automatically be excecuted when a new instance is pulled.
        /// </summary>
        /// <param name="service">The service name (or partial name) that this configuration should be applied to.</param>
        /// <param name="configuration">The method to run.</param>
        /// <param name="order">The order in which this particular configuration should run.</param>
        public void AddConfiguration(ServiceConfigurationKey key, Action<Object, ServiceProvider, ServiceContext> configuration, Int32 order = 0)
            => this.configurations.Add(new ServiceConfiguration(key, configuration, order));

        /// <summary>
        /// Add a new PostBuilder. These are one time methods that are executed only once
        /// after a factory builds a new instance. This should be used for one time
        /// setup that will never change, unlike configurations which will be
        /// excecuted every time a new instance is pulled.
        /// </summary>
        /// <param name="factory">The factory this builder should be applied to.</param>
        /// <param name="builder">The builder method itself.</param>
        /// <param name="order">The order in which this particular builder should run.</param>
        public void AddBuilder(Type factory, Action<Object, ServiceProvider> builder, Int32 order = 0)
            => this.builders.Add(new ServiceBuilder(factory, builder, order));

        /// <summary>
        /// Return a brand new ServiceProvider instance based on the current
        /// collection values.
        /// </summary>
        /// <returns></returns>
        public ServiceProvider BuildServiceProvider()
            => new ServiceProvider(this);
        #endregion

        #region IServiceCollection Implementation
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => this.factories[index].Descriptor; set => this.factories[index] = new ServiceFactoryData() { Descriptor = value, Priority = this.factories[index].Priority }; }

        int ICollection<ServiceDescriptor>.Count => this.factories.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => false;

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
            => this.AddFactory(item, 0);

        void ICollection<ServiceDescriptor>.Clear()
            => this.factories.Clear();

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
            => this.factories.Any(fd => fd.Descriptor == item);

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => _descriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
            => _descriptors.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _descriptors.GetEnumerator();

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
            => _descriptors.IndexOf(item);

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
            => this.factories.Insert(index, new ServiceFactoryData()
            {
                Descriptor = item
            });

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
            => this.factories.Remove(this.factories.First(fd => fd.Descriptor == item));

        void IList<ServiceDescriptor>.RemoveAt(int index)
            => this.factories.RemoveAt(index);
        #endregion
    }
}
