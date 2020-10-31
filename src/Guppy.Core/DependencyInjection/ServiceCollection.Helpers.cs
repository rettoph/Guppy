using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Partial class to store any ServiceCollection helper
    /// methods. This replaces the Extension methods.
    /// </summary>
    public partial class ServiceCollection
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
        public void AddConfiguration(Type type, String name, Action<Object, ServiceProvider, ServiceContext> configuration, Int32 order = 0)
            => this.AddConfiguration(new ServiceConfigurationKey(type, name), configuration, order);

        /// <summary>
        /// Add a brand new configuration method utilizing 
        /// the generic type and name for the inheritance key.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="order"></param>
        public void AddConfiguration<T>(String name, Action<T, ServiceProvider, ServiceContext> configuration, Int32 order = 0)
            where T : class
                => this.AddConfiguration(new ServiceConfigurationKey(typeof(T), name), (i, p, s) => configuration.Invoke(i as T, p, s), order);

        /// <summary>
        /// Add a brand new configuration method utilizing 
        /// the generic type and name for the inheritance key.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="configuration"></param>
        /// <param name="order"></param>
        public void AddConfiguration<T>(Action<T, ServiceProvider, ServiceContext> configuration, Int32 order = 0)
            where T : class
                => this.AddConfiguration(new ServiceConfigurationKey(typeof(T), String.Empty), (i, p, s) => configuration.Invoke(i as T, p, s), order);
        #endregion

        #region AddBuilder Methods
        /// <summary>
        /// Add a new Builder to the collection.
        /// Builders get excecuted when a factory
        /// of a specific type created an instance.
        /// 
        /// This will not get called when pulling
        /// a pooled instance.
        /// </summary>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="builder"></param>
        /// <param name="order"></param>
        public void AddBuilder<TFactory>(Action<TFactory, ServiceProvider> builder, Int32 order = 0)
            where TFactory : class
                => this.AddBuilder(typeof(TFactory), (i, p) => builder.Invoke(i as TFactory, p), order);
        #endregion

        #region AddContext Methods
        /// <summary>
        /// Create a new ServiceContext with a unique name.
        /// </summary>
        /// <typeparam name="TFactory">The factory type to utilize for this context.</typeparam>
        /// <param name="name">The primary lookup name for this service.</param>
        /// <param name="lifetime">The service lifetime If null the factory lifetime will be used instead.</param>
        /// <param name="priority">Only the highest priority level for each lookup name will be saved post initialization.</param>
        /// <param name="serviceType">(Optional) When the lifetime is singleton or scoped this allows for a custom lookup type. If none is defined the factory type will be used instead.</param>
        /// <param name="autoBuild">(Optional) When true, scoped and singleton instances will automatically be created.</param>
        public void AddContext<TFactory>(String name, ServiceLifetime? lifetime = null, Int32 priority = 10, Type serviceType = null, Boolean autoBuild = false)
            => this.AddContext(name, typeof(TFactory), lifetime, priority, serviceType, autoBuild);

        /// <summary>
        /// Create a new ServiceContext with a unique name.
        /// </summary>
        /// <typeparam name="TFactory">The factory type to utilize for this context.</typeparam>
        /// <typeparam name="TFactory">When the lifetime is singleton or scoped this allows for a custom lookup type.</typeparam>
        /// <param name="name">The primary lookup name for this service.</param>
        /// <param name="lifetime">The service lifetime If null the factory lifetime will be used instead.</param>
        /// <param name="priority">Only the highest priority level for each lookup name will be saved post initialization.</param>
        /// <param name="autoBuild">(Optional) When true, scoped and singleton instances will automatically be created.</param>
        public void AddContext<TFactory, TService>(String name, ServiceLifetime? lifetime = null, Int32 priority = 10, Boolean autoBuild = false)
            => this.AddContext(name, typeof(TFactory), lifetime, priority, typeof(TService), autoBuild);
        #endregion
    }
}
