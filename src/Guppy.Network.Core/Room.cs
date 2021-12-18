using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// A room is a targetable 
    /// </summary>
    public sealed class Room
    {
        #region Private Fields
        private Boolean _isScopedLinked;
        private MessageService _messages;
        #endregion

        #region Public Properties
        public Byte Id { get; init; }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to link the room to the current scope.
        /// If a scope is already linked, this will fail,
        /// otherwise, the provider's MessageManager will
        /// be utilized for incoming messages.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public Boolean TryLinkScope(ServiceProvider provider)
        {
            if(_isScopedLinked)
            {
                return false;
            }

            provider.Service(out _messages);

            _isScopedLinked = true;
            return true;
        }

        /// <summary>
        /// Process an incoming message
        /// </summary>
        /// <param name="message"></param>
        public void ProcessIncoming(Message message)
        {
            _messages.ProcessIncoming(message);
        }
        #endregion
    }
}
