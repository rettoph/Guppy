using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        #region GetService Methods
        public static T GetService<T>(
            this ServiceProvider provider,
            ServiceConfigurationKey key,
            Action<T, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => provider.GetService(
                        key, 
                        (i, p, c) => setup?.Invoke(i as T, p, c), 
                        setupOrder, 
                        generics) as T;
        public static T GetService<T>(
            this ServiceProvider provider,
            Action<T, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
        {
            var instance = provider.GetService(
                ServiceConfigurationKey.From<T>(),
                (i, p, c) => setup?.Invoke(i as T, p, c),
                setupOrder,
                generics);

            return instance as T;
        }
        #endregion

        #region Service Methods
        public static void Service<T>(
            this ServiceProvider provider,
            out T instance,
            ServiceConfigurationKey key,
            Action<T, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => instance = provider.GetService<T>(
                        key,
                        setup,
                        setupOrder,
                        generics);
        public static void Service<T>(
            this ServiceProvider provider,
            out T instance,
            Action<T, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => instance = provider.GetService<T>(
                        setup,
                        setupOrder,
                        generics);
        #endregion
    }
}
