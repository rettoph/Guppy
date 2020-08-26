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

        #region Game Methods
        public static void AddGame(this ServiceCollection services, Type game, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Game>(game);

            services.AddFactory(game, factory);
            services.AddSingleton(type: game, priority: priority);
        }
        public static void AddGame<TGame>(this ServiceCollection services, Func<ServiceProvider, TGame> factory, Int32 priority = 0)
            where TGame : Game
                => services.AddGame(typeof(TGame), p => factory(p), priority);
        #endregion

        #region Scene Methods
        public static void AddScene(this ServiceCollection services, Type scene, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Scene>(scene);

            services.AddFactory(scene, factory);
            services.AddScoped(
                type: scene, 
                priority: priority, 
                cacheType: typeof(Scene));
        }
        public static void AddScene<TScene>(this ServiceCollection services, Func<ServiceProvider, TScene> factory, Int32 priority = 0)
            where TScene : Scene
                => services.AddScene(typeof(TScene), p => factory(p), priority);
        #endregion

        #region Driver Methods
        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driver"></param>
        /// <param name="factory"></param>
        public static void AddDriver(this ServiceCollection services, Type driver, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddFactory(driver, factory, priority);
            services.AddTransient(driver);
        }

        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <param name="services"></param>
        /// <param name="factory"></param>
        public static void AddDriver<TDriver>(this ServiceCollection services, Func<ServiceProvider, TDriver> factory, Int32 priority = 0)
            where TDriver : Driver
                => services.AddDriver(typeof(TDriver), p => factory(p), priority);

        /// <summary>
        /// Bind a Driver type to a recieved Driven type. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver(this ServiceCollection services, Type driven, Type driver)
            => services.BindDriver(driven, driver, String.Empty);

        /// <summary>
        /// Bind a Driver type to a recieved Driven type. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver<TDriven, TDriver>(this ServiceCollection services)
            where TDriven : Driven
            where TDriver : Driver
                => services.BindDriver(typeof(TDriven), typeof(TDriver));

        /// <summary>
        /// Bind a Driver type to a recieved Driven type configuration. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created with the 
        /// recieved configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver(this ServiceCollection services, Type driven, Type driver, String configuration)
        {
            ExceptionHelper.ValidateAssignableFrom<Driven>(driven);
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddConfiguration(driven, configuration, (i, p, f) =>
            {
                ((Driven)i).AddDriver(driver);
            }, 5);
        }

        /// <summary>
        /// Bind a Driver type to a recieved Driven type configuration. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created with the 
        /// recieved configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver<TDriven, TDriver>(this ServiceCollection services, String configuration)
            where TDriven : Driven
            where TDriver : Driver
                => services.BindDriver(typeof(TDriven), typeof(TDriver));

        public static void AddAndBindDriver<TDriven, TDriver>(this ServiceCollection services, Func<ServiceProvider, TDriver> factory, Int32 priority = 0)
            where TDriver : Driver
            where TDriven : Driven
        {
            services.AddDriver<TDriver>(factory, priority);
            services.BindDriver<TDriven, TDriver>();
        }
        #endregion
    }
}
