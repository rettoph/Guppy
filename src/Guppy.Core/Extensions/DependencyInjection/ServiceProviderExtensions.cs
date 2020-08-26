using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using xxHashSharp;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        #region GetService Methods
        /// <summary>
        /// Return a new instance of a service 
        /// based on the recieved service id value
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static T GetService<T>(
            this ServiceProvider provider,
            UInt32 id,
            Action<T, ServiceProvider, ServiceDescriptor> setup = null
        )
            => (T)provider.GetService(id, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new instance of a service 
        /// based on the recieved service name value
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static T GetService<T>(
            this ServiceProvider provider,
            String name,
            Action<T, ServiceProvider, ServiceDescriptor> setup = null
        )
            => (T)provider.GetService(name, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved type
        /// & automatically cast to type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static T GetService<T>(
            this ServiceProvider provider,
            Type type,
            Action<T, ServiceProvider, ServiceDescriptor> setup = null
        )
            => (T)provider.GetService(type, (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Return a new nameless service instance
        /// based on the recieved generic type
        /// and cast into type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static T GetService<T>(
            this ServiceProvider provider,
            Action<T, ServiceProvider, ServiceDescriptor> setup = null
        )
            => (T)provider.GetService(typeof(T), (i, p, s) => setup?.Invoke((T)i, p, s));

        /// <summary>
        /// Automaitcally set the out value via the intenral GetService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public static void Service<T>(this ServiceProvider provider, out T service, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            => service = ServiceProviderExtensions.GetService<T>(provider, setup);

        /// <summary>
        /// Automaitcally set the out value via the intenral GetService method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public static void Service<T>(this ServiceProvider provider, out T service, String name, Action<T, ServiceProvider, ServiceDescriptor> setup = null)
            => service = ServiceProviderExtensions.GetService<T>(provider, name, setup);
        #endregion
    }
}
