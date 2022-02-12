using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal sealed class TransientServiceConfiguration : ServiceConfiguration
    {
        public TransientServiceConfiguration(
            Type type,
            TypeFactory typeFactory,
            ServiceLifetime lifetime,
            Type[] aliases,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups) : base(type, typeFactory, lifetime, aliases, setups)
        {
        }

        public override ServiceConfigurationManager BuildServiceCofigurationManager(ServiceProvider provider)
        {
            return new TransientServiceConfigurationManager(this, provider);
        }
    }
}
