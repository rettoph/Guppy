using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.Dtos
{
    public struct ServiceConfigurationDto
    {
        #region Public Fields
        /// <summary>
        /// The primary lookup key for the
        /// current service configuration.
        /// </summary>
        public readonly ServiceConfigurationKey Key;

        /// <summary>
        /// The default lifetime for the described service.
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// The <see cref="ITypeFactory.Type"/> to be used when building a 
        /// new instance of this service.
        /// </summary>
        public readonly Type TypeFactory;

        /// <summary>
        /// When a non <see cref="ServiceLifetime.Transient"/> service gets created
        /// the value will be linked within the defined cache keys so that any of
        /// the values will return the currently defined service.
        /// </summary>
        public readonly ServiceConfigurationKey[] CacheKeys;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// All services will be sorted by priority when
        /// their the service provider is created.
        /// </summary>
        public readonly Int32 Priority;
        #endregion

        #region Constructors
        public ServiceConfigurationDto(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory,
            IEnumerable<ServiceConfigurationKey> cacheKeys,
            Int32 priority = 0)
        {
            this.Key = key;
            this.Lifetime = lifetime;
            this.TypeFactory = typeFactory ?? key.Type;
            this.CacheKeys = cacheKeys.Concat(this.Key).Distinct().ToArray();
            this.Priority = priority;
        }

        public ServiceConfigurationDto(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory,
            ServiceConfigurationKey baseCacheKey,
            Int32 priority = 0) : this(key, lifetime, typeFactory, key.GetAncestors(baseCacheKey), priority)
        {
        }

        public ServiceConfigurationDto(
            ServiceConfigurationKey key,
            ServiceLifetime lifetime,
            Type typeFactory,
            Int32 priority = 0) : this(key, lifetime, typeFactory, key.Yield(), priority)
        {
        }
        #endregion

        #region Helper Methods
        internal IServiceConfiguration CreateServiceConfiguration(
            Dictionary<Type, ITypeFactory> factories,
            IEnumerable<SetupAction> actions)
        {
            switch (this.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    return new SingletonServiceConfiguration(this, factories, actions);
                case ServiceLifetime.Scoped:
                    return new ScopedServiceConfiguration(this, factories, actions);
                case ServiceLifetime.Transient:
                default:
                    return new TransientServiceConfiguration(this, factories, actions);
            }

        }
        #endregion
    }
}
