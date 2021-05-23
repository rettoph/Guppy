using Guppy.DependencyInjection.Descriptors;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ComponentConfiguration : ComponentConfigurationDescriptor
    {
        public readonly ServiceConfiguration ComponentServiceConfiguration;
        public readonly ComponentFilter[] ComponentFilters;

        public ComponentConfiguration(
            ComponentConfigurationDescriptor descriptor,
            ServiceConfiguration componentServiceConfiguration,
            IEnumerable<ComponentFilter> componentFilters
        ) : base(descriptor.ComponentServiceConfigurationKey, descriptor.EntityServiceConfigurationKey)
        {
            this.ComponentServiceConfiguration = componentServiceConfiguration;
            this.ComponentFilters = componentFilters.OrderBy(f => f.Order).ToArray();
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
