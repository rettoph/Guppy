using Guppy.EntityComponent.DependencyInjection.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
{
    public abstract class CustomAction<TMethodArgs, TFilterArgs>
    {
        #region Delegates
        public delegate Boolean FilterDelegate(TFilterArgs args);
        #endregion

        #region Public Fields
        /// <summary>
        /// All <see cref="TypeFactoryBuilder"/>s who's <see cref="TypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>
        /// </summary>
        public readonly Type AssignableFactoryType;

        /// <summary>
        /// A custom filter ran at runtime to confirm if the
        /// recieved <see cref="Type"/> should actually be bound
        /// to the described FactoryAction.
        /// </summary>
        public readonly FilterDelegate Filter;

        /// <summary>
        /// The order in which the current action should be ran.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Constructors
        protected CustomAction(
            Type assignableFactoryType,
            FilterDelegate filter,
            Int32 order)
        {
            this.AssignableFactoryType = assignableFactoryType;
            this.Filter = filter;
            this.Order = order;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Invoke the current acton with the given args & object instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="provider"></param>
        /// <param name="args"></param>
        public abstract void Invoke(Object instance, ServiceProvider provider, TMethodArgs args);
        #endregion
    }

    public class CustomAction<T, TMethodArgs, TFilterArgs> : CustomAction<TMethodArgs, TFilterArgs>
        where T : class
    {
        #region Delegates
        public delegate void MethodDelegate(T instance, ServiceProvider provider, TMethodArgs args);
        #endregion

        #region Public Fields
        /// <summary>
        /// The action to be invoked.
        /// </summary>
        public readonly MethodDelegate Method;
        #endregion

        #region Constructor
        public CustomAction(Type assignableFactoryType, MethodDelegate method, FilterDelegate filter, Int32 order) : base(assignableFactoryType, filter, order)
        {
            this.Method = method;
        }
        #endregion

        #region Helper Methods
        public override void Invoke(Object instance, ServiceProvider provider, TMethodArgs args)
        {
            if(instance is T casted)
            {
                this.Method(casted, provider, args);
            }
            else
            {
                throw new ArgumentException($"{this.GetType().GetPrettyName()}::{nameof(Invoke)} - Param '{nameof(instance)}' is of type '{instance.GetType().GetPrettyName()}', which is not castable to '{nameof(T)}'");
            }
        }
        #endregion
    }
}
