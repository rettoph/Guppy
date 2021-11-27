using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Interfaces;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public class ComponentFilterBuilder : IOrderable<ComponentFilterBuilder>, IBuilder<ComponentFilter>
    {
        #region Private Fields
        private Func<IServiceConfiguration, Boolean> _validator;
        #endregion

        #region Public Fields
        public readonly ServiceConfigurationKey ComponentServiceConfigurationKey;
        public readonly Func<IEntity, GuppyServiceProvider, Type, Boolean> Method;
        #endregion

        #region Public Properties
        public Func<IServiceConfiguration, Boolean> Validator
        {
            get => _validator;
            set => this.SetValidator(value);
        }

        public Int32 Order { get; set; }
        #endregion

        public ComponentFilterBuilder(
            ServiceConfigurationKey componentServiceConfigurationKey,
            Func<IEntity, GuppyServiceProvider, Type, Boolean> method)
        {
            typeof(IComponent).ValidateAssignableFrom(componentServiceConfigurationKey.Type);

            this.ComponentServiceConfigurationKey = componentServiceConfigurationKey;
            this.Method = method;

            this.SetValidator(DefaultValidator);
        }

        #region Helper Methods
        public ComponentFilterBuilder SetValidator(Func<IServiceConfiguration, Boolean> validator)
        {
            _validator = validator;

            return this;
        }

        public ComponentFilter Build()
        {
            return new ComponentFilter(
                this.ComponentServiceConfigurationKey,
                this.Method,
                this.Validator,
                this.Order);
        }
        #endregion

        private static Boolean DefaultValidator(IServiceConfiguration component)
            => true;
    }
}
