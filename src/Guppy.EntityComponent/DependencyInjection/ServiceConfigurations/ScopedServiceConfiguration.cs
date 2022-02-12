using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class ScopedServiceConfiguration : ServiceConfiguration
    {
        public ScopedServiceConfiguration(
            Type type, 
            TypeFactory typeFactory,
            ServiceLifetime lifetime, 
            Type[] aliases,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups) : base(type, typeFactory, lifetime, aliases, setups)
        {
        }

        public override ServiceConfigurationManager BuildServiceCofigurationManager(ServiceProvider provider)
        {
            return new ScopedServiceConfigurationManager(this, provider);
        }
    }
}
