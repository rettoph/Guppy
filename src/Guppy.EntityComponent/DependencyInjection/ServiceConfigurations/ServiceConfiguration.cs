using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public abstract class ServiceConfiguration
    {
        #region Public Fields
        /// <summary>
        /// The xxHash of the <see cref="ServiceConfiguration.Name"/>
        /// </summary>
        public readonly UInt32 Id;

        /// <summary>
        /// The primary lookup key for the current service.
        /// </summary>
        public readonly String Name;

        /// <summary>
        /// The bound <see cref="TypeFactory"/>.
        /// </summary>
        public readonly TypeFactory TypeFactory;

        /// <summary>
        /// The service lifetime.
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// An array of strings with which this service will be cached once activated.
        /// All queries matching any of these values will return the defined
        /// configuration.
        /// </summary>
        public readonly String[] CacheNames;

        /// <summary>
        /// An array of actions to preform when building a new instace
        /// </summary>
        public readonly CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] Setups;
        #endregion

        #region Constructors
        internal ServiceConfiguration(
            String name,
            TypeFactory typeFactory,
            ServiceLifetime lifetime,
            String[] cacheNames,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups)
        {
            this.Id = name.xxHash();
            this.Name = name;
            this.TypeFactory = typeFactory;
            this.Lifetime = lifetime;
            this.CacheNames = cacheNames;
            this.Setups = setups;
        }
        #endregion

        #region Helper Methods
        public abstract ServiceConfigurationManager BuildServiceCofigurationManager(ServiceProvider provider);

        public virtual Object GetInstance(ServiceProvider provider)
        {
            return provider.GetServiceConfigurationManager(this).GetInstance();
        }

        public virtual Object GetInstance(ServiceProvider provider, Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder)
        {
            return provider.GetServiceConfigurationManager(this).GetInstance(customSetup, customSetupOrder);
        }
        #endregion

    }
}
