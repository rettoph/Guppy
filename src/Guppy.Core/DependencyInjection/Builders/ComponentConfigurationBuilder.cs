using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public sealed class ComponentConfigurationBuilder : IFluentOrderable<ComponentConfigurationBuilder>
    {
        #region Private Fields
        private Type _assignableEntityType;
        #endregion

        #region Public Properties
        /// <summary>
        /// The ServiceConfiguration name of the Component to be bound.
        /// </summary>
        public String ComponentServiceName { get; }

        /// <summary>
        /// All <see cref="TypeFactory"/>s who's <see cref="TypeFactory.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will be bound to the defined
        /// <see cref="ComponentServiceName"/>.
        /// </summary>
        public Type AssignableEntityType
        {
            get => _assignableEntityType;
            set => this.SetAssignableEntityType(value);
        }

        public Int32 Order { get; set; }
        #endregion

        #region Constructors
        internal ComponentConfigurationBuilder(String componentServiceName)
        {
            this.ComponentServiceName = componentServiceName;
        }
        #endregion

        #region SetAssignableEntityFactoryType Methods
        public ComponentConfigurationBuilder SetAssignableEntityType(Type assignableEntityType)
        {
            typeof(IEntity).ValidateAssignableFrom(assignableEntityType);

            _assignableEntityType = assignableEntityType;

            return this;
        }
        public ComponentConfigurationBuilder SetAssignableEntityType<TEntity>()
            where TEntity : IEntity
        {
            this.SetAssignableEntityType(typeof(TEntity));

            return this;
        }
        #endregion

        #region Build Methods
        public ComponentConfiguration Build(List<ComponentFilter> allFilters, Dictionary<String, ServiceConfiguration<GuppyServiceProvider>> services)
        {
            ServiceConfiguration<GuppyServiceProvider> componentServiceConfiguration = services[this.ComponentServiceName];

            ComponentFilter[] filters = allFilters.Where(f => {
                return componentServiceConfiguration.TypeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(f.AssignableComponentType);
            }).ToArray();

            Type assignableEntityType = this.AssignableEntityType ?? typeof(IEntity);
            ServiceConfiguration<GuppyServiceProvider>[] entityConfigurations = services.Values.Where(sc =>
            {
                return sc.TypeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(assignableEntityType);
            }).ToArray();

            return new ComponentConfiguration(
                componentServiceConfiguration,
                entityConfigurations,
                this.Order,
                filters);
        }
        #endregion
    }
}
