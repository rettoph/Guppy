using Guppy.DependencyInjection.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Interfaces;

namespace Guppy.DependencyInjection
{
    public partial class GuppyServiceCollection
    {
        #region RegisterTypeFactory Methods
        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        public TypeFactoryBuilder RegisterTypeFactory(
            Type type,
            Func<GuppyServiceProvider, Type, Object> method)
        {
            var builder = new TypeFactoryBuilder(type, method);
            this.Add(builder);
            return builder;
        }

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        public TypeFactoryBuilder RegisterTypeFactory(
            Type type,
            Func<GuppyServiceProvider, Object> method)
        {
            var builder = new TypeFactoryBuilder(type, (p, t) => method(p));
            this.Add(builder);
            return builder;
        }

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        public TypeFactoryBuilder RegisterTypeFactory<T>(
            Func<GuppyServiceProvider, Type, T> method)
        {
            return this.RegisterTypeFactory(typeof(T), (p, t) => method(p, t));
        }

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        public TypeFactoryBuilder RegisterTypeFactory<T>(
            Func<GuppyServiceProvider, T> method)
        {
            return this.RegisterTypeFactory(typeof(T), (p, t) => method(p));
        }
        #endregion

        #region RegisterServiceConfiguration Methods
        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>ok
        public ServiceConfigurationBuilder RegisterService(
            ServiceConfigurationKey key)
        {
            var builder = new ServiceConfigurationBuilder(key);
            this.Add(builder);
            return builder;
        }

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>ok
        public ServiceConfigurationBuilder RegisterService<TServiceConfigurationKeyType>(
            String serviceConfigurationKeyName = default)
        {
            return this.RegisterService(ServiceConfigurationKey.From<TServiceConfigurationKeyType>(serviceConfigurationKeyName));
        }
        #endregion

        #region RegisterComponent Methods
        /// <summary>
        /// Register a new component to an entity.
        /// </summary>
        /// <param name="componentServiceConfigurationKey">The component's key</param>
        public ComponentConfigurationBuilder RegisterComponent(
            ServiceConfigurationKey componentServiceConfigurationKey)
        {
            var builder = new ComponentConfigurationBuilder(componentServiceConfigurationKey);
            this.Add(builder);
            return builder;
        }

        /// <summary>
        /// Register a new component to an entity.
        /// </summary>
        public ComponentConfigurationBuilder RegisterComponent<TComponentServiceConfigurationKeyType>(
            String componentServiceConfigurationKeyName = null)
        {
            var builder = new ComponentConfigurationBuilder(ServiceConfigurationKey.From<TComponentServiceConfigurationKeyType>(componentServiceConfigurationKeyName));
            this.Add(builder);
            return builder;
        }
        #endregion

        #region RegisterComponentFilter Methods
        public ComponentFilterBuilder RegisterComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, GuppyServiceProvider, Type, Boolean> method)
        {
            var builder = new ComponentFilterBuilder(componentServiceConfigurationKey, method);
            this.Add(builder);
            return builder;
        }
        #endregion

        #region RegisterBuilder Methods
        public void RegisterBuilder(
            Type type,
            Action<Object, GuppyServiceProvider, ITypeFactory> method,
            Int32 order = 0,
            Func<IAction<Type, ITypeFactory>, Type, Boolean> filter = default)
                => this.Add(new BuilderAction(type, method, order, filter));
        public void RegisterBuilder<T>(
            Action<T, GuppyServiceProvider, ITypeFactory> method,
            Int32 order = 0,
            Func<IAction<Type, ITypeFactory>, Type, Boolean> filter = default)
                where T : class
                    => this.Add(new BuilderAction(typeof(T), (i, p, f) => method(i as T, p, f), order, filter));
        #endregion

        #region RegisterSetup Methods
        public void RegisterSetup(
            ServiceConfigurationKey key,
            Action<Object, GuppyServiceProvider, IServiceConfiguration> method,
            Int32 order = 0,
            Func<IAction<ServiceConfigurationKey, IServiceConfiguration>, ServiceConfigurationKey, Boolean> filter = default)
                => this.Add(new SetupAction(key, method, order, filter));

        public void RegisterSetup<T>(
            ServiceConfigurationKey key,
            Action<T, GuppyServiceProvider, IServiceConfiguration> method,
            Int32 order = 0,
            Func<IAction<ServiceConfigurationKey, IServiceConfiguration>, ServiceConfigurationKey, Boolean> filter = default)
                where T : class
                    => this.Add(new SetupAction(key, (i, p, c) => method(i as T, p, c), order, filter));

        public void RegisterSetup<TKeyType>(
            String keyName,
            Action<TKeyType, GuppyServiceProvider, IServiceConfiguration> method,
            Int32 order = 0,
            Func<IAction<ServiceConfigurationKey, IServiceConfiguration>, ServiceConfigurationKey, Boolean> filter = default)
                where TKeyType : class
                    => this.Add(new SetupAction(ServiceConfigurationKey.From<TKeyType>(keyName), (i, p, c) => method(i as TKeyType, p, c), order, filter));

        public void RegisterSetup<TKey>(
            Action<TKey, GuppyServiceProvider, IServiceConfiguration> method,
            Int32 order = 0,
            Func<IAction<ServiceConfigurationKey, IServiceConfiguration>, ServiceConfigurationKey, Boolean> filter = default)
                where TKey : class
                    => this.Add(new SetupAction(ServiceConfigurationKey.From<TKey>(), (i, p, c) => method(i as TKey, p, c), order, filter));
        #endregion
    }
}
