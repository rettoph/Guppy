﻿using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class SingletonServiceConfiguration : ServiceConfiguration
    {
        public SingletonServiceConfiguration(
            string name,
            TypeFactory typeFactory,
            ServiceLifetime lifetime,
            string[] cacheNames,
            CustomAction<ServiceConfiguration, ServiceConfigurationBuilder>[] setups) : base(name, typeFactory, lifetime, cacheNames, setups)
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
