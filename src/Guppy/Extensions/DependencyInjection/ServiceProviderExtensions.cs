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
        public static T BuildService<T>(this ServiceProvider provider, UInt32 configurationId, Action<Object, ServiceProvider, ServiceConfiguration> setup = null)
        {
            T instance = default(T);
            provider.GetServiceConfiguration(configurationId).Build(provider, setup, i => instance = (T)i);
            return instance;
        }

        /// <summary>
        /// Returns a new service registered under
        /// the configuration name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static T BuildService<T>(this ServiceProvider provider, String handle)
        {
            return provider.BuildService<T>(ServiceConfiguration.GetId(handle));
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

        #region GetService Methods
        public static ServiceTypeDescriptor GetServiceTypeDescriptor(this ServiceProvider provider, Type type)
            => provider.serviceTypeDescriptors[type];
        public static ServiceTypeDescriptor GetServiceTypeDescriptor<T>(this ServiceProvider provider)
            => provider.GetServiceTypeDescriptor(typeof(T));
        #endregion

        #region GetConfiguration Methods
        /// <summary>
        /// Returns the raw underlying factory from a given
        /// configuration id.
        /// </summary>
        /// <param name="configurationId"></param>
        /// <returns></returns>
        public static ServiceConfiguration GetServiceConfiguration(this ServiceProvider provider, UInt32 configurationId)
            => provider.serviceConfigurations[configurationId];
        public static ServiceConfiguration GetServiceConfiguration(this ServiceProvider provider, String handle)
            => provider.GetServiceConfiguration(ServiceConfiguration.GetId(handle));
        public static ServiceConfiguration GetServiceConfiguration(this ServiceProvider provider, Type serviceType)
            => provider.GetServiceConfiguration(serviceType.FullName);
        #endregion
    }
}
