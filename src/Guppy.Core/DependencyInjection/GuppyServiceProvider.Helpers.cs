using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.ServiceManagers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class GuppyServiceProvider
    {
        private IServiceManager _manager;
        private IServiceConfiguration _service;

        #region GetService Methods
        public Object GetService(
            ServiceConfigurationKey key, 
            Type[] generics = default)
        {
            // Implement some backwards compatibility with Microsoft's DI 
            // For more info visit: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L95

            // Check the recieved key directly...
            if (_activeServices.TryGetValue(key, out _manager))
                return _manager.GetInstance();
            else if (_registeredServices.TryGetValue(key, out _service))
                return _service.GetInstance(this, generics);

            // Attempt to get constructed generic type services...
            key = key.TryGetGenericKey(out generics);
            if (_activeServices.TryGetValue(key, out _manager))
                return _manager.GetInstance();
            else if (_registeredServices.TryGetValue(key, out _service))
                return _service.GetInstance(this, generics);


            // Attempt to create an enumerable instance based on the lookup key...
            return this.TryCreateEnumerable(key, generics);
        }

        public Object GetService(
            ServiceConfigurationKey key,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
        {
            // Implement some backwards compatibility with Microsoft's DI 
            // For more info visit: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L95

            // Check the recieved key directly...
            if (_activeServices.TryGetValue(key, out _manager))
                return _manager.GetInstance(setup, setupOrder);
            else if (_registeredServices.TryGetValue(key, out _service))
                return _service.GetInstance(this, generics, setup, setupOrder);

            // Attempt to get constructed generic type services...
            key = key.TryGetGenericKey(out generics);
            if (_activeServices.TryGetValue(key, out _manager))
                return _manager.GetInstance(setup, setupOrder);
            else if (_registeredServices.TryGetValue(key, out _service))
                return _service.GetInstance(this, generics, setup, setupOrder);


            // Attempt to create an enumerable instance based on the lookup key...
            return this.TryCreateEnumerable(key, setup, setupOrder, generics);
        }

        #region GetService Generic Helpers
        public T GetService<T>(
            ServiceConfigurationKey key,
            Type[] generics = default)
                where T : class
                    => this.GetService(key, generics) as T;


        public T GetService<T>(
            ServiceConfigurationKey key,
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => this.GetService(key, (i, p, c) => setup(i as T, p, c), setupOrder, generics) as T;

        public TKeyType GetService<TKeyType>(
            String keyName,
            Type[] generics = default)
                where TKeyType : class
                    => this.GetService(ServiceConfigurationKey.From<TKeyType>(keyName), generics) as TKeyType;


        public TKeyType GetService<TKeyType>(
            String keyName,
            Action<TKeyType, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where TKeyType : class
                    => this.GetService(ServiceConfigurationKey.From<TKeyType>(keyName), (i, p, c) => setup(i as TKeyType, p, c), setupOrder, generics) as TKeyType;

        public TKey GetService<TKey>(
            Type[] generics = default)
                where TKey : class
                    => this.GetService(ServiceConfigurationKey.From<TKey>(), generics) as TKey;


        public TKey GetService<TKey>(
            Action<TKey, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where TKey : class
                    => this.GetService(ServiceConfigurationKey.From<TKey>(), (i, p, c) => setup(i as TKey, p, c), setupOrder, generics) as TKey;
        #endregion
        #endregion

        #region GetServiceLazy Methods
        public Lazy<T> GetServiceLazy<T>(
            ServiceConfigurationKey key,
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default,
            Boolean isThreadSafe = true)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(key, setup, setupOrder, generics), isThreadSafe);
        public Lazy<T> GetServiceLazy<T>(
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default,
            Boolean isThreadSafe = true)
                where T : class
                    => new Lazy<T>(() => this.GetService<T>(setup, setupOrder, generics), isThreadSafe);
        #endregion

        #region Service Methods
        public void Service<T>(
            ServiceConfigurationKey key,
            out T instance,
            Type[] generics = default)
                where T : class
                    => instance = this.GetService(key, generics) as T;

        public void Service<T>(
            ServiceConfigurationKey key,
            out T instance,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => instance = this.GetService(key, setup, setupOrder, generics) as T;

        public void Service<TKeyType>(
            String keyName,
            out TKeyType instance,
            Type[] generics = default)
                where TKeyType : class
                    => instance = this.GetService<TKeyType>(keyName, generics);


        public void Service<TKeyType>(
            String keyName,
            out TKeyType instance,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where TKeyType : class
                    => instance = this.GetService<TKeyType>(keyName, setup, setupOrder, generics);

        public void Service<TKey>(
            out TKey instance,
            Type[] generics = default)
                where TKey : class
                    => instance = this.GetService<TKey>(generics);


        public void Service<TKey>(
            out TKey instance,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where TKey : class
                    => instance = this.GetService<TKey>(setup, setupOrder, generics);
        #endregion

        #region ServiceLazy Methods
        public void ServiceLazy<T>(
            out Lazy<T> instance,
            ServiceConfigurationKey key,
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => instance = this.GetServiceLazy<T>(
                        key,
                        setup,
                        setupOrder,
                        generics);
        public void ServiceLazy<T>(
            out Lazy<T> instance,
            Action<T, GuppyServiceProvider, IServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                where T : class
                    => instance = this.GetServiceLazy<T>(
                        setup,
                        setupOrder,
                        generics);
        #endregion

        #region TryCreateEnumerable Methods
        /// <summary>
        /// Loosly based on Microsoft's DI.
        /// See original here: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L130
        /// </summary>
        /// <param name="key"></param>
        /// <param name="setup"></param>
        /// <param name="setupOrder"></param>
        /// <returns></returns>
        private Object TryCreateEnumerable(
            ServiceConfigurationKey key,
            Type[] generics)
        {
            if (key.Type == typeof(IEnumerable<>) && generics.Length == 1)
            {
                var configurations = _registeredServices
                    .Where(kvp => generics[0].IsAssignableFrom(kvp.Key.Type));

                return typeof(Enumerable)
                    .GetMethod("Cast")
                    .MakeGenericMethod(generics[0])
                    .Invoke(null, new object[] {
                            configurations.Select(kvp => kvp.Value.GetInstance(this, default))
                    });
            }

            return default;
        }

        /// <summary>
        /// Loosly based on Microsoft's DI.
        /// See original here: https://github.com/aspnet/DependencyInjection/blob/d77b090567a1e6ad9a5bb5fd05f4bdcf281d4185/src/DI/ServiceLookup/CallSiteFactory.cs#L130
        /// </summary>
        /// <param name="key"></param>
        /// <param name="setup"></param>
        /// <param name="setupOrder"></param>
        /// <param name="generics"></param>
        /// <returns></returns>
        private Object TryCreateEnumerable(
            ServiceConfigurationKey key,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> setup,
            Int32 setupOrder,
            Type[] generics)
        {
            if (key.Type == typeof(IEnumerable<>) && generics.Length == 1)
            {
                return typeof(Enumerable)
                    .GetMethod("Cast")
                    .MakeGenericMethod(generics[0])
                    .Invoke(null, new object[] {
                        _registeredServices
                            .Where(kvp => generics[0].IsAssignableFrom(kvp.Key.Type))
                            .Select(kvp => kvp.Value.GetInstance(this, default, setup, setupOrder))
                    });
            }

            return default;
        }
        #endregion
    }
}
