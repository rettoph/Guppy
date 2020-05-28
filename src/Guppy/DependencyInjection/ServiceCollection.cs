using Microsoft.Extensions.DependencyInjection;
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
        public void AddSingleton(Type serviceType, Func<ServiceProvider, Object> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = serviceType,
                CacheType = cacheType == null ? serviceType : cacheType,
                Factory = factory,
                Priority = priority,
                Lifetime = ServiceLifetime.Singleton,
            });
        }
        public void AddSingleton<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.AddSingleton(typeof(TService), p => factory(p), priority, cacheType);
        }
        public void AddSingleton(Type serviceType, Object instance, Int32 priority = 0)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = serviceType,
                CacheType = serviceType,
                Factory = p => instance,
                Priority = priority,
                Lifetime = ServiceLifetime.Singleton,
            });
        }
        public void AddSingleton<TService>(TService instance, Int32 priority = 0)
        {
            this.AddSingleton(typeof(TService), instance, priority);
        }

        public void AddScoped(Type serviceType, Func<ServiceProvider, Object> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = serviceType,
                CacheType = cacheType == null ? serviceType : cacheType,
                Factory = factory,
                Priority = priority,
                Lifetime = ServiceLifetime.Scoped,
            });
        }
        public void AddScoped<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0, Type cacheType = null)
        {
            this.AddScoped(typeof(TService), p => factory(p), priority, cacheType);
        }

        public void AddTransient(Type serviceType, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            this.ServiceDescriptors.Add(new ServiceDescriptor()
            {
                ServiceType = serviceType,
                Factory = factory,
                Priority = priority,
                Lifetime = ServiceLifetime.Transient,
            });
        }
        public void AddTransient<TService>(Func<ServiceProvider, TService> factory, Int32 priority = 0)
        {
            this.AddTransient(typeof(TService), p => factory(p), priority);
        }
        #endregion

        #region Configuration Helper Methods
        public void AddConfiguration(Type service, String name, Action<Object, ServiceProvider, ServiceConfiguration> configure, Int32 priority = 0)
        {
            this.ConfigurationDescriptors.Add(new ConfigurationDescriptor()
            {
                Name = name,
                Configure = configure,
                ServiceType = service,
                Priority = priority,
            });
        }
        /// <summary>
        /// Add a custom configuration that can be used to 
        /// generate specific instances of a certain type.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(String name, Func<TService, ServiceProvider, ServiceConfiguration, TService> configure, Int32 priority = 0) 
        {
            this.AddConfiguration(typeof(TService), name, (i, p, c) => configure((TService)i, p, c), priority);
        }

        /// <summary>
        /// Add a custom configuration that can be used to 
        /// generate specific instances of a certain type.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(String name, Action<TService, ServiceProvider, ServiceConfiguration> configure, Int32 priority = 0)
        {
            this.AddConfiguration<TService>(name, (i, p, c) =>
            {
                configure(i, p, c);
                return i;
            }, priority);
        }
        /// <summary>
        /// Add global configuration that will be applied to all
        /// instances of ServiceType<TService>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(Func<TService, ServiceProvider, ServiceConfiguration, TService> configure, Int32 priority = 0)
        {
            this.AddConfiguration<TService>(String.Empty, configure, priority);
        }
        /// <summary>
        /// Add global configuration that will be applied to all
        /// instances of ServiceType<TService>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="configure"></param>
        /// <param name="priority"></param>
        public void AddConfiguration<TService>(Action<TService, ServiceProvider, ServiceConfiguration> configure, Int32 priority = 0)
        {
            this.AddConfiguration<TService>(String.Empty, configure, priority);
        }
        #endregion
    }
}
