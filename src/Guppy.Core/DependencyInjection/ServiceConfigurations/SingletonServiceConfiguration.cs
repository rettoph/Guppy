﻿using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Contexts;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceConfigurations
{
    internal sealed class SingletonServiceConfiguration : BaseServiceConfiguration
    {
        #region Constructors
        public SingletonServiceConfiguration(
            ServiceConfigurationContext context, 
            Dictionary<Type, ITypeFactory> factories, 
            IEnumerable<SetupAction> actions) : base(context, factories, actions)
        {
        }
        #endregion

        #region IServiceConfiguration Implementation
        public override IServiceManager BuildServiceManager(ServiceProvider provider, Type[] generics)
        {
            if (provider.IsRoot)
                return new SingletonServiceManager(this, provider, generics);
            else
                return provider.Root.GetServiceManager(this, generics);
        }
        #endregion
    }
}
