using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Contexts;
using Lidgren.Network;
using Guppy.Network.Interfaces;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Class used to contain a <see cref="NetOutgoingMessageContext"/>,
    /// <see cref="NetOutgoingMessage"/>, and 0 to many <see cref="IUser"/>
    /// instances to broadcast the message to.
    /// </summary>
    internal sealed class NetOutgoingMessageContainer
    {
        #region Public Fields
        public NetOutgoingMessageContext Context;
        public NetOutgoingMessage Message;
        public IEnumerable<NetConnection> Recipients;
        #endregion

        #region Constructors
        public NetOutgoingMessageContainer(
            NetOutgoingMessageContext context, 
            NetOutgoingMessage message,
            IEnumerable<NetConnection> recipients)
        {
            this.Context = context;
            this.Message = message;
            this.Recipients = recipients;
        }
        #endregion
    }
}
