using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class ServiceCollection
    {
        #region Public Fields
        public HashSet<ServiceDescriptor> ServiceDescriptors;
        public HashSet<ConfigurationDescriptor> ConfigurationDescriptors;
        #endregion

        #region Events
        public event EventHandler<ServiceDescriptor> OnServiceAdded;
        public event EventHandler<ConfigurationDescriptor> OnConfigurationAdded;

        public event EventHandler<ServiceDescriptor> OnServiceRemoved;
        public event EventHandler<ConfigurationDescriptor> OnConfigurationRemoved;
        #endregion

        #region Constructor
        internal ServiceCollection()
        {
            this.ServiceDescriptors = new HashSet<ServiceDescriptor>();
            this.ConfigurationDescriptors = new HashSet<ConfigurationDescriptor>();
        }
        #endregion

        #region Helper Methods
        internal ServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(this);
        }
        #endregion

        #region Service Helper Methods
        public void AddSingleton<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = typeof(TService),
                CacheType = cacheType == null ? typeof(TService) : cacheType,
                Factory = (p) => factory(p),
                Priority = priority,
                Lifetime = ServiceLifetime.Singleton,
            });
        }
        public void AddSingleton<TService>(TService instance, Int32 priority = 0)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = typeof(TService),
                CacheType = typeof(TService),
                Factory = (p) => instance,
                Priority = priority,
                Lifetime = ServiceLifetime.Singleton,
            });
        }

        public void AddScoped<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = typeof(TService),
                CacheType = cacheType == null ? typeof(TService) : cacheType,
                Factory = (p) => factory(p),
                Priority = priority,
                Lifetime = ServiceLifetime.Scoped,
            });
        }

        public void AddTransient<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = typeof(TService),
                Factory = (p) => factory(p),
                Priority = priority,
                Lifetime = ServiceLifetime.Transient,
            });
        }
        #endregion

        #region Configuration Helper Methods
        /// <summary>
        /// Add a custom configuration that can be used to 
        /// generate specific instances of a certain type.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(String name, Func<TService, ServiceProvider, Configuration, TService> configure, Int32 priority = 0) 
        {
            this.ConfigurationDescriptors.Add(new ConfigurationDescriptor()
            {
                Name = name,
                Configure = (i, p, c) => configure((TService)i, p, c),
                ServiceType = typeof(TService),
                Priority = priority,
            });
        }
        /// <summary>
        /// Add global configuration that will be applied to all
        /// instances of ServiceType<TService>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(Func<TService, ServiceProvider, Configuration, TService> configure, Int32 priority = 0)
        {
            this.AddConfiguration<TService>(String.Empty, configure, priority);
        }
        #endregion
    }
}
