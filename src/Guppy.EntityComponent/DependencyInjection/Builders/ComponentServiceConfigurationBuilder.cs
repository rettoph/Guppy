using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public sealed class ComponentServiceConfigurationBuilder<TService> : ServiceConfigurationBuilder<TService>
        where TService : class
    {
        #region Constructors
        public ComponentServiceConfigurationBuilder(String name, ServiceProviderBuilder services) : base(name, services)
        {
        }
        #endregion

        #region RegisterComponentConfiguration Methods
        public ComponentServiceConfigurationBuilder<TService> RegisterComponentConfiguration(Action<ComponentConfigurationBuilder> builder)
        {
            ComponentConfigurationBuilder componentConfiguration = this.services.RegisterComponent(this.Name);
            builder(componentConfiguration);

            return this;
        }
        #endregion
    }
}
