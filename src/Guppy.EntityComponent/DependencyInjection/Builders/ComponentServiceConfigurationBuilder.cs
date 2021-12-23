using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public sealed class ComponentServiceConfigurationBuilder<TComponent> : ServiceConfigurationBuilder<TComponent, ComponentServiceConfigurationBuilder<TComponent>>
        where TComponent : class, IComponent
    {
        #region Constructors
        public ComponentServiceConfigurationBuilder(String name, ServiceProviderBuilder services) : base(name, services)
        {
        }
        #endregion

        #region RegisterComponentConfiguration Methods
        public ComponentServiceConfigurationBuilder<TComponent> RegisterComponentConfiguration(Action<ComponentConfigurationBuilder> builder)
        {
            ComponentConfigurationBuilder componentConfiguration = this.services.RegisterComponent(this.Name);
            builder(componentConfiguration);

            return this;
        }
        #endregion
    }
}
