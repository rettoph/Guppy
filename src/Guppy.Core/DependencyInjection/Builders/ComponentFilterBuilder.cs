using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Guppy.DependencyInjection.Builders
{
    public sealed class ComponentFilterBuilder : IFluentOrderable<ComponentFilterBuilder>
    {
        #region Private Fields
        private Type _assignableEntityType;
        private ComponentFilter.MethodDelegate _method;
        private ComponentFilter.FilterDelegate _filter;
        #endregion

        #region Public Properties
        /// <summary>
        /// The ServiceConfiguration name of the Component to be bound.
        /// </summary>
        public Type AssignableComponentType { get; }

        /// <summary>
        /// All <see cref="TypeFactory"/>s who's <see cref="TypeFactory.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will be bound to the defined
        /// <see cref="ComponentName"/>.
        /// </summary>
        public Type AssignableEntityType
        {
            get => _assignableEntityType;
            set => this.SetAssignableEntityType(value);
        }

        /// <summary>
        /// The method to be ran on entity creation to indicate
        /// whether or not the component should be added to an entity.
        /// </summary>
        public ComponentFilter.MethodDelegate Method
        {
            get => _method;
            set => this.SetMethod(value);
        }

        /// <summary>
        /// A simple method ran on configre time to determin if the reciever component
        /// <see cref="ServiceConfiguration{GuppyServiceProvider}"/> should bind to the recieved
        /// entity.
        /// </summary>
        public ComponentFilter.FilterDelegate Filter
        {
            get => _filter;
            set => this.SetFilter(value);
        }

        public Int32 Order { get; set; }
        #endregion

        #region Constructors
        internal ComponentFilterBuilder(Type assignableComponentType)
        {
            this.AssignableComponentType = assignableComponentType;
        }
        #endregion

        #region SetAssignableEntityFactoryType Methods
        public ComponentFilterBuilder SetAssignableEntityType(Type assignableEntityFactoryType)
        {
            typeof(IEntity).ValidateAssignableFrom(assignableEntityFactoryType);

            _assignableEntityType = assignableEntityFactoryType;

            return this;
        }
        public ComponentFilterBuilder SetAssignableEntityType<TAssignableEntityType>()
        {
            return this.SetAssignableEntityType(typeof(TAssignableEntityType));
        }
        #endregion

        #region SetMethod Methods
        public ComponentFilterBuilder SetMethod(ComponentFilter.MethodDelegate method)
        {
            _method = method;

            return this;
        }
        #endregion

        #region SetValidator Methods
        public ComponentFilterBuilder SetFilter(ComponentFilter.FilterDelegate filter)
        {
            _filter = filter;

            return this;
        }
        #endregion

        #region Build Methods
        internal ComponentFilter Build()
        {
            Debug.Assert(this.Method is not null, $"{nameof(ComponentFilter)}::{nameof(Build)} - {nameof(Method)} should not be null.");

            Boolean DefaultMethod(IEntity entity, GuppyServiceProvider provider, ServiceConfiguration<GuppyServiceProvider> componentConfiguration)
            {
                return true;
            }

            Boolean DefaultFilter(ServiceConfiguration<GuppyServiceProvider> componentConfiguration, ServiceConfiguration<GuppyServiceProvider> entityConfiguration)
            {
                return true;
            }

            return new ComponentFilter(
                this.AssignableComponentType,
                this.AssignableEntityType ?? typeof(IEntity),
                this.Method ?? DefaultMethod,
                this.Filter ?? DefaultFilter,
                this.Order);
        }
        #endregion
    }
}
