using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Descriptors;
using Guppy.DependencyInjection.Enums;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceCollection = Guppy.DependencyInjection.ServiceCollection;
using ServiceProvider = Guppy.DependencyInjection.ServiceProvider;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region RegisterSingleton Methods
        public static void RegisterSingleton(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Type typeFactory = null,
            Type baseLookupType = null,
            Int32 priority = 0)
                => collection.RegisterServiceConfiguration(
                    key, 
                    ServiceLifetime.Singleton, 
                    typeFactory,
                    baseLookupType, 
                    priority);

        public static void RegisterSingleton<TKeyType>(
            this ServiceCollection collection,
            String keyName = "",
            Type typeFactory = null,
            Type baseLookupType = null,
            Int32 priority = 0)
                => collection.RegisterServiceConfiguration(
                    ServiceConfigurationKey.From<TKeyType>(keyName),
                    ServiceLifetime.Singleton,
                    typeFactory,
                    baseLookupType,
                    priority);

        public static void RegisterSingleton(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Object instance,
            Type baseLookupType = null,
            Int32 priority = 0)
        {
            collection.RegisterTypeFactory(key.Type, p => instance);

            collection.RegisterServiceConfiguration(
                    key,
                    ServiceLifetime.Singleton,
                    key.Type,
                    baseLookupType,
                    priority);
        }

        public static void RegisterSingleton<TKeyType>(
            this ServiceCollection collection,
            String keyName,
            Object instance,
            Type baseLookupType = null,
            Int32 priority = 0)
                => collection.RegisterSingleton(
                    ServiceConfigurationKey.From<TKeyType>(keyName),
                    instance,
                    baseLookupType,
                    priority);

        public static void RegisterSingleton<TKeyType>(
            this ServiceCollection collection,
            Object instance,
            Type baseLookupType = null,
            Int32 priority = 0)
                => collection.RegisterSingleton(
                    ServiceConfigurationKey.From<TKeyType>(),
                    instance,
                    baseLookupType,
                    priority);
        #endregion

        #region RegisterScoped Methods
        public static void RegisterScoped(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Type typeFactory = null,
            Type baseLookupType = null,
            int priority = 0)
                => collection.RegisterServiceConfiguration(
                    key,
                    ServiceLifetime.Scoped,
                    typeFactory,
                    baseLookupType,
                    priority);

        public static void RegisterScoped<TKeyType>(
            this ServiceCollection collection,
            String keyName = "",
            Type typeFactory = null,
            Type baseLookupType = null,
            int priority = 0)
                => collection.RegisterServiceConfiguration(
                    ServiceConfigurationKey.From<TKeyType>(keyName),
                    ServiceLifetime.Scoped,
                    typeFactory,
                    baseLookupType,
                    priority);
        #endregion

        #region RegisterTransient Methods
        public static void RegisterTransient(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Type typeFactory = null,
            int priority = 0)
                => collection.RegisterServiceConfiguration(
                    key,
                    ServiceLifetime.Transient,
                    typeFactory,
                    default,
                    priority);

        public static void RegisterTransient<TKeyType>(
            this ServiceCollection collection,
            String keyName = "",
            Type typeFactory = null,
            int priority = 0)
                => collection.RegisterServiceConfiguration(
                    ServiceConfigurationKey.From<TKeyType>(keyName),
                    ServiceLifetime.Transient,
                    typeFactory,
                    default,
                    priority);
        #endregion

        #region RegisterBuilder Methods
        public static void RegisterBuilder<T>(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Action<T, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                where T : class
                    => collection.RegisterServiceAction(
                        key,
                        ServiceActionType.Builder,
                        (i, p, c) => method(i as T, p, c),
                        order);

        public static void RegisterBuilder<T>(
            this ServiceCollection collection,
            Action<T, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                where T : class
                    => collection.RegisterServiceAction(
                        ServiceConfigurationKey.From<T>(),
                        ServiceActionType.Builder,
                        (i, p, c) => method(i as T, p, c),
                        order);
        #endregion

        #region RegisterSetup Methods
        public static void RegisterSetup<T>(
            this ServiceCollection collection,
            ServiceConfigurationKey key,
            Action<T, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                where T : class
                    => collection.RegisterServiceAction(
                        key,
                        ServiceActionType.Setup,
                        (i, p, c) => method(i as T, p, c),
                        order);

        public static void RegisterSetup<T>(
            this ServiceCollection collection,
            Action<T, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                where T : class
                    => collection.RegisterServiceAction(
                        ServiceConfigurationKey.From<T>(),
                        ServiceActionType.Setup,
                        (i, p, c) => method(i as T, p, c),
                        order);
        #endregion

        #region AddComponent Methods
        public static void RegisterComponent<TEntity>(
            this ServiceCollection collection,
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServiceConfigurationKey)
                where TEntity : class, IEntity
        {
            collection.RegisterComponent(
                componentServiceConfigurationKey,
                entityServiceConfigurationKey);
        }

        public static void RegisterComponent<TEntity>(
            this ServiceCollection collection,
            ServiceConfigurationKey componentServiceConfigurationKey)
                where TEntity : class, IEntity
                    => collection.RegisterComponent<TEntity>(componentServiceConfigurationKey, ServiceConfigurationKey.From<TEntity>());

        public static void RegisterComponent<TComponent, TEntity>(
            this ServiceCollection collection)
                where TComponent : class, IComponent
                where TEntity : class, IEntity
                    => collection.RegisterComponent<TEntity>(ServiceConfigurationKey.From<TComponent>());
        #endregion
    }
}
