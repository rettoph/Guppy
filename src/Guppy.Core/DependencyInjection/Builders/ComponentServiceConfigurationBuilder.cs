using DotNetUtils.DependencyInjection.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public sealed class ComponentServiceConfigurationBuilder<TService> : ServiceConfigurationBuilder<TService, GuppyServiceProvider>
        where TService : class
    {
        #region Constructors
        public ComponentServiceConfigurationBuilder(String name, ServiceProviderBuilder<GuppyServiceProvider> services) : base(name, services)
        {
        }
        #endregion

        #region RegisterComponentConfiguration Methods
        public ComponentServiceConfigurationBuilder<TService> RegisterComponentConfiguration(Action<ComponentConfigurationBuilder> builder)
        {
            ComponentConfigurationBuilder componentConfiguration = (this.services as GuppyServiceProviderBuilder).RegisterComponent(this.Name);
            builder(componentConfiguration);

            return this;
        }
        #endregion
    }
}
