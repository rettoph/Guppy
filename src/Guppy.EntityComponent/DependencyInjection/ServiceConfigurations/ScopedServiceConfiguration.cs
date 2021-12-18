﻿using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    internal class ScopedServiceConfiguration : ServiceConfiguration
    {
        public ScopedServiceConfiguration(
            string name, 
            TypeFactory typeFactory,
            ServiceLifetime lifetime, 
            string[] cacheNames,
            CustomAction<ServiceConfiguration, IServiceConfigurationBuilder>[] setups) : base(name, typeFactory, lifetime, cacheNames, setups)
        {
        }

        public override ServiceConfigurationManager BuildServiceCofigurationManager(ServiceProvider provider)
        {
            return new ScopedServiceConfigurationManager(this, provider);
        }
    }
}
