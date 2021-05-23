using Guppy.DependencyInjection.Enums;
using System;

namespace Guppy.DependencyInjection
{
    public class ServiceAction
    {
        #region Public Properties
        /// <summary>
        /// A custom identifier that allows for mass
        /// action application to inherited services.
        /// </summary>
        public readonly ServiceConfigurationKey Key;

        /// <summary>
        /// The action type, telling the service provider
        /// at which point during the service lifecycle
        /// to run the current action.
        /// </summary>
        public readonly ServiceActionType Type;

        /// <summary>
        /// The actual action to be invoked
        /// </summary>
        public readonly Action<Object, ServiceProvider, ServiceConfiguration> Method;

        /// <summary>
        /// The order in which the current action should be
        /// preformed compared to other actions of the same
        /// type.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Constructor
        public ServiceAction(
            ServiceConfigurationKey key,
            ServiceActionType type,
            Action<Object, ServiceProvider, ServiceConfiguration> method,
            Int32 order = 0)
        {
            this.Key = key;
            this.Type = type;
            this.Method = method;
            this.Order = order;
        }
        #endregion
    }
}
