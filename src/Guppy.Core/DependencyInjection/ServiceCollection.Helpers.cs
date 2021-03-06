﻿using Guppy.DependencyInjection.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.Actions;

namespace Guppy.DependencyInjection
{
    public partial class ServiceCollection
    {
        #region RegisterTypeFactory Methods
        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="typeImplementation">The type to be passed into the recieved <paramref name="method"/> when constructing a new instance. If neccessary, a constructed generic type should be built.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory(
            Type type,
            Type typeImplementation,
            Func<ServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(type, typeImplementation, method, maxPoolSize, priority));

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory(
            Type type,
            Func<ServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(type, method, maxPoolSize, priority));


        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <typeparam name="T">The "lookup" type for the described factory.</typeparam>
        /// <typeparam name="TImplementation">The type to be passed into the recieved <paramref name="method"/> when constructing a new instance. If neccessary, a constructed generic type should be built.</typeparam>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory<T, TImplementation>(
            Func<ServiceProvider, Type, T> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(typeof(T), typeof(TImplementation), method as Func<ServiceProvider, Type, Object>, maxPoolSize, priority));

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <typeparam name="T">The "lookup" type for the described factory.</typeparam>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory<T>(
            Func<ServiceProvider, Type, T> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(typeof(T), method as Func<ServiceProvider, Type, Object>, maxPoolSize, priority));

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="typeImplementation">The type to be passed into the recieved <paramref name="method"/> when constructing a new instance. If neccessary, a constructed generic type should be built.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory(
            Type type,
            Type typeImplementation,
            Func<ServiceProvider, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(type, typeImplementation, (p, t) => method(p), maxPoolSize, priority));

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <param name="type">The "lookup" type for the described factory.</param>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory(
            Type type,
            Func<ServiceProvider, Object> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(type, (p, t) => method(p), maxPoolSize, priority));


        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <typeparam name="T">The "lookup" type for the described factory.</typeparam>
        /// <typeparam name="TImplementation">The type to be passed into the recieved <paramref name="method"/> when constructing a new instance. If neccessary, a constructed generic type should be built.</typeparam>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory<T, TImplementation>(
            Func<ServiceProvider, T> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(typeof(T), typeof(TImplementation), (p, t) => method(p), maxPoolSize, priority));

        /// <summary>
        /// Registers a new TypeFactory.
        /// </summary>
        /// <typeparam name="T">The "lookup" type for the described factory.</typeparam>
        /// <param name="method">The factory method to create a brand new instance when requested.</param>
        /// <param name="maxPoolSize">The maximum size of the factory's internal pool.</param>
        /// <param name="priority">The priority value for this specific descriptor. All factories will be sorted by priority building the provider.</param>
        public void RegisterTypeFactory<T>(
            Func<ServiceProvider, T> method,
            UInt16 maxPoolSize = 500,
            Int32 priority = 0)
                => this.Add(new TypeFactoryContext(typeof(T), (p, t) => method(p), maxPoolSize, priority));
        #endregion

        #region RegisterServiceConfiguration Methods
        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(key, lifetime, typeFactory, cacheKeys, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(key, lifetime, typeFactory, baseCacheKey, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(key, lifetime, typeFactory, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKeyType>(
            String keyName,
            ServiceLifetime lifetime,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKeyType>(keyName), lifetime, typeFactory, cacheKeys, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKeyType>(
            String keyName,
            ServiceLifetime lifetime,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKeyType>(keyName), lifetime, typeFactory, baseCacheKey, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKeyType>(
            String keyName,
            ServiceLifetime lifetime,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKeyType>(keyName), lifetime, typeFactory, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKey>(
            ServiceLifetime lifetime,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKey>(), lifetime, typeFactory, cacheKeys, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKey>(
            ServiceLifetime lifetime,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKey>(), lifetime, typeFactory, baseCacheKey, priority));

        /// <summary>
        /// Registers a new ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="lifetime">The default lifetime for the described service.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterServiceConfiguration<TKey>(
            ServiceLifetime lifetime,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.Add(new ServiceConfigurationContext(ServiceConfigurationKey.From<TKey>(), lifetime, typeFactory, priority));

        #region RegisterSingleton Methods
        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton(
            ServiceConfigurationKey key,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Singleton, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton(
            ServiceConfigurationKey key,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Singleton, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton(
            ServiceConfigurationKey key,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Singleton, typeFactory, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKeyType>(
            String keyName,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Singleton, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKeyType>(
            String keyName,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Singleton, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKeyType>(
            String keyName,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Singleton, typeFactory, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKey>(
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Singleton, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKey>(
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Singleton, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new singleton ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterSingleton<TKey>(
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Singleton, typeFactory, priority);
        #endregion

        #region RegisterScoped Methods
        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped(
            ServiceConfigurationKey key,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Scoped, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped(
            ServiceConfigurationKey key,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Scoped, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped(
            ServiceConfigurationKey key,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Scoped, typeFactory, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKeyType>(
            String keyName,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Scoped, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKeyType>(
            String keyName,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Scoped, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKeyType>(
            String keyName,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Scoped, typeFactory, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKey>(
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Scoped, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKey>(
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Scoped, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new scoped ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterScoped<TKey>(
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Scoped, typeFactory, priority);
        #endregion

        #region RegisterTransient Methods
        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient(
            ServiceConfigurationKey key,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Transient, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient(
            ServiceConfigurationKey key,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Transient, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <param name="key">The primary lookup key for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient(
            ServiceConfigurationKey key,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration(key, ServiceLifetime.Transient, typeFactory, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKeyType>(
            String keyName,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Transient, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKeyType>(
            String keyName,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Transient, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKeyType">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="keyName">The primary lookup key name for the current service configuration.</param>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKeyType>(
            String keyName,
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKeyType>(keyName, ServiceLifetime.Transient, typeFactory, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created the value will be linked within the defined cache keys so that any of the values will return the currently defined service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKey>(
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Transient, typeFactory, cacheKeys, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="baseCacheKey">The minimum cache key. All ancestors between <paramref name="baseCacheKey"/> and <paramref name="key"/> will be used as <see cref="ServiceConfigurationContext.CacheKeys"/>.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKey>(
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Transient, typeFactory, baseCacheKey, priority);

        /// <summary>
        /// Registers a new transient ServiceConfiguration.
        /// </summary>
        /// <typeparam name="TKey">The primary lookup key type for the current service configuration.</typeparam>
        /// <param name="typeFactory">The <see cref="ITypeFactory.Type"/> to be used when building a new instance of this service.</param>
        /// <param name="priority">The priority value for this specific descriptor. All services will be sorted by priority when their the service provider is created.</param>
        public void RegisterTransient<TKey>(
            Type typeFactory = default,
            Int32 priority = 0)
                => this.RegisterServiceConfiguration<TKey>(ServiceLifetime.Transient, typeFactory, priority);
        #endregion

        #endregion

        #region RegisterComponent Methods
        /// <summary>
        /// Register a new component to an entity.
        /// </summary>
        /// <param name="componentServiceConfigurationKey">The component's key</param>
        /// <param name="entityServiceConfigurationKey">The entity's key</param>
        public void RegisterComponent(
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServiceConfigurationKey)
                => this.Add(new ComponentConfigurationContext(componentServiceConfigurationKey, entityServiceConfigurationKey));

        /// <summary>
        /// Register a new component to an entity.
        /// </summary>
        /// <typeparam name="TComponentKey">The component's key</typeparam>
        /// <typeparam name="TEntityKey">The entity's key</typeparam>
        public void RegisterComponent<TComponentKey, TEntityKey>()
            where TComponentKey : class, IComponent
            where TEntityKey : class, IEntity
                => this.RegisterComponent(ServiceConfigurationKey.From<TComponentKey>(), ServiceConfigurationKey.From<TEntityKey>());

        /// <summary>
        /// Register a new component to an entity.
        /// </summary>
        /// <typeparam name="TComponentKeyType">The component's key type</typeparam>
        /// <typeparam name="TEntityKeyType">The entity's key type</typeparam>
        /// <param name="componentKeyName">The component's key name</param>
        /// <param name="entityKeyName">The entity's key name</param>
        public void RegisterComponent<TComponentKeyType, TEntityKeyType>(
            String componentKeyName,
            String entityKeyName)
                where TComponentKeyType : class, IComponent
                where TEntityKeyType : class, IEntity
                    => this.RegisterComponent(ServiceConfigurationKey.From<TComponentKeyType>(componentKeyName), ServiceConfigurationKey.From<TEntityKeyType>(entityKeyName));
        #endregion

        #region RegisterComponentFilter Methods
        public void RegisterComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Func<IServiceConfiguration, IServiceConfiguration, Boolean> validator,
            Int32 order = 0)
                => this.Add(new ComponentFilter(componentServiceConfigurationKey, method, validator, order));

        public void RegisterComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Int32 order = 0)
                => this.Add(new ComponentFilter(componentServiceConfigurationKey, method, order));
        #endregion

        #region RegisterBuilder Methods
        public void RegisterBuilder(
            Type type,
            Action<Object, ServiceProvider, ITypeFactory> method,
            Int32 order = 0)
                => this.Add(new BuilderAction(type, method, order));
        public void RegisterBuilder<T>(
            Action<T, ServiceProvider, ITypeFactory> method,
            Int32 order = 0)
                where T : class
                    => this.Add(new BuilderAction(typeof(T), (i, p, f) => method(i as T, p, f), order));
        #endregion

        #region RegisterSetup Methods
        public void RegisterSetup(
            ServiceConfigurationKey key,
            Action<Object, ServiceProvider, IServiceConfiguration> method,
            Int32 order = 0)
                => this.Add(new SetupAction(key, method, order));
        public void RegisterSetup<TKeyType>(
            String keyName,
            Action<TKeyType, ServiceProvider, IServiceConfiguration> method,
            Int32 order = 0)
                where TKeyType : class
                    => this.Add(new SetupAction(ServiceConfigurationKey.From<TKeyType>(keyName), (i, p, c) => method(i as TKeyType, p, c), order));

        public void RegisterSetup<TKey>(
            Action<TKey, ServiceProvider, IServiceConfiguration> method,
            Int32 order = 0)
                where TKey : class
                    => this.Add(new SetupAction(ServiceConfigurationKey.From<TKey>(), (i, p, c) => method(i as TKey, p, c), order));
        #endregion
    }
}
