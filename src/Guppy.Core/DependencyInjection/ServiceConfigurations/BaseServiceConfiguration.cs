using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Dtos;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.ServiceConfigurations
{
    internal abstract class BaseServiceConfiguration : IServiceConfiguration
    {
        #region Protected Fields
        protected readonly ServiceConfigurationDto context;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public ServiceConfigurationKey Key => this.context.Key;

        /// <inheritdoc />
        public ServiceLifetime Lifetime => this.context.Lifetime;

        /// <inheritdoc />
        public ITypeFactory TypeFactory { get; private set; }

        /// <inheritdoc />
        public SetupAction[] SetupActions { get; private set; }

        /// <inheritdoc />
        public ServiceConfigurationKey[] DefaultCacheKeys => this.context.CacheKeys;
        #endregion

        #region Constructors
        internal BaseServiceConfiguration(
            ServiceConfigurationDto context,
            Dictionary<Type, ITypeFactory> factories,
            IEnumerable<SetupAction> actions)
        {
            this.context = context;
            this.TypeFactory = factories[this.context.TypeFactory];
            this.SetupActions = actions
                .Where(action => this.Key.Inherits(action.Key))
                .OrderBy(action => action.Order)
                .ToArray();
        }
        #endregion

        #region IServiceConfiguration Implementation
        /// <inheritdoc />
        public abstract IServiceManager BuildServiceManager(GuppyServiceProvider provider, Type[] generics);
        #endregion
    }
}
