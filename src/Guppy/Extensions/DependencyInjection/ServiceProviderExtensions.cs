using Guppy.DependencyInjection;
using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        #region Build Service Methods
        /// <summary>
        /// Build a new instance of a service regardless of it's
        /// lifetime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public static T BuildService<T>(this ServiceProvider provider, UInt32 configurationId)
        {
            return (T)provider.GetFactory(configurationId).Build(provider);;
        }

        /// <summary>
        /// Returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static T BuildService<T>(this ServiceProvider provider, String configuration)
        {
            return provider.BuildService<T>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(configuration)));
        }

        /// <summary>
        /// Returns a new instance of the requested service type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Object BuildService(this ServiceProvider provider, Type type)
        {
            return provider.BuildService<Object>(type.FullName);
        }

        /// <summary>
        /// Returns a new instance of the requested service type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T BuildService<T>(this ServiceProvider provider)
        {
            return (T)provider.BuildService(typeof(T));
        }
        #endregion

        #region Content Methods
        /// <summary>
        /// Automatically return content firectly from the
        /// ServiceProvider's ContentLoader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static T GetContent<T>(this ServiceProvider provider, String handle)
            => provider.GetService<ContentLoader>().Get<T>(handle);
        /// <summary>
        /// Automatically load content from the internal ContentLoader
        /// and set the value to the recieved instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <param name="content"></param>
        public static void Content<T>(this ServiceProvider provider, String handle, out T content)
            => content = provider.GetContent<T>(handle);
        #endregion

        #region GetFactory Methods
        /// <summary>
        /// Returns the raw underlying factory from a given
        /// configuration id.
        /// </summary>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public static ServiceFactory GetFactory(this ServiceProvider provider, UInt32 configurationId)
        {
            return provider.factories[configurationId];
        }
        public static ServiceFactory GetFactory(this ServiceProvider provider, String configuration)
        {
            return provider.GetFactory(xxHash.CalculateHash(Encoding.UTF8.GetBytes(configuration)));
        }
        public static ServiceFactory GetFactory(this ServiceProvider provider, Type serviceType)
        {
            return provider.GetFactory(serviceType.FullName);
        }
        #endregion
    }
}
