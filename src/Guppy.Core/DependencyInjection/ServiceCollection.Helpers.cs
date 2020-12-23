using Guppy.DependencyInjection.Descriptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Partial implementation of the ServiceCollection class
    /// that will add simple helper methods.
    /// </summary>
    public partial class ServiceCollection
    {
        #region AddFactory Methods
        private static Func<ServiceProvider, Type, Object> ServiceFactoryFrom<T>(Func<ServiceProvider, T> factory)
            => (p, t) => factory(p);

        private static Func<ServiceProvider, Type, Object> ServiceFactoryFrom<T>(Func<ServiceProvider, Type, T> factory)
            => (p, t) => factory(p, t);

        public void AddFactory(Type type, Func<ServiceProvider, Type, Object> factory, Type implementationType = null, Int32 priority = 0)
            => this.Add(new ServiceFactoryDescriptor(type, factory, implementationType, priority));

        public void AddFactory<T>(Func<ServiceProvider, Type, T> factory, Type implementationType = null, Int32 priority = 0)
            => this.AddFactory(typeof(T), ServiceFactoryFrom(factory), implementationType, priority);

        public void AddFactory<T, TImplementation>(Func<ServiceProvider, Type, TImplementation> factory, Int32 priority = 0)
            => this.AddFactory(typeof(T), ServiceFactoryFrom(factory), typeof(TImplementation), priority);

        public void AddFactory(Type type, Func<ServiceProvider, Object> factory, Type implementationType = null, Int32 priority = 0)
            => this.Add(new ServiceFactoryDescriptor(type, ServiceFactoryFrom(factory), implementationType, priority));

        public void AddFactory<T>(Func<ServiceProvider, T> factory, Type implementationType = null, Int32 priority = 0)
            => this.AddFactory(typeof(T), ServiceFactoryFrom(factory), implementationType, priority);

        public void AddFactory<T, TImplementation>(Func<ServiceProvider, TImplementation> factory, Int32 priority = 0)
            => this.AddFactory(typeof(T), ServiceFactoryFrom(factory), typeof(TImplementation), priority);
        #endregion

        #region AddConfiguration Methods
        public void AddConfiguration(String name, ServiceLifetime lifetime, Type factory, Int32 priority = 0, Type cacheType = null)
            => this.Add(new ServiceConfigurationDescriptor(name, lifetime, factory, priority, cacheType));
        public void AddConfiguration<TFactory>(String name, ServiceLifetime lifetime, Int32 priority = 0, Type cacheType = null)
            => this.Add(new ServiceConfigurationDescriptor(name, lifetime, typeof(TFactory), priority, cacheType));

        #region Singleton Methods
        public void AddSingleton(String name, Type factory, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Singleton(name, factory, priority, cacheType));

        public void AddSingleton(Type type, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Singleton(type.FullName, type, priority, cacheType));

        public void AddSingleton<TFactory>(String name, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Singleton<TFactory>(name, priority, cacheType));

        public void AddSingleton<TFactory>(Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Singleton<TFactory>(priority, cacheType));
        #endregion

        #region Scoped Methods
        public void AddScoped(String name, Type factory, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Scoped(name, factory, priority, cacheType));

        public void AddScoped(Type type, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Scoped(type.FullName, type, priority, cacheType));

        public void AddScoped<TFactory>(String name, Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Scoped<TFactory>(name, priority, cacheType));

        public void AddScoped<TFactory>(Int32 priority = 0, Type cacheType = null)
            => this.Add(ServiceConfigurationDescriptor.Scoped<TFactory>(priority, cacheType));
        #endregion

        #region Transient Methods
        public void AddTransient(String name, Type factory, Int32 priority = 0)
            => this.Add(ServiceConfigurationDescriptor.Transient(name, factory, priority));

        public void AddTransient(Type type, Int32 priority = 0)
            => this.Add(ServiceConfigurationDescriptor.Transient(type.FullName, type, priority));

        public void AddTransient<TFactory>(String name, Int32 priority = 0)
            => this.Add(ServiceConfigurationDescriptor.Transient<TFactory>(name, priority));

        public void AddTransient<TFactory>(Int32 priority = 0)
            => this.Add(ServiceConfigurationDescriptor.Transient<TFactory>(priority));
        #endregion
        #endregion

        #region Action Methods
        private static Action<Object, ServiceProvider, ServiceConfiguration> ServiceActionFrom<T>(Action<T, ServiceProvider, ServiceConfiguration> action)
            where T : class
            => (i, p, c) => action(i as T, p, c);

        public void AddAction(ServiceActionType type, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(new ServiceAction(ServiceActionKey.From(), type, action, order));
        public void AddAction(ServiceActionKey key, ServiceActionType type, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(new ServiceAction(key, type, action, order));
        public void AddAction<T>(ServiceActionType type, Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(new ServiceAction(ServiceActionKey.From<T>(), type, ServiceActionFrom(action), order));
        public void AddAction<T>(String name, ServiceActionType type, Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(new ServiceAction(ServiceActionKey.From<T>(name), type, ServiceActionFrom(action), order));

        #region Builder Methods
        public void AddBuilder(Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(ServiceAction.Builder(ServiceActionKey.From(), action, order));
        public void AddBuilder(ServiceActionKey key, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(ServiceAction.Builder(key, action, order));
        public void AddBuilder<T>(Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(ServiceAction.Builder(ServiceActionKey.From<T>(), ServiceActionFrom(action), order));
        public void AddBuilder<T>(String name, Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(ServiceAction.Builder(ServiceActionKey.From<T>(name), ServiceActionFrom(action), order));
        #endregion

        #region Setup Methods
        public void AddSetup(Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(ServiceAction.Setup(ServiceActionKey.From(), action, order));
        public void AddSetup(ServiceActionKey key, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            => this.Add(ServiceAction.Setup(key, action, order));
        public void AddSetup<T>(Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(ServiceAction.Setup(ServiceActionKey.From<T>(), ServiceActionFrom(action), order));
        public void AddSetup<T>(String name, Action<T, ServiceProvider, ServiceConfiguration> action, Int32 order = 0)
            where T : class
                => this.Add(ServiceAction.Setup(ServiceActionKey.From<T>(name), ServiceActionFrom(action), order));
        #endregion
        #endregion
    }
}
