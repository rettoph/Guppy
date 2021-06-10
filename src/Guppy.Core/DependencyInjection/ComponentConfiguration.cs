using Guppy.DependencyInjection.Contexts;
using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ComponentConfiguration
    {
        public readonly ServiceConfigurationKey EntityServiceConfigurationKey;
        public readonly IServiceConfiguration ComponentServiceConfiguration;
        public readonly ComponentFilter[] ComponentFilters;

        internal ComponentConfiguration(
            ComponentConfigurationContext context,
            Dictionary<ServiceConfigurationKey, IServiceConfiguration[]> serviceConfigurations,
            IEnumerable<ComponentFilter> componentFilters)
        {
            this.EntityServiceConfigurationKey = context.EntityServiceConfigurationKey;
            this.ComponentServiceConfiguration = serviceConfigurations[context.ComponentServiceConfigurationKey][0];
            this.ComponentFilters = componentFilters
                .Where(f => context.ComponentServiceConfigurationKey.Inherits(f.ComponentServiceConfigurationKey))
                .OrderBy(f => f.Order)
                .ToArray();
        }

        public Boolean Validate(IEntity instance, ServiceProvider provider)
        {
            foreach (ComponentFilter filter in this.ComponentFilters)
                if (!filter.Method(instance, provider, this.ComponentServiceConfiguration.TypeFactory.Type))
                    return false;

            return true;
        }
    }
}
