using Guppy.Attributes;
using Guppy.Configurations;
using Guppy.Interfaces;
using Guppy.Utilities.Loaders;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtenions
    {
        #region Layer Methods
        public static void AddLayer<TLayer>(this IServiceCollection services)
            where TLayer : Layer
        {
            services.AddPool<TLayer, UniquePool<TLayer>>();
        }
        #endregion

        #region Scene Methods
        public static void AddScene<TScene>(this IServiceCollection services)
            where TScene : Scene
        {
            services.AddPool<TScene, ScopedInitializablePool<TScene>>();
            services.AddScoped<TScene>(p => p.GetScene<TScene>());
        }
        #endregion

        #region Loader Methods
        public static void AddLoader<TLoader>(this IServiceCollection services)
            where TLoader : ILoader
        {

        }
        public static void AddLoader(this IServiceCollection services, Type loader)
        {
            if (!typeof(ILoader).IsAssignableFrom(loader))
                throw new Exception($"Unable to add loader! 'ILoader' is not assignable from '{loader.Name}'");

            services.AddSingleton(loader);
            services.AddSingleton(typeof(ILoader), p => p.GetRequiredService(loader));
        }
        #endregion

        #region Game Methods
        public static void AddGame<TGame>(this IServiceCollection services)
            where TGame : Game
        {
            services.AddPool<TGame, ScopedInitializablePool<TGame>>();
            services.AddScoped<TGame>(p => p.GetGame<TGame>());
        }
        #endregion

        #region Pool Methods
        private static HashSet<Type> RegisteredPools = new HashSet<Type>();

        /// <summary>
        /// Add a new pool if that specific type has yet
        /// to be added to the service. If it has, do nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPool"></typeparam>
        /// <param name="services"></param>
        public static void AddPool<T, TPool>(this IServiceCollection services, Func<IServiceProvider, TPool> factory = null)
            where TPool : Pool<T>
        {
            if (IServiceCollectionExtenions.RegisteredPools.Add(typeof(T)))
            {
                if (factory == null)
                    services.Add(new ServiceDescriptor(typeof(Pool<T>), typeof(TPool), ServiceLifetime.Scoped));
                else
                    services.Add(new ServiceDescriptor(typeof(Pool<T>), factory, ServiceLifetime.Scoped));

                return;
            }

            throw new Exception("Unable to pool service! Another pool for this type already exists.");
        }

        public static void AddTransientInitializable<T>(this IServiceCollection services, Boolean scoped = false)
            where T : class, IInitializable
        {
            if(scoped)
                services.AddPool<T, ScopedInitializablePool<T>>();
            else
                services.AddPool<T, InitializablePool<T>>();     

            services.AddTransient<T>(p => p.GetPooledService<T>());
        }
        #endregion

        #region Driver Methods
        public static void AddDriver<TDriven, TDriver>(this IServiceCollection services)
            where TDriven : IDriven
            where TDriver : IDriver
        {
            services.AddDriver(typeof(TDriven), typeof(TDriver));
        }

        public static void AddDriver(this IServiceCollection services, Type driven, Type driver)
        {
            if (!typeof(IDriven).IsAssignableFrom(driven))
                throw new Exception($"Unable to add driven! 'IDriven' is not assignable from '{driven.Name}'");
            if (!typeof(IDriver).IsAssignableFrom(driver))
                throw new Exception($"Unable to add driver! 'IDriver' is not assignable from '{driver.Name}'");

            services.AddSingleton(new DriverConfiguration()
            {
                Driven = driven,
                Driver = driver,
                Pool = new UniquePool<IDriver>(driver)
            });
        }
        #endregion
    }
}
