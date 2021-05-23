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
        public readonly Func<ServiceConfiguration, ServiceConfiguration, Boolean> Validator;
        public readonly Int32 Order;

        public ComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, ServiceProvider, Type, Boolean> method,
            Func<ServiceConfiguration, ServiceConfiguration, Boolean> validator,
            Int32 order)
        {
            ExceptionHelper.ValidateAssignableFrom<IComponent>(componentServiceConfigurationKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.Method = method;
            this.Validator = validator ?? DefaultValidator;
            this.Order = order;
        }

        private static Boolean DefaultValidator(ServiceConfiguration component, ServiceConfiguration entity)
            => true;
    }
}
