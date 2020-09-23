using Guppy.DependencyInjection;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region AddConfiguration Methods
        /// <summary>
        /// Add a brand new configuration method utilizing 
        /// the recieved type and name for the inheritance key.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="order"></param>
        public static void AddConfiguration(this ServiceCollection services, Type type, String name, Action<Object, ServiceProvider, ServiceDescriptor> configuration, Int32 order = 0)
            => services.AddConfiguration(new ServiceConfigurationKey(type, name), configuration, order);

        /// <summary>
        /// Add a brand new configuration method utilizing 
        /// the generic type and name for the inheritance key.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="order"></param>
        public static void AddConfiguration<T>(this ServiceCollection services, String name, Action<T, ServiceProvider, ServiceDescriptor> configuration, Int32 order = 0)
            => services.AddConfiguration(new ServiceConfigurationKey(typeof(T), name), (i, p, s) => configuration.Invoke((T)i, p, s), order);

        /// <summary>
        /// Add a brand new configuration method utilizing 
        /// the generic type and name for the inheritance key.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="order"></param>
        public static void AddConfiguration<T>(this ServiceCollection services, Action<T, ServiceProvider, ServiceDescriptor> configuration, Int32 order = 0)
            => services.AddConfiguration(new ServiceConfigurationKey(typeof(T), String.Empty), (i, p, s) => configuration.Invoke((T)i, p, s), order);
        #endregion

        #region AddBuilder Methods
        public static void AddBuilder<TFactory>(this ServiceCollection services, Action<TFactory, ServiceProvider> builder, Int32 order = 0)
            => services.AddBuilder(typeof(TFactory), (i, p) => builder.Invoke((TFactory)i, p), order);
        #endregion

        #region Singleton Methods
        /// <summary>
        /// Create a new singleton service descriptor.
        /// </summary>
        /// <param name="name">The service name which will be used as the primary service key.</param>
        /// <param name="factory">The factory to be used when creating a new instance.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddSingleton(this ServiceCollection services, String name, Type factory, Int32 priority = 10, Type cacheType = null)
            => services.AddService(name, factory, ServiceLifetime.Singleton, priority, cacheType);

        /// <summary>
        /// Create a new nameless singleton service descriptor.
        /// </summary>
        /// <param name="type">The service type which will be used as the primary service key.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddSingleton(this ServiceCollection services, Type type, Type factory = null, Int32 priority = 10, Type cacheType = null)
            => services.AddSingleton(type.FullName, factory ?? type, priority, cacheType);

        /// <summary>
        /// Create a new nameless singleton service descriptor.
        /// </summary>
        /// <typeparam name="T">The service type which will be used as the primary service key.</typeparam>
        /// <param name="name">(Optional) The name to register this service to. If none is defined, the generic type will be used instead.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddSingleton<T>(this ServiceCollection services, String name = null, Type factory = null, Int32 priority = 10, Type cacheType = null)
        {
            if (name == null)
                services.AddSingleton(typeof(T), factory, priority, cacheType);
            else
                services.AddSingleton(name, factory ?? typeof(T), priority, cacheType);
        }

        /// <summary>
        /// Add a named instanced singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="priority"></param>
        public static void AddSingleton<T>(this ServiceCollection services, String name, T instance, Int32 priority = 10, Type cacheType = null)
        {
            services.AddFactory<T>(factory: p => instance, priority: priority);
            services.AddSingleton(
                name: name,
                factory: typeof(T),
                priority: priority,
                cacheType: cacheType);
        }

        /// <summary>
        /// Add a nameless instanced singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="priority"></param>
        public static void AddSingleton<T>(this ServiceCollection services, T instance, Int32 priority = 10, Type cacheType = null)
            => services.AddSingleton<T>(typeof(T).FullName, instance, priority, cacheType);

        /// <summary>
        /// Add a nameless instanced singleton
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="priority"></param>
        public static void AddSingleton(this ServiceCollection services, Type type, Object instance, Int32 priority = 10, Type cacheType = null)
            => services.AddSingleton(type.FullName, instance, priority, cacheType);

        #endregion

        #region Scoped Methods
        /// <summary>
        /// Create a new scoped service descriptor.
        /// </summary>
        /// <param name="name">The service name which will be used as the primary service key.</param>
        /// <param name="factory">The factory to be used when creating a new instance.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddScoped(this ServiceCollection services, String name, Type factory, Int32 priority = 10, Type cacheType = null)
            => services.AddService(name, factory, ServiceLifetime.Scoped, priority, cacheType);

        /// <summary>
        /// Create a new nameless scoped service descriptor.
        /// </summary>
        /// <param name="type">The service type which will be used as the primary service key.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddScoped(this ServiceCollection services, Type type, Type factory = null, Int32 priority = 10, Type cacheType = null)
            => services.AddScoped(type.FullName, factory ?? type, priority, cacheType);

        /// <summary>
        /// Create a new nameless scoped service descriptor.
        /// </summary>
        /// <typeparam name="T">The service type which will be used as the primary service key.</typeparam>
        /// <param name="name">(Optional) The name to register this service to. If none is defined, the generic type will be used instead.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddScoped<T>(this ServiceCollection services, String name = null, Type factory = null, Int32 priority = 10, Type cacheType = null)
        {
            if (name == null)
                services.AddScoped(typeof(T), factory, priority, cacheType);
            else
                services.AddScoped(name, factory ?? typeof(T), priority, cacheType);
        }
        #endregion

        #region Transient Methods
        /// <summary>
        /// Create a new transient service descriptor.
        /// </summary>
        /// <param name="name">The service name which will be used as the primary service key.</param>
        /// <param name="factory">The factory to be used when creating a new instance.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddTransient(this ServiceCollection services, String name, Type factory, Int32 priority = 10, Type cacheType = null)
            => services.AddService(name, factory, ServiceLifetime.Transient, priority, cacheType);

        /// <summary>
        /// Create a new nameless transient service descriptor.
        /// </summary>
        /// <param name="type">The service type which will be used as the primary service key.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddTransient(this ServiceCollection services, Type type, Type factory = null, Int32 priority = 10, Type cacheType = null)
            => services.AddTransient(type.FullName, factory ?? type, priority, cacheType);

        /// <summary>
        /// Create a new nameless transient service descriptor.
        /// </summary>
        /// <typeparam name="T">The service type which will be used as the primary service key.</typeparam>
        /// <param name="name">(Optional) The name to register this service to. If none is defined, the generic type will be used instead.</param>
        /// <param name="factory">(Optional) The factory to be used when creating a new instance. When null, the "type" value will be used instead.</param>
        /// <param name="priority">(Optional) The priority of this key definition. If a service with the same key is registered then that will be used instead.</param>
        /// <param name="cacheType">(Optional) The cache type of the singleton instance. When null, the factory type will be used instead.</param>
        public static void AddTransient<T>(this ServiceCollection services, String name = null, Type factory = null, Int32 priority = 10, Type cacheType = null)
        {
            if (name == null)
                services.AddTransient(typeof(T), factory, priority, cacheType);
            else
                services.AddTransient(name, factory ?? typeof(T), priority, cacheType);
        }
        #endregion
    }
}
