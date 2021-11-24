using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Dtos
{
    public class ComponentConfigurationDto
    {
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        public readonly ServiceConfigurationKey EntityServiceConfigurationKey;

        /// <summary>
        /// The priority value for this specific descriptor.
        /// All components will be sorted by priority when created
        /// for an entity.
        /// </summary>
        public readonly Int32 Order;

        public ComponentConfigurationDto(
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServicConfigurationeKey,
            Int32 order = 0)
        {
            typeof(IComponent).ValidateAssignableFrom(componentServiceConfigurationKey.Type);
            typeof(IEntity).ValidateAssignableFrom(entityServicConfigurationeKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.EntityServiceConfigurationKey = entityServicConfigurationeKey;
            this.Order = order;
        }

        public ComponentConfiguration CreateComponentConfiguration(
            Dictionary<ServiceConfigurationKey, IServiceConfiguration> serviceConfigurations,
            IEnumerable<ComponentFilter> componentFilters)
                => new ComponentConfiguration(this, serviceConfigurations, componentFilters);
    }
}
