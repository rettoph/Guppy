using Guppy.EntityComponent.Interfaces;
using Minnow.General;
using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public abstract class ComponentConfigurationBuilder : IOrderable
    {
        #region Public Properties
        public Int32 Order { get; set; }
        #endregion

        #region Build Method
        public abstract ComponentConfiguration Build(List<ComponentFilter> allFilters, DoubleDictionary<String, UInt32, ServiceConfiguration> services);
        #endregion
    }

    public sealed class ComponentConfigurationBuilder<TComponent> : ComponentConfigurationBuilder, IFluentOrderable<ComponentConfigurationBuilder<TComponent>>
        where TComponent : class, IComponent
    {
        #region Private Fields
        private Type _assignableEntityType;
        private ServiceProviderBuilder _services;
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
        #endregion

        #region Constructors
        internal ComponentConfigurationBuilder(String componentServiceName, ServiceProviderBuilder services)
        {
            _services = services;

            this.ComponentServiceName = componentServiceName;
        }
        #endregion

        #region SetAssignableEntityFactoryType Methods
        public ComponentConfigurationBuilder<TComponent> SetAssignableEntityType(Type assignableEntityType)
        {
            typeof(IEntity).ValidateAssignableFrom(assignableEntityType);

            _assignableEntityType = assignableEntityType;

            return this;
        }
        public ComponentConfigurationBuilder<TComponent> SetAssignableEntityType<TEntity>()
            where TEntity : IEntity
        {
            this.SetAssignableEntityType(typeof(TEntity));

            return this;
        }
        #endregion

        #region RegisterComponentTypeFilter Methods
        public ComponentConfigurationBuilder<TComponent> RegisterComponentFilter(Type assignableComponentType, Action<ComponentFilterBuilder> builder)
        {
            typeof(TComponent).ValidateAssignableFrom(assignableComponentType);

            ComponentFilterBuilder filter = _services.RegisterComponentFilter(assignableComponentType);
            builder(filter);

            return this;
        }

        public ComponentConfigurationBuilder<TComponent> RegisterComponentFilter<TAssignableComponent>(Action<ComponentFilterBuilder> builder)
            where TAssignableComponent : class, TComponent
        {
            ComponentFilterBuilder filter = _services.RegisterComponentFilter<TAssignableComponent>();
            builder(filter);

            return this;
        }

        public ComponentConfigurationBuilder<TComponent> RegisterComponentFilter(Action<ComponentFilterBuilder> builder)
        {
            return this.RegisterComponentFilter<TComponent>(builder);
        }
        #endregion

        #region RegisterService Methods
        /// <summary>
        /// Register a new service for the component
        /// </summary>
        /// <param name="name"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ComponentConfigurationBuilder<TComponent> RegisterService(String name, Action<ServiceConfigurationBuilder<TComponent>> builder)
        {
            ServiceConfigurationBuilder<TComponent> service = _services.RegisterService<TComponent>(name);
            service.SetLifetime(ServiceLifetime.Transient);
            builder(service);

            return this;
        }
        /// <summary>
        /// Register a new service for the component.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public ComponentConfigurationBuilder<TComponent> RegisterService(Action<ServiceConfigurationBuilder<TComponent>> builder)
        {
            this.RegisterService(typeof(TComponent).FullName, builder);

            return this;
        }
        #endregion

        #region Build Methods
        public override ComponentConfiguration Build(List<ComponentFilter> allFilters, DoubleDictionary<String, UInt32, ServiceConfiguration> services)
        {
            ServiceConfiguration componentServiceConfiguration = services[this.ComponentServiceName];

            ComponentFilter[] filters = allFilters.Where(f => {
                return componentServiceConfiguration.TypeFactory.Type.IsAssignableToOrSubclassOfGenericDefinition(f.AssignableComponentType)
                    && f.Filter(componentServiceConfiguration);
            }).ToArray();

            Type assignableEntityType = this.AssignableEntityType ?? typeof(IEntity);
            ServiceConfiguration[] entityConfigurations = services.Values.Where(sc =>
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
