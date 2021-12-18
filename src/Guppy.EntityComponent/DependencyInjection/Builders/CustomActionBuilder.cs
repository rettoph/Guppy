using Minnow.General.Interfaces;
using Guppy.EntityComponent.DependencyInjection.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public class CustomActionBuilder<T, TMethodArgs, TFilterArgs> : ICustomActionBuilder<TMethodArgs, TFilterArgs>, IFluentOrderable<CustomActionBuilder<T, TMethodArgs, TFilterArgs>>
        where T : class
    {
        #region Public Properties
        /// <inheritdoc />
        public Type AssignableFactoryType { get; }

        /// <inheritdoc />
        public Int32 Order { get; set; }

        /// <summary>
        /// The action to be invoked.
        /// </summary>
        public CustomAction<T, TMethodArgs, TFilterArgs>.MethodDelegate Method { get; set; }

        /// <summary>
        /// A custom filter ran at runtime to confirm if the
        /// recieved <see cref="Type"/> should actually be bound
        /// to the described FactoryAction.
        /// </summary>
        public CustomAction<TMethodArgs, TFilterArgs>.FilterDelegate Filter { get; set; }
        #endregion

        #region Constructors
        public CustomActionBuilder(
            Type assignableFactoryType)
        {
            typeof(T).IsAssignableFrom(assignableFactoryType);

            this.AssignableFactoryType = assignableFactoryType;
        }
        #endregion

        #region SetMethod Methods
        /// <summary>
        /// Set the <see cref="Method"/> value.
        /// </summary>
        /// <param name="method">The action to be invoked.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TMethodArgs, TFilterArgs> SetMethod(CustomAction<T, TMethodArgs, TFilterArgs>.MethodDelegate method)
        {
            this.Method = method;

            return this;
        }
        #endregion

        #region SetFilter Methods
        /// <summary>
        /// Set the <see cref="Filter"/> value.
        /// </summary>
        /// <param name="filter">A custom filter ran at runtime to confirm if the
        /// recieved <see cref="Type"/> should actually be bound
        /// to the described FactoryAction.</param>
        /// <returns></returns>
        public CustomActionBuilder<T, TMethodArgs, TFilterArgs> SetFilter(CustomAction<TMethodArgs, TFilterArgs>.FilterDelegate filter)
        {
            this.Filter = filter;

            return this;
        }
        #endregion

        #region IFactoryActionBuilder<TArgs> Implementation
        /// <inheritdoc />
        CustomAction<TMethodArgs, TFilterArgs> ICustomActionBuilder<TMethodArgs, TFilterArgs>.Build()
        {
            Debug.Assert(this.Method is not null, $"{this.GetType().GetPrettyName()}::{nameof(ICustomActionBuilder<TMethodArgs, TFilterArgs>.Build)} - {nameof(Method)} should be defined.");

            void DefaultMethod(T instance, ServiceProvider provider, TMethodArgs args)
            {

            }

            Boolean DefaultFilter(TFilterArgs type)
            {
                return true;
            }

            return new CustomAction<T, TMethodArgs, TFilterArgs>(
                this.AssignableFactoryType,
                this.Method ?? DefaultMethod,
                this.Filter ?? DefaultFilter,
                this.Order);
        }
        #endregion
    }
}
