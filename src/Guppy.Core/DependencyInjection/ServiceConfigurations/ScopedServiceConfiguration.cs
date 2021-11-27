using Guppy.DependencyInjection.Actions;
using Guppy.DependencyInjection.Builders;
using Guppy.DependencyInjection.Interfaces;
using Guppy.DependencyInjection.ServiceManagers;
using Guppy.DependencyInjection.TypeFactories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.ServiceConfigurations
{
    internal sealed class ScopedServiceConfiguration : BaseServiceConfiguration
    {
        #region Constructors
        public ScopedServiceConfiguration(
            ServiceConfigurationBuilder context, 
            Dictionary<Type, ITypeFactory> factories, 
            IEnumerable<SetupAction> actions) : base(context, factories, actions)
        {
        }
        #endregion

        #region IServiceConfiguration Implementation
        public override IServiceManager BuildServiceManager(GuppyServiceProvider provider, Type[] generics)
            => new ScopedServiceManager(this, provider, generics);
        #endregion
    }
}
