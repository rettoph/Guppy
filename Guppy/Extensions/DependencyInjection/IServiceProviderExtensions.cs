using Guppy.Configurations;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Utilities.Pools;
using Guppy.Collections;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceProviderExtensions
    {
        #region Scene Methods
        public static TScene GetScene<TScene>(this IServiceProvider provider, Action<TScene> setup = null)
            where TScene : Scene
        {
            return provider.GetConfigurationValueOrCreate<TScene>("scene", setup);
        }
        #endregion

        #region Loader Methods
        public static TLoader GetLoader<TLoader>(this IServiceProvider provider)
            where TLoader : class, ILoader
        {
            return provider.GetLoader(typeof(TLoader)) as TLoader;
        }

        public static ILoader GetLoader(this IServiceProvider provider, Type loader)
        {
            if (!typeof(ILoader).IsAssignableFrom(loader))
                throw new Exception($"Unable to add loader! 'ILoader' is not assignable from '{loader.Name}'");

            return provider.GetService(loader) as ILoader;
        }
        #endregion

        #region Game Methods
        /// <summary>
        /// Return the current scope's game instance or create a 
        /// brand new one. The inputed setup method is used in the event
        /// of a new game being generated from the game pool.
        /// </summary>
        /// <typeparam name="TGame"></typeparam>
        /// <param name="provider"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static TGame GetGame<TGame>(this IServiceProvider provider, Action<TGame> setup = null)
            where TGame : Game
        {
            return provider.GetConfigurationValueOrCreate<TGame>("game", setup);
        }
        #endregion

        #region Scope Methods 
        /// <summary>
        /// Create a new scope but cope the parent's
        /// ScopeConfiguration to thenew scope
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider CreateScopeWithConfiguration(this IServiceProvider provider)
        {
            var parentConfig = provider.GetService<ScopeConfiguration>();
            var childProvider = provider.CreateScope().ServiceProvider;
            var childConfig = childProvider.GetService<ScopeConfiguration>();

            // Copt the parent config to the child config...
            childConfig.Copy(parentConfig);

            return childProvider;
        }

        public static TBaseType GetConfigurationValue<TBaseType>(this IServiceProvider provider, String key)
        {
            var config = provider.GetRequiredService<ScopeConfiguration>();
            return config.Get<TBaseType>(key);
        }
        public static Object GetConfigurationValue(this IServiceProvider provider, String key)
        {
            var config = provider.GetRequiredService<ScopeConfiguration>();
            return config.Get(key);
        }

        public static void SetConfigurationValue(this IServiceProvider provider, String key, Object value)
        {
            var config = provider.GetRequiredService<ScopeConfiguration>();
            config.Set(key, value);
        }

        public static T GetConfigurationValueOrCreate<T>(this IServiceProvider provider, String value, Action<T> setup = null)
            where T : class
        {
            // Load the cached configuration value...
            var cached = provider.GetConfigurationValue(value);

            if(cached == default(T))
            { // If there is no data cached, create and save a new value...
                return provider.GetPooledService<T>(setup);
            }
            else if (cached.GetType().IsAssignableFrom(typeof(T)))
            { // There is data cached and it is a valid type; so return the it...
                return cached as T;
            }
            else
            {
                throw new Exception($"Unable to return value, invalid type. Cached value is an Object<{cached.GetType().Name}>, but Object<{typeof(T).Name}> is required.");
            }
        }
        #endregion

        #region Pool Methods
        public static T GetPooledService<T>(this IServiceProvider provider, Action<T> setup = null)
        {
            var pool = provider.GetService<Pool<T>>();

            if (pool == null)
                throw new Exception($"Unable to find service pool for '{typeof(T).Name}'. Please ensure a pool was registered.");

            return pool.Pull(provider, setup);
        }
        #endregion

        #region Driver Methods
        /// <summary>
        /// Return new or recycled instances
        /// of all driver types that are
        /// registered to the given driven instance
        /// type.
        /// </summary>
        /// <param name="driven"></param>
        /// <returns></returns>
        public static FrameableCollection<IDriver> GetDrivers(this IServiceProvider provider, IDriven driven)
        {
            var drivers = new FrameableCollection<IDriver>(provider);

            drivers.AddRange(provider.GetServices<DriverConfiguration>()
                .Where(c => c.Driven.IsAssignableFrom(driven.GetType()))
                .Select(c => ActivatorUtilities.CreateInstance(provider, c.Driver, driven) as IDriver)
                .OrderBy(d => d.UpdateOrder));

            return drivers;
        }
        #endregion
    }
}
