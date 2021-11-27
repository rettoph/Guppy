using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.TypeFactories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public class ServiceConfigurationBuilder : IPrioritizable<ServiceConfigurationBuilder>
    {
        #region Private Fields
        private ServiceLifetime _lifetime;
        private Type _typeFactory;
        private ServiceConfigurationKey[] _cacheKeys;
        #endregion

        #region Public Fields
        /// <summary>
        /// The primary lookup key for the
        /// current service configuration.
        /// </summary>
        public readonly ServiceConfigurationKey Key;
        #endregion

        #region Public Properties
        /// <summary>
        /// The default lifetime for the described service.
        /// </summary>
        public ServiceLifetime Lifetime
        {
            get => _lifetime;
            set => this.SetLifetime(value);
        }


        /// <summary>
        /// The <see cref="ITypeFactory.Type"/> to be used when building a 
        /// new instance of this service.
        /// </summary>
        public Type TypeFactory
        {
            get => _typeFactory;
            set => this.SetTypeFactory(value);
        }

        /// <summary>
        /// When a non <see cref="ServiceLifetime.Transient"/> service gets created
        /// the value will be linked within the defined cache keys so that any of
        /// the values will return the currently defined service.
        /// </summary>
        public ServiceConfigurationKey[] CacheKeys
        {
            get => _cacheKeys;
            set => this.SetCacheKeys(value);
        }

        /// <summary>
        /// The priority value for this specific descriptor.
        /// All services will be sorted by priority when
        /// their the service provider is created.
        /// </summary>
        public Int32 Priority { get; set; }
        #endregion

        #region Constructors
        public ServiceConfigurationBuilder(
            ServiceConfigurationKey key)
        {
            _cacheKeys = new ServiceConfigurationKey[]
            {
                key
            };

            this.Key = key;

            this.SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(key.Type);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Set the default lifetime for the described service.
        /// </summary>
        public ServiceConfigurationBuilder SetLifetime(ServiceLifetime lifetime)
        {
            _lifetime = lifetime;

            return this;
        }

        /// <summary>
        /// Set the <see cref="ITypeFactory.Type"/> to be used when building a 
        /// new instance of this service.
        /// </summary>
        public ServiceConfigurationBuilder SetTypeFactory(Type typeFactory)
        {
            if(typeFactory.ValidateAssignableFrom(Key.Type))
            {
                _typeFactory = typeFactory ?? this.Key.Type;
            }
            

            return this;
        }

        /// <summary>
        /// Set the <see cref="ITypeFactory.Type"/> to be used when building a 
        /// new instance of this service.
        /// </summary>
        public ServiceConfigurationBuilder SetTypeFactory<TFactory>()
        {
            if (typeof(TFactory).ValidateAssignableFrom(Key.Type))
            {
                _typeFactory = typeof(TFactory);
            }

            return this;
        }

        /// <summary>
        /// Set the <see cref="CacheKeys"/>.
        /// </summary>
        /// <param name="cacheKeys">When a non <see cref="ServiceLifetime.Transient"/> service gets created
        /// the value will be linked within the defined cache keys so that any of
        /// the values will return the currently defined service.</param>
        /// <returns></returns>
        public ServiceConfigurationBuilder SetCacheKeys(IEnumerable<ServiceConfigurationKey> cacheKeys)
        {
            Debug.Assert(this.Lifetime != ServiceLifetime.Transient, $"{nameof(ServiceConfigurationBuilder)}::{nameof(SetCacheKeys)} - {nameof(CacheKeys)} are only used when {nameof(Lifetime)} is not {ServiceLifetime.Transient}.");

            _cacheKeys = cacheKeys.Concat(this.Key).Distinct().ToArray();

            return this;
        }

        /// <summary>
        /// Given a <paramref name="baseCacheKey"/>, invoke <see cref="SetCacheKeys(IEnumerable{ServiceConfigurationKey})"/>
        /// using all ancestors between <see cref="Key"/> and <paramref name="baseCacheKey"/>.
        /// </summary>
        /// <param name="baseCacheKey"></param>
        /// <returns></returns>
        public ServiceConfigurationBuilder SetBaseCacheKey(ServiceConfigurationKey baseCacheKey)
        {
            return this.SetCacheKeys(this.Key.GetAncestors(baseCacheKey));
        }

        internal IServiceConfiguration Build(
            Dictionary<Type, ITypeFactory> factories,
            IEnumerable<SetupAction> actions)
        {
            return this.Lifetime switch
            {
                ServiceLifetime.Singleton => new SingletonServiceConfiguration(this, factories, actions),
                ServiceLifetime.Scoped => new ScopedServiceConfiguration(this, factories, actions),
                _ => new TransientServiceConfiguration(this, factories, actions)
            };
        }
        #endregion
    }
}
