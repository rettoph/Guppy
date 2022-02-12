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
        /// The xxHash of the <see cref="ServiceConfiguration.Type"/>
        /// </summary>
        public readonly UInt32 Id;

        /// <summary>
        /// The primary lookup key for the current service.
        /// </summary>
        public readonly Type Type;

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
        public readonly Type[] Aliases;

        /// <summary>
        /// An array of actions to preform when building a new instace
        /// </summary>
        public readonly CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] Setups;
        #endregion

        #region Constructors
        internal ServiceConfiguration(
            Type type,
            TypeFactory typeFactory,
            ServiceLifetime lifetime,
            Type[] aliases,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups)
        {
            this.Id = type.AssemblyQualifiedName.xxHash();
            this.Type = type;
            this.TypeFactory = typeFactory;
            this.Lifetime = lifetime;
            this.Aliases = aliases;
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

        public virtual Object BuildInstance(ServiceProvider provider)
        {
            return provider.GetServiceConfigurationManager(this).BuildInstance();
        }

        public virtual Object BuildInstance(ServiceProvider provider, Action<Object, ServiceProvider, ServiceConfiguration> customSetup, Int32 customSetupOrder)
        {
            return provider.GetServiceConfigurationManager(this).BuildInstance(customSetup, customSetupOrder);
        }
        #endregion

    }
}
