using Guppy.DependencyInjection.Descriptors;
using Guppy.DependencyInjection.Enums;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ServiceConfiguration : ServiceConfigurationDescriptor
    {
        #region Private Fields
        private Object _instance;
        private Func<ServiceProvider, Action<Object, ServiceProvider, ServiceConfiguration>, Int32, Type[], Object> _getInstance;
        #endregion

        #region Public Properties
        /// <summary>
        /// The factory to be used when creating a new instance of
        /// this service.
        /// </summary>
        public new readonly TypeFactory TypeFactory;

        /// <summary>
        /// A table of actions to preform on services within various
        /// positions of their lifecycle.
        /// </summary>
        public readonly Dictionary<ServiceActionType, ServiceAction[]> Actions;
        #endregion

        #region Constructor
        internal ServiceConfiguration(
            ServiceConfigurationDescriptor descriptor,
            TypeFactory typeFactory,
            IEnumerable<ServiceAction> actions
        ) : base(descriptor.Key, descriptor.Lifetime, descriptor.TypeFactory, descriptor.BaseLookupType, descriptor.Priority)
        {
            this.TypeFactory = typeFactory;
            this.Actions = this.Actions = DictionaryHelper.BuildEnumDictionary<ServiceActionType, ServiceAction[]>(
                with: actions.GroupBy(a => a.Type).ToDictionary(
                    keySelector: g => g.Key,
                    elementSelector: g => g.ToArray()),
                fallback: new ServiceAction[0]);

            switch (this.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    _getInstance = this.GetSingleton;
                    break;
                case ServiceLifetime.Scoped:
                    _getInstance = this.GetScoped;
                    break;
                case ServiceLifetime.Transient:
                default:
                    _getInstance = this.GetTransient;
                    break;
            }

        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Get or create a new instance of the described service
        /// based on the internal ServiceLifetime.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        public Object GetInstance(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                => _getInstance(provider, setup, setupOrder, generics);

        /// <summary>
        /// Get or create a new singleton instance of the current described service
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        private Object GetSingleton(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
        {
            if (provider.TryGetSingletonInstance(this.Key, out _instance))
                return _instance;

            var instance = this.BuildInstance(provider, generics, provider.CacheSingletonInstance);
            this.Actions[ServiceActionType.Setup].Apply(instance, provider, this, setup, setupOrder);
            return instance;
        }

        /// <summary>
        /// Get or create a scoped instance of the current described service
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        private Object GetScoped(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
        {
            if (provider.TryGetScopedInstance(this.Key, out _instance))
                return _instance;

            var instance = this.BuildInstance(provider, generics, provider.CacheScopedInstance);
            this.Actions[ServiceActionType.Setup].Apply(instance, provider, this, setup, setupOrder);
            return instance;
        }

        /// <summary>
        /// Create a new transient instance of the current described service
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="setup">A custom setup action to preform.</param>
        /// <param name="setupOrder">The order in which to preform the custom setup method.</param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        private Object GetTransient(
            ServiceProvider provider,
            Action<Object, ServiceProvider, ServiceConfiguration> setup = default,
            Int32 setupOrder = 0,
            Type[] generics = default)
                => this.Actions[ServiceActionType.Setup].Apply(
                    instance: this.BuildInstance(provider, generics), 
                    provider: provider, 
                    configuration: this,
                    setup: setup,
                    setupOrder: setupOrder);

        /// <summary>
        /// Create & return a brand new instance of the descriped service
        /// regardless of the defined ServiceLifetime without running any setup
        /// methods.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="generics">Generic type arguments to be passed into the factory.</param>
        private Object BuildInstance(
            ServiceProvider provider,
            Type[] generics = default,
            Action<ServiceConfiguration, Object, IEnumerable<ServiceConfigurationKey>> cacher = default)
        {
            return this.TypeFactory.Build(provider, generics, i =>
            {
                cacher?.Invoke(this, i, this.Key.GetAncestors(this.BaseLookupType));
                this.Actions[ServiceActionType.Builder].Apply(i, provider, this);
            });
        }
        #endregion
    }
}
