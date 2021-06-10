using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Contexts
{
    public class ComponentConfigurationContext
    {
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        public readonly ServiceConfigurationKey EntityServiceConfigurationKey;

        public ComponentConfigurationContext(
            ServiceConfigurationKey componentServiceConfigurationKey,
            ServiceConfigurationKey entityServicConfigurationeKey)
        {
            ExceptionHelper.ValidateAssignableFrom<IComponent>(componentServiceConfigurationKey.Type);
            ExceptionHelper.ValidateAssignableFrom<IEntity>(entityServicConfigurationeKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.EntityServiceConfigurationKey = entityServicConfigurationeKey;
        }

        public ComponentConfiguration CreateComponentConfiguration(
            Dictionary<ServiceConfigurationKey, IServiceConfiguration[]> serviceConfigurations,
            IEnumerable<ComponentFilter> componentFilters)
                => new ComponentConfiguration(this, serviceConfigurations, componentFilters);
    }
}
