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
        public readonly Func<IEntity, GuppyServiceProvider, Type, Boolean> Method;
        public readonly Func<IServiceConfiguration, Boolean> Validator;
        public readonly Int32 Order;

        public ComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, GuppyServiceProvider, Type, Boolean> method,
            Func<IServiceConfiguration, Boolean> validator,
            Int32 order)
        {
            typeof(IComponent).ValidateAssignableFrom(componentServiceConfigurationKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.Method = method;
            this.Validator = validator ?? DefaultValidator;
            this.Order = order;
        }
        public ComponentFilter(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, GuppyServiceProvider, Type, Boolean> method,
            Int32 order) : this(componentServiceConfigurationKey, method, ComponentFilter.DefaultValidator, order)
        {
        }

        private static Boolean DefaultValidator(IServiceConfiguration component)
            => true;
    }
}
