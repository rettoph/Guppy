using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public sealed class ComponentFilter
    {
        #region Delegates
        public delegate Boolean MethodDelegate(IEntity entity, ServiceProvider provider, ServiceConfiguration componentConfiguration);
        public delegate Boolean FilterDelegate(ServiceConfiguration componentConfiguration, ServiceConfiguration entityConfiguration);
        #endregion

        #region Public Fields
        /// <summary>
        /// The ServiceConfiguration name of the Component to be bound.
        /// </summary>
        public readonly Type AssignableComponentType;

        /// <summary>
        /// All <see cref="TypeFactory"/>s who's <see cref="TypeFactory.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will be bound to the defined
        /// <see cref="ComponentName"/>.
        /// </summary>
        public readonly Type AssignableEntityType;

        /// <summary>
        /// The method to be ran on entity creation to indicate
        /// whether or not the component should be added to an entity.
        /// </summary>
        public readonly MethodDelegate Method;

        /// <summary>
        /// A simple method ran on configre time to determin if the reciever component
        /// <see cref="ServiceConfiguration{ServiceProvider}"/> should bind to the recieved
        /// entity.
        /// </summary>
        public readonly FilterDelegate Filter;

        /// <summary>
        /// The order in which the filter will be ran.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Constructors
        internal ComponentFilter(Type assignableComponentType, Type assignableEntityType, MethodDelegate method, FilterDelegate filter, Int32 order)
        {
            this.AssignableComponentType = assignableComponentType;
            this.AssignableEntityType = assignableEntityType;
            this.Method = method;
            this.Filter = filter;
            this.Order = order;
        }
        #endregion
    }
}
