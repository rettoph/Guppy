using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class SingletonServiceConfiguration : ServiceConfiguration
    {
        public SingletonServiceConfiguration(
            Type type,
            TypeFactory typeFactory,
            ServiceLifetime lifetime,
            Type[] aliases,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups) : base(type, typeFactory, lifetime, aliases, setups)
        {
        }

        public override ServiceConfigurationManager BuildServiceCofigurationManager(ServiceProvider provider)
        {
            if (provider.IsRoot)
                return new SingletonServiceConfigurationManager(this, provider);
            else
                return provider.Root.GetServiceConfigurationManager(this);
        }
    }
}
