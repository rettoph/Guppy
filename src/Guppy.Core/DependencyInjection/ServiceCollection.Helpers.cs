using Guppy.DependencyInjection.Descriptors;
using Guppy.DependencyInjection.Enums;
using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class ServiceCollection
    {
        #region RegisterTypeFactory Methods
        public void RegisterTypeFactory(
            Type type,
            Func<ServiceProvider, Type, Object> method,
            UInt16 maxPoolSize = 100,
            Int32 priority = 0)
                => this.Add(new TypeFactoryDescriptor(type, method, maxPoolSize, priority));
        public void RegisterTypeFactory<TType>(
            Func<ServiceProvider, Type, TType> method,
            UInt16 maxPoolSize = 100,
            Int32 priority = 0)
                => this.RegisterTypeFactory(typeof(TType), (p, t) => method(p, t), maxPoolSize, priority);
        public void RegisterTypeFactory(
            Type type,
            Func<ServiceProvider, Object> method,
            UInt16 maxPoolSize = 100,
            Int32 priority = 0)
                => this.Add(new TypeFactoryDescriptor(type, (p, t) => method(p), maxPoolSize, priority));
        public void RegisterTypeFactory<TType>(
            Func<ServiceProvider, TType> method,
            UInt16 maxPoolSize = 100,
            Int32 priority = 0)
                => this.RegisterTypeFactory(typeof(TType), p => method(p), maxPoolSize, priority);
        #endregion

        #region RegisterServiceAction Methods
        private static Action<Object, ServiceProvider, ServiceConfiguration> BuildServiceActionMethod<T>(
            Action<T, ServiceProvider, ServiceConfiguration> action)
                where T : class
                    => (o, p, c) => action?.Invoke(o as T, p, c);
        public void RegisterServiceAction(
            ServiceConfigurationKey key,
            ServiceActionType type,
            Action<Object, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                => this.Add(new ServiceAction(key, type, method, order));
        public void RegisterServiceAction<T>(
            ServiceConfigurationKey key,
            ServiceActionType type,
            Action<T, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
                where T : class
                    => this.Add(new ServiceAction(key, type, BuildServiceActionMethod(method), order));
        #endregion

        #region RegisterServiceConfiguration Methods
        public void RegisterServiceConfiguration(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory = default,
            Type baseLookupType = default,
            int priority = 0)
                => this.Add(new ServiceConfigurationDescriptor(key, lifetime, typeFactory, baseLookupType, priority));
        #endregion

        #region RegisterComponent Methods
        public void RegisterComponent(
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServiceConfigurationKey)
                => this.Add(new ComponentConfigurationDescriptor(componentServiceConfigurationKey, entityServiceConfigurationKey));
        #endregion

        #region RegisterComponentFilter Methods
        public void RegisterComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Func<ServiceConfiguration, ServiceConfiguration, Boolean> validator = default,
            Int32 order = 0)
                => this.Add(new ComponentFilter(componentServiceConfigurationKey, method, validator, order));
        #endregion
    }
}
