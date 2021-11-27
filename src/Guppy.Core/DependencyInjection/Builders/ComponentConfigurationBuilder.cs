using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public class ComponentConfigurationBuilder : IOrderable<ComponentConfigurationBuilder>
    {
        #region Private Fields
        private ServiceConfigurationKey _entityServiceConfigurationKey;
        #endregion

        #region Public Fields
        /// <summary>
        /// The desired <see cref="IComponent"/>'s <see cref="ServiceConfigurationKey"/>.
        /// </summary>
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        #endregion

        #region Public Properties
        /// <summary>
        /// The desired <see cref="IEntity"/>'s <see cref="ServiceConfigurationKey"/>.
        /// </summary>
        public ServiceConfigurationKey EntityServiceConfigurationKey 
        {
            get => _entityServiceConfigurationKey;
            set => this.SetEntityServiceConfigurationKey(value);
        }

        /// <summary>
        /// The order value for this specific descriptor.
        /// All components will be sorted by priority when created
        /// for an entity.
        /// </summary>
        public Int32 Order { get; set; }
        #endregion

        #region Constructor
        internal ComponentConfigurationBuilder(ServiceConfigurationKey componentServiceConfigurationKey)
        {
            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;

            this.SetEntityServiceConfigurationKey(ServiceConfigurationKey.From<IEntity>());
        }
        #endregion

        /// <summary>
        /// Set the desired <see cref="IEntity"/>'s <see cref="ServiceConfigurationKey"/>.
        /// </summary>
        /// <param name="entityServicConfigurationeKey"></param>
        /// <returns></returns>
        public ComponentConfigurationBuilder SetEntityServiceConfigurationKey(ServiceConfigurationKey entityServicConfigurationeKey)
        {
            typeof(IEntity).ValidateAssignableFrom(entityServicConfigurationeKey.Type);

            _entityServiceConfigurationKey = entityServicConfigurationeKey;

            return this;
        }

        /// <summary>
        /// Set the desired <see cref="IEntity"/>'s <see cref="ServiceConfigurationKey"/>.
        /// </summary>
        /// <param name="entityServicConfigurationeKey"></param>
        /// <returns></returns>
        public ComponentConfigurationBuilder SetEntityServiceConfigurationKey<TEntityServicConfigurationeKeyType>(String entityServicConfigurationeKeyName = null)
            where TEntityServicConfigurationeKeyType : IEntity
        {
            _entityServiceConfigurationKey = ServiceConfigurationKey.From< TEntityServicConfigurationeKeyType>(entityServicConfigurationeKeyName);

            return this;
        }

        /// <summary>
        /// Construct a new <see cref="ComponentConfiguration"/> instance based on the internal
        /// values.
        /// </summary>
        /// <param name="serviceConfigurations"></param>
        /// <param name="componentFilters"></param>
        /// <returns></returns>
        internal ComponentConfiguration Build(
            Dictionary<ServiceConfigurationKey, IServiceConfiguration> serviceConfigurations,
            IEnumerable<ComponentFilter> componentFilters)
                => new ComponentConfiguration(this, serviceConfigurations, componentFilters);
    }
}
