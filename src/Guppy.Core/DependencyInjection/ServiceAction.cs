using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    /// <summary>
    /// Represents an aaction to be preformed on an action
    /// during runtime. This will generally either be for setup
    /// or building.
    /// </summary>
    public sealed class ServiceAction
    {
        #region Private Fields
        private readonly Action<Object, ServiceProvider, ServiceConfiguration> _action;
        #endregion

        #region Public Properties
        /// <summary>
        /// A custom identifier that allows for mass
        /// action application to inherited services.
        /// </summary>
        public readonly ServiceActionKey Key;

        /// <summary>
        /// The action type, telling the service provider
        /// at which point during the service lifecycle
        /// to run the current action.
        /// </summary>
        public readonly ServiceActionType Type;

        /// <summary>
        /// The order in which the current action should be
        /// preformed compared to other actions.
        /// </summary>
        public readonly Int32 Order;
        #endregion

        #region Constructor
        public ServiceAction(ServiceActionKey key, ServiceActionType type, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order)
        {
            _action = action;
            this.Key = key;
            this.Type = type;
            this.Order = order;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Preform the current action on the recieved
        /// service instance.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="instance"></param>
        public void Excecute(Object instance, ServiceProvider provider, ServiceConfiguration configuration)
            => _action.Invoke(instance, provider, configuration);
        #endregion

        #region Static Helper Methods
        public static ServiceAction Builder(ServiceActionKey key, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order)
            => new ServiceAction(key, ServiceActionType.Builder, action, order);

        public static ServiceAction Setup(ServiceActionKey key, Action<Object, ServiceProvider, ServiceConfiguration> action, Int32 order)
            => new ServiceAction(key, ServiceActionType.Setup, action, order);
        #endregion
    }
}
