using DotNetUtils.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public sealed class GuppyServiceProvider : ServiceProvider<GuppyServiceProvider>
    {
        public readonly Dictionary<UInt32, ComponentConfiguration[]> EntityComponentConfigurations;

        public GuppyServiceProvider(
            Dictionary<String, ServiceConfiguration<GuppyServiceProvider>> services,
            Dictionary<UInt32, ComponentConfiguration[]> entityComponentConfigurations) : base(services)
        {
            this.EntityComponentConfigurations = entityComponentConfigurations;
        }
    }
}
