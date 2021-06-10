using Guppy.DependencyInjection.ServiceConfigurations;
using Guppy.Interfaces;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public class ComponentFilter
    {
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        public readonly Func<IEntity, ServiceProvider, Type, Boolean> Method;
        public readonly Func<IServiceConfiguration, IServiceConfiguration, Boolean> Validator;
        public readonly Int32 Order;

        public ComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Func<IServiceConfiguration, IServiceConfiguration, Boolean> validator,
            Int32 order)
        {
            ExceptionHelper.ValidateAssignableFrom<IComponent>(componentServiceConfigurationKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.Method = method;
            this.Validator = validator ?? DefaultValidator;
            this.Order = order;
        }
        public ComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Int32 order) : this(componentServiceConfigurationKey, method, ComponentFilter.DefaultValidator, order)
        {
        }

        private static Boolean DefaultValidator(IServiceConfiguration component, IServiceConfiguration entity)
            => true;
    }
}
